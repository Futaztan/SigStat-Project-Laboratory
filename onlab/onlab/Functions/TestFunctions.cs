using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class TestFunctions
    {
        public List<TestFunctionDescriptor> TestFunctionList = new List<TestFunctionDescriptor>();

        public TestFunctions()
        {
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.K_nearest,Method = ByKnearest });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.K_farest,Method = ByKfarest });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Probability,Method = ByProbability });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Average, Method = ByAverage });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Min, Method = ByMin });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Voting, Method = ByVoting });
        }
        
        private double ByAverage(List<double> values, double threshold)
        {
            if (values.Average() < threshold) return 1;
            return 0;
        }

        private double ByMin(List<double> values, double threshold)
        {
            if (values.Min() < threshold) return 1;
            return 0;
        }
    
        private double ByKnearest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.Order().Take(k);
            return kBestValues.Average() < threshold ? 1.0 : 0.0;
        }
        private double ByKfarest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.OrderDescending().Take(k);
            return kBestValues.Average() < threshold ? 1.0 : 0.0;
        }
        private double ByProbability(List<double> values, double threshold)
        {
            double dist = values.Average();
                       
            double score = threshold / (threshold + dist);
            return score;
        }
     
        private double ByVoting(List<double> values, double threshold)
        {

            int votes = values.Count(v => v < threshold);
          
            return (votes > values.Count / 2) ? 1.0 : 0.0;
        }
    }
}
