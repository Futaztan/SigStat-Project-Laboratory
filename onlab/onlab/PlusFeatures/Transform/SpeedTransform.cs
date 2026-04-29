using Accord;
using onlab.PlusFeatures.Feature;
using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class SpeedTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> X { get; set; } = Features.X;

        public required FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;

        public required FeatureDescriptor<List<double>> T { get; set; } = Features.T;

        public required FeatureDescriptor<List<double>> OutputSpeed { get; set; } = MyFeatures.Speed;

        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);
            var t = signature.GetFeature(T);
            List<double> vs = new List<double>();
            vs.Add(0);
            
            for (int i = 1; i < x.Count; i++)
            {
                double v = (Math.Sqrt(Math.Pow((x[i] - x[i - 1]), 2) + Math.Pow(y[i] - y[i - 1], 2))) / (t[i] - t[i - 1]);
                vs.Add(v);
            }
            signature.SetFeature(OutputSpeed, vs);
        }
    }
}
