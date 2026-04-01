using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class TestFunctions
    {
        public List<(string Name, Func<List<double>, double, double> Method)> funcs = new List<(string Name, Func<List<double>, double, double> Method)>();

        public TestFunctions()
        {
            funcs.Add(("K nearest",testSignatureKNearest));
            funcs.Add(("K farest", testSignatureKFarest));
            funcs.Add(("Probability",testSignatureProbability));
            funcs.Add(("Average",testSignaturesAVG));
            funcs.Add(("Min",testSignaturesMIN));
            funcs.Add(("Voting",testSignatureVoting));
        }
        
        public double testSignaturesAVG(List<double> values, double threshold)
        {
            if (values.Average() < threshold) return 1;
            return 0;
        }

        public double testSignaturesMIN(List<double> values, double threshold)
        {
            if (values.Min() < threshold) return 1;
            return 0;
        }
    
        public double testSignatureKNearest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.Order().Take(k);
            return kBestValues.Average() < threshold ? 1.0 : 0.0;
        }
        public double testSignatureKFarest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.OrderDescending().Take(k);
            return kBestValues.Average() < threshold ? 1.0 : 0.0;
        }
        public double testSignatureProbability(List<double> values, double threshold)
        {
            double dist = values.Average();
                       
            double score = threshold / (threshold + dist);
            return score;
        }
     
        public double testSignatureVoting(List<double> values, double threshold)
        {

            int votes = values.Count(v => v < threshold);
          
            return (votes > values.Count / 2) ? 1.0 : 0.0;
        }
    }
}
