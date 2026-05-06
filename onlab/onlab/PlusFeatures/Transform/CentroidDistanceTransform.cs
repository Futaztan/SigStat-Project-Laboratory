using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class CentroidDistanceTransform : PipelineBase, ITransformation
    {
        required public FeatureDescriptor<List<double>> X { get; set; } = Features.X;
        required public FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;
        required public FeatureDescriptor<List<double>> Output { get; set; } = MyFeatures.CentroidDistance;

        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);
          
            double midX = x.Average();
            double midY = y.Average();

            List<double> distances = new List<double>();
            foreach (var (px, py) in x.Zip(y, (a, b) => (a, b)))
            {
                double d = Math.Sqrt(Math.Pow(px - midX, 2) + Math.Pow(py - midY, 2));
                distances.Add(d);
            }

            signature.SetFeature(Output, distances);
        }
    }
}
