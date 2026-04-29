using onlab.Functions.Descriptors;
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
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Median, Method = ByMedian });
            TestFunctionList.Add(new TestFunctionDescriptor { Name = TestFunctionName.Harmonic_Mean, Method = ByHarmonicMean });
        }

        private double ToSigmoid(double dist, double threshold)
        {

            double steepness = 2; //50 /threshold;
            return 1.0 / (1.0 + Math.Exp((dist - threshold) *  steepness));
        }
      

        private double ByAverage(List<double> values, double threshold)
        {
            //if (values.Average() < threshold) return 1;
            //return 0;
            return ToSigmoid(values.Average(), threshold);
        }

        private double ByMin(List<double> values, double threshold)
        {
            //if (values.Min() < threshold) return 1;
            //return 0;
            return ToSigmoid(values.Min(), threshold);
        }
    
        private double ByKnearest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.Order().Take(k);
            //return kBestValues.Average() < threshold ? 1.0 : 0.0;
            return ToSigmoid(kBestValues.Average(), threshold);
        }
        private double ByKfarest(List<double> values, double threshold)
        {
            int k = 10;
            
            var kBestValues = values.OrderDescending().Take(k);
            //return kBestValues.Average() < threshold ? 1.0 : 0.0;
            return ToSigmoid(kBestValues.Average(), threshold);
        }
        private double ByProbability(List<double> values, double threshold)
        {
            double dist = values.Average();
                       
            double score = threshold / (threshold + dist);
            return score;
        }
     
        private double ByVoting(List<double> values, double threshold)
        {

            //int votes = values.Count(v => v < threshold);

            //return (votes > values.Count / 2) ? 1.0 : 0.0;
            List<double> list = new();
            values.ForEach(v => list.Add(ToSigmoid(v, threshold)));
            return list.Average();
        }
        private double ByMedian(List<double> values, double threshold)
        {
            if (values.Count == 0) return 0.0;

            var sortedValues = values.OrderBy(v => v).ToList();
            int midIndex = sortedValues.Count / 2;

            double median;
            if (sortedValues.Count % 2 != 0)
            {
                median = sortedValues[midIndex];
            }
            else
            {
                median = (sortedValues[midIndex - 1] + sortedValues[midIndex]) / 2.0;
            }

            return ToSigmoid(median, threshold);
        }
        private double ByHarmonicMean(List<double> values, double threshold)
        {
            // Védelem a nullával való osztás ellen (bár távolság ritkán pont 0)
            if (values.Any(v => v <= 0.0001)) return ToSigmoid(0, threshold);

            double sumOfInverses = values.Sum(v => 1.0 / v);
            double harmonicMean = values.Count / sumOfInverses;

            return ToSigmoid(harmonicMean, threshold);
        }
    }
}
