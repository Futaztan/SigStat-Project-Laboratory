using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class LogCurvatureRadiusTransform : PipelineBase, ITransformation
    {
        required public FeatureDescriptor<List<double>> X { get; set; } = Features.X;
        required public FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;
        required public FeatureDescriptor<List<double>> OutputLogCurvature { get; set; } = MyFeatures.LogCurvatureRadius;

        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);

            if (x == null || y == null || x.Count == 0)
                return;

            List<double> results = new List<double>();

  
            results.Add(0);
            for (int i = 1; i < x.Count - 1; i++)
            {
                double dx = (x[i + 1] - x[i - 1]) / 2.0;
                double dy = (y[i + 1] - y[i - 1]) / 2.0;

   
                double ddx = x[i + 1] - 2 * x[i] + x[i - 1];
                double ddy = y[i + 1] - 2 * y[i] + y[i - 1];

                double numerator = Math.Abs(dx * ddy - dy * ddx);
                double denominator = Math.Pow(dx * dx + dy * dy, 1.5);

                if (numerator ==0|| denominator == 0)
                {
                    results.Add(10.0); // log(R) magas értéke egyenes vonalnál
                }
                else
                {
                    double kappa = numerator / denominator;
                    double radius = 1.0 / kappa;
                    results.Add(Math.Log(radius + 1.0));
                }
            }

            if (results.Count < x.Count)
            {
                results.Add(results[results.Count - 1]);
            }

            signature.SetFeature(OutputLogCurvature, results);
        }
    }
}
