using onlab.Functions.EnsembleClassifiers;
using onlab.Functions.SimpleClassifiers;
using onlab.Functions.ThresholdFunctions;
using onlab.Managers;
using SigStat.Common;

namespace onlab
{
    class Program
    {
        static void Main(string[] args)
        {
            ExcelManager excelManager = new();
            BenchmarkManager benchmarkManager = new();
            TestEnsembleClassifiersAndFeatures(benchmarkManager, excelManager);
            //TestSimpleClassifiersAndThresholdFunctions(benchmarkManager, excelManager);
        }

        private static void TestEnsembleClassifiersAndFeatures(BenchmarkManager benchmarkManager,ExcelManager excelManager)
        {
            var featureSets = new List<List<FeatureDescriptor>>()
            {
                new()
                {
                    Features.X, Features.T
                }
                
            };
            List<SimpleClassifierBase> simpleClassifiers = new()
            {
                new AverageSimpleClassifier(), new HarmonicMeanSimpleClassifier(), new KNearestSimpleClassifier(),
                new MedianSimpleClassifier(), new MinimumSimpleClassifier(),
                new ProbabilitySimpleClassifier(), new VotingSimpleClassifier()
            };

            List<IEnsembleClassifier> ensembleClassifiers = new()
            {
                new AverageEnsembleClassifier(), 
                new GeometricMeanEnsembleClassifier(), new MedianEnsembleClassifier(),
                new StrictEnsembleClassifier(), 
                new VotingEnsembleClassifier()
            };

            List<Result> results = new();
            foreach (var ensembleClassifier in ensembleClassifiers)
            {
                //decideResults.Add(benchmarkManager.UseMultipleClassifier(decide));
                foreach (var feature in featureSets)
                {
                    results.Add(benchmarkManager.UseMultipleClassifierWithPlusFeatures(ensembleClassifier, feature,
                        simpleClassifiers, new MedianThresholdFunction()));
                }
            }

            // excelManager.PrintDecideToExcel(decideResults);
            excelManager.PrintToExcelEnsembleClassifiersAndFeatures(results);
        }

        private static void TestSimpleClassifiersAndThresholdFunctions(BenchmarkManager benchmarkManager,
            ExcelManager excelManager)
        {
            List<Result> results = new List<Result>();

            List<IThresholdFunction> thresholdFunctions = new()
            {
                new AverageThresholdFunction(), new DeviationThresholdFunction(), new MaximumThresholdFunction(),
                new MedianThresholdFunction(), new PercentileThresholdFunction()
            };

            List<SimpleClassifierBase> classifiers = new()
            {
                new AverageSimpleClassifier(), new HarmonicMeanSimpleClassifier(), new KNearestSimpleClassifier(),
                new MedianSimpleClassifier(), new MinimumSimpleClassifier(),
                new ProbabilitySimpleClassifier(), new VotingSimpleClassifier()
            };

            foreach (var thresholdFunction in thresholdFunctions)
            {
                foreach (var classifier in classifiers)
                {
                    results.Add(benchmarkManager.TrainAndTestFunctions(thresholdFunction, classifier));
                }
            }

            foreach (var result in results)
            {
                result.Print();
            }

            excelManager.PrintToExcelSimpleClassifierAndThresholdFunction(results);
        }
    }
}