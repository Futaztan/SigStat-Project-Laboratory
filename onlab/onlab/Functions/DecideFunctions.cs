using onlab.Functions.Enums;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class DecideFunctions
    {
        public List<DecideFunctionDescriptor> DecideFunctionList = new List<DecideFunctionDescriptor>();

        public DecideFunctions()
        {
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Maximum, Method = ByMaximum });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Minimum, Method = ByMinimum });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Average, Method = ByAverage });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Median, Method = ByMedian });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Voting, Method = ByVoting });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.TopK, Method = ByTopK });

        }

        private double ByAverage(List<FunctionPair> functionPairs)
        {
            return (functionPairs.Average(f => f.Probability));
        }
        private double ByMaximum(List<FunctionPair> functionPairs)
        {
            return functionPairs.Max(f => f.Probability);
           
        }
        private double ByMinimum(List<FunctionPair> functionPairs)
        {
            return functionPairs.Min(f => f.Probability);
        }
        private double ByMedian(List<FunctionPair> functionPairs)
        {
            var sorted = functionPairs.Select(r => r.Probability).OrderBy(p => p).ToList();
            int count = sorted.Count;
            if (count % 2 == 0)
                return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
            return sorted[count / 2];
        }
        private double ByVoting(List<FunctionPair> functionPair)
        {
            int votes = functionPair.Count(f => f.Probability > 0.5);
            return (double)(votes / functionPair.Count());
        }
        private double ByTopK(List<FunctionPair> functionPair)
        {
            return functionPair.OrderByDescending(r => r.Probability).Take(3).Average(r => r.Probability);
        }

    }
}

