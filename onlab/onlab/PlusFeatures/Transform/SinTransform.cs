using onlab.PlusFeatures.Feature;
using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class SinTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> X { get; set; } = Features.X;

        public required FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;


        public required FeatureDescriptor<List<double>> OutputSin { get; set; } = MyFeatures.Sin;



        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);
            List<double> sines = new List<double>();

            for (int i = 1; i < x.Count; i++)
            {
                double dx = x[i] - x[i - 1];
                double dy = y[i] - y[i - 1];
                double distance = Math.Sqrt(dx * dx + dy * dy);
                if (distance == 0)
                    sines.Add(0);
                else
                {
                    double sin = dy / distance;
                    sines.Add(sin);
                }

            }
            sines.Insert(0, sines[0]);
            signature.SetFeature(OutputSin, sines);
        }
    }
}
