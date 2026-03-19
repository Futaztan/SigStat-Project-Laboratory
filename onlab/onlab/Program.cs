using onlab.Classifier;
using onlab.Functions;
using SigStat.Common;
using SigStat.Common.Algorithms.Distances;
using SigStat.Common.Framework.Samplers;
using SigStat.Common.Loaders;
using SigStat.Common.Logging;
using SigStat.Common.Model;
using SigStat.Common.PipelineItems.Classifiers;

namespace onlab
{

  
    class Program
    {
        static List<Result> results = new List<Result>();

        static void Main(string[] args)
        {
            //LoadSignaturesExample();
            //UseBenchmarkExample();
            TrainFunctions trainFunctions = new TrainFunctions();
            TestFunctions testFunctions = new TestFunctions();
            foreach (var train in trainFunctions.funcs)
            {
                foreach (var test in testFunctions.funcs)
                {
                    UseBenchMark(train.Method, train.Name, test.Method, test.Name);
                }
            }
            foreach (var result in results)
            {
                result.Print();
            }


        }
       
       
        private static void UseBenchMark(Func<IEnumerable<double>, double> trainFunction, string trainName, Func<List<double>, double, double> testFunction, string testName)
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
                    Classifier = new MyDTWClassifier()
                    {
                        Features = new List<FeatureDescriptor>() { Features.X, Features.Y, Features.T },
                        DistanceFunction = new EuclideanDistance().Calculate,
                        TestFunction = testFunction,
                        ThresholdFunction = trainFunction
                       



                    },
                    Logger = new SimpleConsoleLogger()

                },
                Sampler = new FirstNSampler()
            };
            
            BenchmarkResults result = benchmark.Execute(true);
            Result res = new Result { AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr, TrainName = trainName, TestName = testName };
            results.Add(res);
            //Console.WriteLine("TEST METHOD: " + testName + "\t TRAIN METHOD: " + trainName);
            //Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
            //Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
            //Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");
            
          
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
