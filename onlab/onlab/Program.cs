using OfficeOpenXml;
using OfficeOpenXml.Style;
using onlab.Classifier;
using onlab.Functions;
using onlab.Functions.Enums;
using SigStat.Common;
using SigStat.Common.Algorithms.Distances;
using SigStat.Common.Framework.Samplers;
using SigStat.Common.Loaders;
using SigStat.Common.Logging;
using SigStat.Common.Model;
using SigStat.Common.Pipeline;
using SigStat.Common.PipelineItems.Classifiers;
using SigStat.Common.PipelineItems.Transforms.Preprocessing;
using System.ComponentModel;
using System.Drawing;
namespace onlab
{


    class Program
    {
        static List<Result> results = new List<Result>();
        
        static TrainFunctions trainFunctions = new TrainFunctions();
        static TestFunctions testFunctions = new TestFunctions();

        static void Main(string[] args)
        {


            //LoadSignaturesExample();
            //UseBenchmarkExample();

            //UseBenchMark(trainFunctions.funcs[0], testFunctions.funcs[0]);
            //TestAllMethod();
            //PrintToExcel();
            DecideFunctions decideFunctions = new DecideFunctions();
            List<DecideResult> decideResults = new List<DecideResult>();
            foreach (var decide in decideFunctions.DecideFunctionList)
            {
                decideResults.Add(UseMultipleClassifier(decide));
            }
            PrintDecideToExcel(decideResults);




        }
        private static void PrintDecideToExcel(List<DecideResult> results)
        {
            results = results.OrderBy(r => r.DecideName).ToList();
            ExcelPackage.License.SetNonCommercialPersonal("onlab");
            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Összesítés");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            workSheet.Cells[1, 1].Value = "Decide Name";
            workSheet.Cells[1, 2].Value = "AER";
            workSheet.Cells[1, 3].Value = "FAR";
            workSheet.Cells[1, 4].Value = "FRR";
            int row = 2;
            foreach (var result in results)
            {
                for (int i = 1; i <= 4; i++)
                {
                    workSheet.Cells[row, i].Style.Numberformat.Format = "0.00%";
                }
                workSheet.Cells[row, 1].Value = result.DecideName;
                workSheet.Cells[row, 2].Value = result.AER;
                workSheet.Cells[row, 3].Value = result.FAR;
                workSheet.Cells[row, 4].Value = result.FRR;
                row++;

            }

            string path = @"C:\Users\David\Downloads\bme_decide.xlsx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream objFileStrm = File.Create(path);
            objFileStrm.Close();


            File.WriteAllBytes(path, excel.GetAsByteArray());

            excel.Dispose();
        }
        private static DecideResult UseMultipleClassifier(DecideFunctionDescriptor decide)
        {
            var path = @"C:\Users\David\Downloads\MCYT100.zip";
            // Console.WriteLine("Add meg az adatbázis helyét! (pl. C:/Work/Temalabor/MCYT100.zip");
            //var path = Console.ReadLine();

            var benchmark = new VerifierBenchmark()
            {
                Loader = new MCYTLoader(path, true),
                Logger = new SimpleConsoleLogger(),


                Verifier = new Verifier()
                {
                    Pipeline = new SequentialTransformPipeline
                    {
                        new ZNormalization() { InputFeature = Features.X, OutputFeature = Features.X },
                        new ZNormalization() { InputFeature = Features.Y, OutputFeature = Features.Y },
                        new ZNormalization() { InputFeature = Features.Pressure, OutputFeature = Features.Pressure },
                    },
                    Classifier = new MultipleDTWClassifier()
                    {


                        Features = new List<FeatureDescriptor>() { Features.X, Features.Y, Features.Pressure },
                        //DistanceFunction = new EuclideanDistance().Calculate,
                        DistanceFunction = new ManhattanDistance().Calculate,
                        DecideFunction = decide,
                        TestFunctions = testFunctions.TestFunctionList,
                        ThresholdFunction = new TrainFunctionDescriptor { Name = TrainFunctionName.Medián, Method = trainFunctions.calculateThresholdMedian }




                    },
                    Logger = new SimpleConsoleLogger()

                },
                Sampler = new OddNSampler(10)
            };

            BenchmarkResults result = benchmark.Execute(true);

            DecideResult res = new DecideResult { AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr, DecideName = decide.Name };

            //Console.WriteLine("TEST METHOD: " + testName + "\t TRAIN METHOD: " + trainName);
            Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
            Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
            Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");
            return res;
        }

