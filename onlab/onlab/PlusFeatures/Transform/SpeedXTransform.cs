using onlab.PlusFeatures.Feature;
using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class SpeedXTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> X { get; set; } = Features.X;

        public required FeatureDescriptor<List<double>> T { get; set; } = Features.T;

        public required FeatureDescriptor<List<double>> Output { get; set; } = MyFeatures.SpeedX;


        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var t = signature.GetFeature(T);
            List<double> vs = new List<double>();
            vs.Add(0);
            for (int i = 1; i < x.Count; i++)
            {
                double dt = t[i] - t[i - 1];

                if (dt != 0)
                    vs.Add((x[i] - x[i - 1]) / dt);
                else vs.Add(0);
            }
            signature.SetFeature(Output, vs);
        }
    }
}
