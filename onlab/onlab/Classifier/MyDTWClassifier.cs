
using Accord.Math;
using onlab.SignerModel;
using SigStat.Common;
using SigStat.Common.Algorithms;
using SigStat.Common.Helpers.Serialization;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Classifier
{
    internal class MyDTWClassifier : IClassifier
    {

        public required List<FeatureDescriptor> Features { get; set; }

        


        public required Func<IEnumerable<double>, double> ThresholdFunction { get; set; }
        public required Func<List<double>, double, double> TestFunction { get; set; }

        public required Func<double[], double[], double> DistanceFunction { get; set; }
        double IClassifier.Test(ISignerModel model, Signature signature)
        {
            MyDTWSignerModel m = (MyDTWSignerModel)model;
            List<double> values = new List<double>();
            foreach (Signature s in m.GenuineSignatures)
            {
                double[][] feature1 = s.GetAggregateFeature(Features).ToArray();
                double[][] feature2 = signature.GetAggregateFeature(Features).ToArray();
                double num = DtwImplementations.ExactDtwWikipedia(feature1, feature2, DistanceFunction);
                values.Add(num);
            }
            double probability = TestFunction(values, m.Threshold);
            return probability;
            //Console.WriteLine("ATLAG: " + values.Average() + " TRHESHOLD: "+ m.Threshold +"\t"+ (values.Average()<m.Threshold));



        }

        ISignerModel IClassifier.Train(List<Signature> signatures)
        {
            List<Signature> validSignatures = signatures.FindAll(s => s.Origin == Origin.Genuine);
           
            DistanceMatrix<string, string, double> distanceMatrix = new DistanceMatrix<string, string, double>();
            foreach (Signature s1 in validSignatures)
            {
                foreach (Signature s2 in validSignatures)
                {
                    if (s1 == s2) continue;
                    if (s1.Origin == Origin.Genuine)
                    {
                        double[][] feature1 = s1.GetAggregateFeature(Features).ToArray();
                        double[][] feature2 = s2.GetAggregateFeature(Features).ToArray();
                        double num = DtwImplementations.ExactDtwWikipedia(feature1, feature2, DistanceFunction);
                        distanceMatrix[s1.ID, s2.ID] = num;



                     }
                }
               
            }

            
            IEnumerable<double> values = distanceMatrix.GetValues();

            double tr = ThresholdFunction(values);
           
           



            MyDTWSignerModel model = new MyDTWSignerModel { 
                SignerID = signatures[0].Signer.ID,
                GenuineSignatures = validSignatures,
                Threshold = tr

            };
            return model;

        }
    }
}
