using onlab.Functions;
using onlab.PlusFeatures.Feature;
using SigStat.Common;
using SigStat.Common.Loaders;

namespace onlab
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelManager excelManager = new();
            BenchmarkManager benchmarkManager = new();
            //LoadSignaturesExample();
            //UseBenchmarkExample();
            //UseBenchMark(trainFunctions.funcs[0], testFunctions.funcs[0]);
            //TestAllMethod();
            //PrintToExcel();

            var featureSets = new List<List<FeatureDescriptor>>()
            {
                // new() { Features.X, MyFeatures.ConvertedPenDown},
                // new() { Features.Y, MyFeatures.ConvertedPenDown},
                // new() { Features.Pressure, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.Speed, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.SpeedX, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.SpeedY, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.DeltaP, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.Acceleration, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.Sin, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.Cos, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.StrokeLengthToWidthRatio, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.LogCurvatureRadius, MyFeatures.ConvertedPenDown},
                // new() { MyFeatures.CentroidDistance, MyFeatures.ConvertedPenDown}
                //new() { MyFeatures.ConvertedPenDown},
                //new() {Features.X, Features.Y, Features.Pressure, MyFeatures.Speed, MyFeatures.SpeedX, MyFeatures.SpeedY,
                //MyFeatures.DeltaP,MyFeatures.Acceleration,MyFeatures.Sin,MyFeatures.Cos,MyFeatures.StrokeLengthToWidthRatio,
                //MyFeatures.LogCurvatureRadius,MyFeatures.CentroidDistance},
                new()
                {
                    Features.X, Features.Y, MyFeatures.Speed, MyFeatures.SpeedX, MyFeatures.SpeedY, Features.Pressure,
                   MyFeatures.Sin, MyFeatures.Cos, MyFeatures.CentroidDistance,
                    MyFeatures.ConvertedPenDown
                },

            };


            DecideFunctions decideFunctions = new DecideFunctions();
            List<DecideResult> decideResults = new List<DecideResult>();
            foreach (var decide in decideFunctions.DecideFunctionList)
            {
                //decideResults.Add(UseMultipleClassifier(decide));
                foreach (var feature in featureSets)
                {
                    decideResults.Add(benchmarkManager.UseMultipleClassifierWithPlusFeatures(decide, feature));
                }
            }

            //PrintDecideToExcel(decideResults);
            excelManager.PrintToExcelDecideAndFeature(decideResults);
        }

        private static void TestAllMethod(BenchmarkManager benchmarkManager)
        {
            List<Result> results = new List<Result>();
            TrainFunctions trainFunctions = new TrainFunctions();
            TestFunctions testFunctions = new TestFunctions();
            foreach (var train in trainFunctions.TrainFunctionList)
            {
                foreach (var test in testFunctions.TestFunctionList)
                {
                    results = benchmarkManager.TrainAndTestFunctions(train, test);
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