using onlab.Functions;
using onlab.SignerModel;
using SigStat.Common;
using SigStat.Common.Algorithms;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace onlab.Classifier
{
    public class MultipleDTWClassifier : IClassifier
    {
        public required List<FeatureDescriptor> Features { get; set; }
        private List<FunctionPair> results = new List<FunctionPair>();
        
        public static readonly ConcurrentDictionary<(string, string), double> DistanceCache = new();

        private double GetCachedDtw(Signature s1, Signature s2, double[][] f1, double[][] f2)
        {

            var key = string.Compare(s1.ID, s2.ID) < 0 ? (s1.ID, s2.ID) : (s2.ID, s1.ID);

            return DistanceCache.GetOrAdd(key, _ =>
                DtwImplementations.ExactDtwWikipedia(f1, f2, DistanceFunction));
        }

        public required List<(TrainFunctionName Name, Func<IEnumerable<double>, double> Method)> ThresholdFunctions { get; set; }
        public required List<(TestFunctionName Name, Func<List<double>, double, double> Method)> TestFunctions { get; set; }
        public required Func<List<FunctionPair>, double> DecideFunction { get; set; }

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
            List<FunctionPair> results = new List<FunctionPair>();
            foreach (var trainResult in m.TrainResults)
            {
                foreach (var testFunction in TestFunctions)
                {
                    double probability = testFunction.Method(values, trainResult.Threshold);
                    results.Add(new FunctionPair
                    {
                        TrainFunction = trainResult.TrainFunction,
                        Threshold = trainResult.Threshold,
                        Probability = probability,
                        TestFunction = testFunction.Name
                    });
                }
                
            }

            return DecideFunction(results);

        }

        ISignerModel IClassifier.Train(List<Signature> signatures)
        {
            List<Signature> validSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine);
            List<double[][]> validFeatures = validSignatures.Select(s => s.GetAggregateFeature(Features).ToArray()).ToList();

            // DistanceMatrix<string, string, double> distanceMatrix = new DistanceMatrix<string, string, double>();
            List<double> distancesBetweenValid = new List<double>();

            for (int i = 0; i < validFeatures.Count; i++)
            {
                for (int j = i + 1; j < validFeatures.Count; j++)
                {
                    double dist = GetCachedDtw(validSignatures[i], validSignatures[j], validFeatures[i], validFeatures[j]);
                    distancesBetweenValid.Add(dist);
                }
            }
            List<FunctionPair> trainResults = new List<FunctionPair>();
            foreach (var ThresholdFunction in ThresholdFunctions)
            {
                double tr = ThresholdFunction.Method(distancesBetweenValid);
                trainResults.Add(new FunctionPair { TrainFunction = ThresholdFunction.Name, Threshold = tr });
            }
           
           

            MultipleDTWSignerModel model = new MultipleDTWSignerModel
            {
                SignerID = signatures[0].Signer.ID,
                GenuineSignatures = validSignatures,
                GenuineFeatures = validFeatures,
                TrainResults = trainResults
            };
            return model;

        }
    }
}
