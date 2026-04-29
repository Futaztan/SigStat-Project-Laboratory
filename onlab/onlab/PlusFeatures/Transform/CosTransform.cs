using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class CosTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> X { get; set; } = Features.X;

        public required FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;


        public required FeatureDescriptor<List<double>> OutputCos { get; set; } = MyFeatures.Cos;
        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);
            List<double> coses = new List<double>();

            for (int i = 1; i < x.Count; i++)
            {
                double dx = x[i] - x[i - 1];
                double dy = y[i] - y[i - 1];
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (distance == 0)
                    coses.Add(0);
                else
                {
                    double cos = dx / distance;
                    coses.Add(cos);
                }

            }
            coses.Insert(0, coses[0]);
            signature.SetFeature(OutputCos, coses);
        }
    }
}
