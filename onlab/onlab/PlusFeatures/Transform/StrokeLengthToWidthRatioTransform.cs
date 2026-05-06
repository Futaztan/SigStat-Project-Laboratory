using onlab.PlusFeatures.Feature;
using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class StrokeLengthToWidthRatioTransform : PipelineBase, ITransformation
    {
        required public FeatureDescriptor<List<double>> X { get; set; } = Features.X;
        required public FeatureDescriptor<List<double>> Y { get; set; } = Features.Y;
        public int WindowSize { get; set; } = 5;
        required public FeatureDescriptor<List<double>> Output { get; set; } = MyFeatures.StrokeLengthToWidthRatio;

        public void Transform(Signature signature)
        {
            var x = signature.GetFeature(X);
            var y = signature.GetFeature(Y);


            List<double> results = new List<double>();

            for (int i = 0; i < WindowSize - 1; i++)
            {
                results.Add(0);
            }


            for (int n = WindowSize-1; n < x.Count; n++)
            {
             
                int start = n - (WindowSize - 1) ;
                int end = n;

                double sumDist = 0;
                for (int k = start+1; k <= end; k++)
                {
                    if (k > 0) 
                    {
                        double dx = x[k] - x[k - 1];
                        double dy = y[k] - y[k - 1];
                        sumDist += Math.Sqrt(dx * dx + dy * dy);
                    }
                }

                double minX = x[start];
                double maxX = x[start];
                for (int k = start; k <= end; k++)
                {
                    if (x[k] < minX) minX = x[k];
                    if (x[k] > maxX) maxX = x[k];
                }

                double width = maxX - minX;


                if (width == 0)
                {
                    results.Add(0); 
                }
                else
                {
                    results.Add(sumDist / width);
                }
            }

            signature.SetFeature(Output, results);
        }
    }
}
