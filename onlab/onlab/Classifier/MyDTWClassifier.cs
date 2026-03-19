
using Accord.Math;
using onlab.SignerModel;
using SigStat.Common;
using SigStat.Common.Algorithms;
using SigStat.Common.Helpers.Serialization;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace onlab.Classifier
{
    internal class MyDTWClassifier : IClassifier
    {

        public required List<FeatureDescriptor> Features { get; set; }
        private static readonly ConcurrentDictionary<(string, string), double> DistanceCache = new();

        private double GetCachedDtw(Signature s1, Signature s2, double[][] f1, double[][] f2)
        {
            
            var key = string.Compare(s1.ID, s2.ID) < 0 ? (s1.ID, s2.ID) : (s2.ID, s1.ID);

            return DistanceCache.GetOrAdd(key, _ =>
                DtwImplementations.ExactDtwWikipedia(f1, f2, DistanceFunction));
        }


        public required Func<IEnumerable<double>, double> ThresholdFunction { get; set; }
        public required Func<List<double>, double, double> TestFunction { get; set; }

        public required Func<double[], double[], double> DistanceFunction { get; set; }
        double IClassifier.Test(ISignerModel model, Signature signature)
        {
            MyDTWSignerModel m = (MyDTWSignerModel)model;
            List<double> values = new List<double>();
           
            double[][] signFeature = signature.GetAggregateFeature(Features).ToArray();
            for(int i =0; i< m.GenuineSignatures.Count; i++)
            {
               // double dist = DtwImplementations.ExactDtwWikipedia(genFeature, signFeature, DistanceFunction);
               double dist = GetCachedDtw(m.GenuineSignatures[i], signature, m.GenuineFeatures[i], signFeature);
                values.Add(dist);
            }
            double probability = TestFunction(values, m.Threshold);
            return probability;
            //Console.WriteLine("ATLAG: " + values.Average() + " TRHESHOLD: "+ m.Threshold +"\t"+ (values.Average()<m.Threshold));



        }

        ISignerModel IClassifier.Train(List<Signature> signatures)
        {
            List<Signature> validSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine);
            List<double[][]> extractedFeatures = validSignatures.Select(s => s.GetAggregateFeature(Features).ToArray()).ToList();

            // DistanceMatrix<string, string, double> distanceMatrix = new DistanceMatrix<string, string, double>();
            List<double> distances = new List<double>();
            
            for (int i = 0; i < extractedFeatures.Count; i++)
            {
                for (int j = i + 1; j < extractedFeatures.Count; j++)
                {
                    //double dist = DtwImplementations.ExactDtwWikipedia(extractedFeatures[i],extractedFeatures[j],DistanceFunction);
                    double dist = GetCachedDtw(validSignatures[i], validSignatures[j], extractedFeatures[i], extractedFeatures[j]);
                    distances.Add(dist);
                }

            }
            double tr = ThresholdFunction(distances);

            MyDTWSignerModel model = new MyDTWSignerModel
            {
                SignerID = signatures[0].Signer.ID,
                GenuineSignatures = validSignatures,
                GenuineFeatures = extractedFeatures,
                Threshold = tr



            };
            return model;

        }
    }
}
