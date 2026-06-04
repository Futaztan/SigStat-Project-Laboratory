using System.Collections.Concurrent;
using onlab.Functions.EnsembleClassifiers;
using onlab.Functions.SimpleClassifiers;
using onlab.Functions.ThresholdFunctions;
using onlab.SignerModel;
using SigStat.Common;
using SigStat.Common.Algorithms;
using SigStat.Common.Pipeline;

namespace onlab.Classifiers
{
    public class MultipleDTWClassifier : IClassifier
    {
        public required List<FeatureDescriptor> Features { get; set; }


        public static readonly ConcurrentDictionary<(string, string, string), double> DistanceCache = new();

        private double GetCachedDtw(Signature s1, Signature s2, double[][] f1, double[][] f2)
        {
            var featuresKey = string.Join("|", Features.Select(f => f.Name).OrderBy(n => n));
            var idKey = string.Compare(s1.ID, s2.ID) < 0 ? (s1.ID, s2.ID) : (s2.ID, s1.ID);
            var key = (idKey.Item1, idKey.Item2, featuresKey);

            return DistanceCache.GetOrAdd(key, _ =>
                DtwImplementations.ExactDtwWikipedia(f1, f2, DistanceFunction));
        }

        public required IThresholdFunction ThresholdFunction { get; set; }
        public required List<SimpleClassifierBase> SimpleClassifiers { get; set; }
        public required IEnsembleClassifier EnsembleClassifier { get; set; }

        public required Func<double[], double[], double> DistanceFunction { get; set; }

        double IClassifier.Test(ISignerModel model, Signature signature)
        {
            MultipleDTWSignerModel m = (MultipleDTWSignerModel)model;
            List<double> values = new List<double>();

            double[][] signFeature = signature.GetAggregateFeature(Features).ToArray();


            for (int i = 0; i < m.GenuineSignatures.Count; i++)
            {
                // double dist = DtwImplementations.ExactDtwWikipedia(genFeature, signFeature, DistanceFunction);
                double dist = GetCachedDtw(m.GenuineSignatures[i], signature, m.GenuineFeatures[i], signFeature);
                values.Add(dist);
            }

            List<Result> results = new ();

            foreach (var classifier in SimpleClassifiers)
            {
                double probability = classifier.Decide(values, m.Threshold);
                results.Add(new Result()
                {
                    SimpleClassifierName = classifier.Name,
                    Threshold = m.Threshold,
                    SimpleProbability = probability,
                    ThresholdFunctionName = ThresholdFunction.Name
                });
            }


            return EnsembleClassifier.Decide(results);
        }

        ISignerModel IClassifier.Train(List<Signature> signatures)
        {
            List<Signature> validSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine);
            List<double[][]> validFeatures =
                validSignatures.Select(s => s.GetAggregateFeature(Features).ToArray()).ToList();

            // DistanceMatrix<string, string, double> distanceMatrix = new DistanceMatrix<string, string, double>();
            List<double> distancesBetweenValid = new List<double>();

            for (int i = 0; i < validFeatures.Count; i++)
            {
                for (int j = i + 1; j < validFeatures.Count; j++)
                {
                    double dist = GetCachedDtw(validSignatures[i], validSignatures[j], validFeatures[i],
                        validFeatures[j]);
                    distancesBetweenValid.Add(dist);
                }
            }

            // List<FunctionPair> trainResults = new List<FunctionPair>();
            /* foreach (var ThresholdFunction in ThresholdFunctions)
             {
                 double tr = ThresholdFunction.Method(distancesBetweenValid);
                 trainResults.Add(new FunctionPair { TrainFunction = ThresholdFunction.Name, Threshold = tr });
             }*/
            double tr = ThresholdFunction.CalculateThreshold(distancesBetweenValid);
            
            MultipleDTWSignerModel model = new MultipleDTWSignerModel
            {
                SignerID = signatures[0].Signer.ID,
                GenuineSignatures = validSignatures,
                GenuineFeatures = validFeatures,
                Threshold = tr
            };
            return model;
        }
    }
}