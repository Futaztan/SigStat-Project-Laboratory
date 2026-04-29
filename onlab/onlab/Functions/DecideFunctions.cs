using onlab.Functions.Descriptors;
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
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Strict, Method = ByStrict });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Permissive, Method = ByPermissive });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Average, Method = ByAverage });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Median, Method = ByMedian });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Voting, Method = ByVoting });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.TopK, Method = ByTopK });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.GeometricMean, Method = ByGeometricMean });
            DecideFunctionList.Add(new DecideFunctionDescriptor { Name = DecideFunctionName.Confidence, Method = ByConfidence });

        }

        private double ByAverage(List<FunctionPair> functionPairs)
        {
            return (functionPairs.Average(f => f.Probability));
        }
        private double ByStrict(List<FunctionPair> functionPairs)
        {
            if (functionPairs.Any(f => f.Probability < 0.5))
                return 0;
            return 1;


        }
        private double ByPermissive(List<FunctionPair> functionPairs)
        {
            if (functionPairs.Any(f => f.Probability >= 0.5))
                return 1;
            return 0;

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
            return (double)votes / functionPair.Count();
        }
        private double ByTopK(List<FunctionPair> functionPair)
        {
            return functionPair.OrderByDescending(r => r.Probability).Take(5).Average(r => r.Probability);
        }

        private double ByGeometricMean(List<FunctionPair> functionPairs)
        {
            if (functionPairs.Count == 0) return 0;

            double product = 1.0;
            foreach (var f in functionPairs)
            {

                product *= f.Probability;
            }

            return Math.Pow(product, 1.0 / functionPairs.Count);
        }

        private double ByConfidence(List<FunctionPair> functionPairs)
        {
            double weightedSum = 0;
            double totalWeight = 0;

            foreach (var f in functionPairs)
            {
   
                double confidence = Math.Abs(f.Probability - 0.5);

                weightedSum += f.Probability * confidence;
                totalWeight += confidence;
            }

            // Ha véletlenül mindenki pont 0.5-öt adott, fallback sima átlagra
            return weightedSum / totalWeight;
        }

    }

}

