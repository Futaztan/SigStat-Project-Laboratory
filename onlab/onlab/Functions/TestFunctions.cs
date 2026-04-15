using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class TestFunctions
    {
        public List<(TestFunctionName Name, Func<List<double>, double, double> Method)> funcs = new List<(TestFunctionName Name, Func<List<double>, double, double> Method)>();

        public TestFunctions()
        {
            funcs.Add((TestFunctionName.K_nearest, testSignatureKNearest));
            funcs.Add((TestFunctionName.K_farest, testSignatureKFarest));
            funcs.Add((TestFunctionName.Probability, testSignatureProbability));
            funcs.Add((TestFunctionName.Average, testSignaturesAVG));
            funcs.Add((TestFunctionName.Min, testSignaturesMIN));
            funcs.Add((TestFunctionName.Voting, testSignatureVoting));
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
