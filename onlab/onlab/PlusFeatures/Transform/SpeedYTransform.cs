using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class SpeedYTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;

        public required FeatureDescriptor<List<double>> T { get; set; } = Features.T;

        public required FeatureDescriptor<List<double>> Output { get; set; } = MyFeatures.SpeedY;


        public void Transform(Signature signature)
        {
            var y = signature.GetFeature(Y);
            var t = signature.GetFeature(T);
            List<double> vs = new List<double>();
            vs.Add(0);
            for (int i = 1; i < y.Count; i++)
            {
                double dt = t[i] - t[i - 1];

                if (dt != 0)
                    vs.Add((y[i] - y[i - 1]) / dt);
                else vs.Add(0);
            }
            signature.SetFeature(Output, vs);
        }
    }
}
