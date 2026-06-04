using onlab.Classifiers;
using onlab.Functions.EnsembleClassifiers;
using onlab.Functions.SimpleClassifiers;
using onlab.Functions.ThresholdFunctions;
using onlab.PlusFeatures.Feature;
using onlab.PlusFeatures.Transform;
using SigStat.Common;
using SigStat.Common.Algorithms.Distances;
using SigStat.Common.Framework.Samplers;
using SigStat.Common.Loaders;
using SigStat.Common.Logging;
using SigStat.Common.Model;
using SigStat.Common.Pipeline;
using SigStat.Common.PipelineItems.Transforms.Preprocessing;

namespace onlab.Managers;

public class BenchmarkManager
{
    private string path;

    public BenchmarkManager()
    {
        
        Console.WriteLine("Add meg az adatbázis helyét! (pl. C:/Work/Temalabor/MCYT100.zip");
        path = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(path) || !File.Exists(path))
        {
            Console.WriteLine("Nem található ilyen file");
            path = Console.ReadLine();
        }
    }

    public Result UseMultipleClassifierWithPlusFeatures(IEnsembleClassifier ensembleClassifier,
        List<FeatureDescriptor> features, List<SimpleClassifierBase> classifiers, IThresholdFunction thresholdFunction)
    {
        var benchmark = new VerifierBenchmark()
        {
            Loader = new MCYTLoader(path, true),
            Logger = new SimpleConsoleLogger(),


            Verifier = new Verifier()
            {
                Pipeline = new SequentialTransformPipeline
                {
                    new SpeedTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        T = Features.T,
                        OutputSpeed = MyFeatures.Speed
                    },
                    new DeltaPTransform()
                    {
                        OutputDeltaP = MyFeatures.DeltaP,
                        P = Features.Pressure
                    },
                    new AccelerationTransform()
                    {
                        T = Features.T,
                        V = MyFeatures.Speed,
                        OutPutAcceleration = MyFeatures.Acceleration
                    },
                    new CosTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        OutputCos = MyFeatures.Cos
                    },
                    new SinTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        OutputSin = MyFeatures.Sin
                    },
                    new StrokeLengthToWidthRatioTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        Output = MyFeatures.StrokeLengthToWidthRatio
                    },
                    new LogCurvatureRadiusTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        OutputLogCurvature = MyFeatures.LogCurvatureRadius
                    },
                    new PenDownConverter()
                    {
                        PenDown = Features.PenDown,
                        Output = MyFeatures.ConvertedPenDown
                    },
                    new SpeedXTransform()
                    {
                        X = Features.X,
                        T = Features.T,
                        Output = MyFeatures.SpeedX
                    },
                    new SpeedYTransform()
                    {
                        Y = Features.Y,
                        T = Features.T,
                        Output = MyFeatures.SpeedY
                    },
                    new CentroidDistanceTransform()
                    {
                        X = Features.X,
                        Y = Features.Y,
                        Output = MyFeatures.CentroidDistance
                    },

                    new ZNormalization() { InputFeature = Features.X, OutputFeature = Features.X },
                    new ZNormalization() { InputFeature = Features.Y, OutputFeature = Features.Y },
                    new ZNormalization() { InputFeature = Features.Pressure, OutputFeature = Features.Pressure },
                    new ZNormalization() { InputFeature = MyFeatures.Speed, OutputFeature = MyFeatures.Speed },
                    new ZNormalization() { InputFeature = MyFeatures.DeltaP, OutputFeature = MyFeatures.DeltaP },
                    new ZNormalization()
                        { InputFeature = MyFeatures.Acceleration, OutputFeature = MyFeatures.Acceleration },
                    new ZNormalization() { InputFeature = MyFeatures.Sin, OutputFeature = MyFeatures.Sin },
                    new ZNormalization() { InputFeature = MyFeatures.Cos, OutputFeature = MyFeatures.Cos },
                    new ZNormalization()
                    {
                        InputFeature = MyFeatures.StrokeLengthToWidthRatio,
                        OutputFeature = MyFeatures.StrokeLengthToWidthRatio
                    },
                    new ZNormalization()
                        { InputFeature = MyFeatures.LogCurvatureRadius, OutputFeature = MyFeatures.LogCurvatureRadius },
                    new ZNormalization()
                        { InputFeature = MyFeatures.ConvertedPenDown, OutputFeature = MyFeatures.ConvertedPenDown },
                    new ZNormalization() { InputFeature = MyFeatures.SpeedX, OutputFeature = MyFeatures.SpeedX },
                    new ZNormalization() { InputFeature = MyFeatures.SpeedY, OutputFeature = MyFeatures.SpeedY },
                    new ZNormalization()
                        { InputFeature = MyFeatures.CentroidDistance, OutputFeature = MyFeatures.CentroidDistance },
                    new ZNormalization() { InputFeature = Features.Altitude, OutputFeature = Features.Altitude },
                    new ZNormalization() { InputFeature = Features.Azimuth, OutputFeature = Features.Azimuth },
                },
                Classifier = new MultipleDTWClassifier()
                {
                    Features = features,
                    //DistanceFunction = new EuclideanDistance().Calculate,
                    DistanceFunction = new ManhattanDistance().Calculate,
                    EnsembleClassifier = ensembleClassifier,
                    SimpleClassifiers = classifiers,
                    ThresholdFunction = thresholdFunction
                },
                Logger = new SimpleConsoleLogger()
            },
            Sampler = new OddNSampler(10)
        };

        BenchmarkResults result = benchmark.Execute(true);
        List<string> names = new List<string>();
        foreach (var feature in features)
        {
            names.Add(feature.Name);
        }

        Result res = new()

        {
            AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr,
            EnsembleClassifierName = ensembleClassifier.Name, FeatureNames = names
        };

        //Console.WriteLine("TEST METHOD: " + testName + "\t TRAIN METHOD: " + trainName);
        Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
        Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
        Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");
        return res;
    }


    public Result UseMultipleClassifier(IEnsembleClassifier ensembleClassifier,
        List<SimpleClassifierBase> classifiers, IThresholdFunction thresholdFunction)
    {
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
                    EnsembleClassifier = ensembleClassifier,
                    SimpleClassifiers = classifiers,
                    ThresholdFunction = thresholdFunction
                },
                Logger = new SimpleConsoleLogger()
            },
            Sampler = new OddNSampler(10)
        };

        BenchmarkResults result = benchmark.Execute(true);

        Result res = new Result
        {
            AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr,
            EnsembleClassifierName = ensembleClassifier.Name
        };

        //Console.WriteLine("TEST METHOD: " + testName + "\t TRAIN METHOD: " + trainName);
        Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
        Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
        Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");
        return res;
    }


    public Result TrainAndTestFunctions(IThresholdFunction thresholdFunction, SimpleClassifierBase classifier)
    {
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
                    SimpleClassifier = classifier,
                    ThresholdFunction = thresholdFunction
                },
                Logger = new SimpleConsoleLogger()
            },
            Sampler = new OddNSampler(10)
        };

        BenchmarkResults result = benchmark.Execute(true);
        Result res = new Result
        {
            AER = result.FinalResult.Aer, FAR = result.FinalResult.Far, FRR = result.FinalResult.Frr,
            SimpleClassifierName = thresholdFunction.Name, ThresholdFunctionName = classifier.Name
        };

        Console.WriteLine("TEST METHOD: " + classifier.Name + "\t TRAIN METHOD: " + thresholdFunction.Name);
        Console.WriteLine($"AER (Average Error Rate): {result.FinalResult.Aer}");
        Console.WriteLine($"FAR (False Acceptance Rate): {result.FinalResult.Far}");
        Console.WriteLine($"FRR (False Rejection Rate): {result.FinalResult.Frr}");
        return res;
    }
}