        private static void PrintToExcel()
        {
            ExcelPackage.License.SetNonCommercialPersonal("onlab");


            var excel = new ExcelPackage();
            var workSheet = excel.Workbook.Worksheets.Add("Összesítés");


            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;

            workSheet.Cells[1, 1].Value = "Train Name";
            workSheet.Cells[1, 2].Value = "Test Name";
            workSheet.Cells[1, 3].Value = "AER";
            workSheet.Cells[1, 4].Value = "FAR";
            workSheet.Cells[1, 5].Value = "FRR";


            results = results.OrderBy(o => o.TrainName).ThenBy(o => o.TestName).ToList();
            int row = 2;

            foreach (var result in results)
            {
                for (int i = 1; i <= 5; i++)
                {
                    workSheet.Cells[row, i].Style.Numberformat.Format = "0.00%";
                }
                workSheet.Cells[row, 1].Value = result.TrainName;
                workSheet.Cells[row, 2].Value = result.TestName;
                workSheet.Cells[row, 3].Value = result.AER;
                workSheet.Cells[row, 4].Value = result.FAR;
                workSheet.Cells[row, 5].Value = result.FRR;
                row++;

            }

            workSheet = excel.Workbook.Worksheets.Add("AER");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            string prev = "";
            row = 1;
            int col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.AER;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }

            workSheet = excel.Workbook.Worksheets.Add("FAR");
            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            prev = "";
            row = 1;
            col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.FAR;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }


            workSheet = excel.Workbook.Worksheets.Add("FRR");

            workSheet.Row(1).Height = 20;
            workSheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            workSheet.Row(1).Style.Font.Bold = true;
            prev = "";
            row = 1;
            col = 2;
            foreach (var result in results)
            {
                if (!result.TrainName.Equals(prev))
                {
                    row++;
                    col = 2;
                    workSheet.Cells[row, 1].Value = result.TrainName;
                    prev = result.TrainName;
                }

                workSheet.Cells[1, col].Value = result.TestName;
                workSheet.Cells[row, col].Value = result.FRR;
                workSheet.Cells[row, col].Style.Numberformat.Format = "0.00%";
                col++;
            }




            string path = @"C:\Users\David\Downloads\bme.xlsx";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            FileStream objFileStrm = File.Create(path);
            objFileStrm.Close();


            File.WriteAllBytes(path, excel.GetAsByteArray());

            excel.Dispose();
        }



        private static void TestAllMethod()
        {
            TrainFunctions trainFunctions = new TrainFunctions();
            TestFunctions testFunctions = new TestFunctions();
            foreach (var train in trainFunctions.TrainFunctionList)
            {
                foreach (var test in testFunctions.TestFunctionList)
                {
                    UseBenchMark(train, test);
                }
            }
            foreach (var result in results)
            {
                result.Print();
            }
            results = results.OrderBy(o => o.AER).ToList();
            Console.WriteLine("Legjobb AER: ");
            results[0].Print();
            Console.WriteLine("Legjobb FAR aztán FRR: ");
            results = results.OrderBy(o => o.FAR).ThenBy(o => o.FRR).ToList();
            results[0].Print();
            Console.WriteLine("Legjobb FRR aztán FAR: ");
            results = results.OrderBy(o => o.FRR).ThenBy(o => o.FAR).ToList();
            results[0].Print();
        }


        private static void UseBenchMark(TrainFunctionDescriptor train, TestFunctionDescriptor test)
        {
            Func<IEnumerable<double>, double> trainFunction = train.Method;
            string trainName = train.Name.ToString();
            Func<List<double>, double, double> testFunction = test.Method;
            string testName = test.Name.ToString();
            var path = @"C:\Users\David\Downloads\MCYT100.zip";
            // Console.WriteLine("Add meg az adatbázis helyét! (pl. C:/Work/Temalabor/MCYT100.zip");
            //var path = Console.ReadLine();

            var benchmark = new VerifierBenchmark()
            {
                Loader = new MCYTLoader(path, true),
                Logger = new SimpleConsoleLogger(),


                Verifier = new Verifier()
                {
                    Pipeline = new SequentialTransformPipeline
                    {
                        new ZNormalization() { InputFeature = Features.X, OutputFeature = Features.X },
                        new ZNormalization() { InputFeature = Features.Y, OutputFeature = Features.Y },
                        new ZNormalization() { InputFeature = Features.Pressure, OutputFeature = Features.Pressure },
                    },
                    Classifier = new MyDTWClassifier()
                    {


                        Features = new List<FeatureDescriptor>() { Features.X, Features.Y, Features.Pressure },
                        //DistanceFunction = new EuclideanDistance().Calculate,
                        DistanceFunction = new ManhattanDistance().Calculate,
                        TestFunction = testFunction,
                        ThresholdFunction = trainFunction




                    },
                    Logger = new SimpleConsoleLogger()

                },
                Sampler = new OddNSampler(10)
            };

            BenchmarkResults result = benchmark.Execute(true);
            Result res = new Result { AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr, TrainName = trainName, TestName = testName };
            results.Add(res);
            Console.WriteLine("TEST METHOD: " + testName + "\t TRAIN METHOD: " + trainName);
            Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
            Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
            Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");


        }

        private static void UseBenchmarkExample()
        {
            // Console.WriteLine("Add meg az adatbázis helyét! (pl. C:/Work/Temalabor/SVC2004.zip");
            // var path = Console.ReadLine();
            var path = @"C:\Users\David\Downloads\MCYT100.zip";


            var benchmark = new VerifierBenchmark()
            {
                Loader = new MCYTLoader(path, true),
                Logger = new SimpleConsoleLogger(),


                Verifier = new Verifier()
                {
                    Classifier = new DtwClassifier()
                    {
                        Features = new List<FeatureDescriptor>() { Features.X, Features.Y, Features.T },
                        //DistanceFunction = new EuclideanDistance().Calculate,
                        Logger = new SimpleConsoleLogger()



                    },
                    Logger = new SimpleConsoleLogger()

                },
                Sampler = new FirstNSampler()
            };
            Console.WriteLine("aab");
            BenchmarkResults result = benchmark.Execute(true);

            Console.WriteLine("bb");
            Console.WriteLine($"AER: {result.FinalResult.Aer}");
            Console.WriteLine($"FAR: {result.FinalResult.Far}");
            Console.WriteLine($"FRR: {result.FinalResult.Frr}");
            Console.ReadKey();
        }

        private static void LoadSignaturesExample()
        {
            // Console.WriteLine("Add meg az adatbázis helyét! (pl. C:/Work/Temalabor/SVC2004.zip");
            //var path = Console.ReadLine();
            var path = @"C:\Users\David\Downloads\MCYT100.zip";
            MCYTLoader loader = new MCYTLoader(path, true);
            var signers = new List<Signer>(loader.EnumerateSigners());

            var signaturesOfUser1 = signers[0].Signatures;
            for (int i = 0; i < signers.Count; i++)
            {
                Console.WriteLine(signers[i].Signatures.Count);
            }
            var signature = signaturesOfUser1[0];

            Console.WriteLine($"A(z) {signature.Signer.ID}. aláíró {signature.ID}. aláírása:");
            Console.WriteLine("X \t Y \t P \t T");

            var id = signature.ID;
            var x = signature.GetFeature(Features.X);
            var y = signature.GetFeature(Features.Y);
            var t = signature.GetFeature(Features.T);
            var p = signature.GetFeature(Features.Pressure);

            for (int i = 0; i < x.Count; i++)
            {
                Console.WriteLine($"{x[i]} \t {y[i]} \t {p[i]} \t {t[i]}");
            }
            Console.ReadKey();
        }
    }
}
