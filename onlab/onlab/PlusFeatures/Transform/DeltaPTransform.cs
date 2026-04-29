using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class DeltaPTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> P { get; set; } = Features.Pressure;
        public required FeatureDescriptor<List<double>> OutputDeltaP { get; set; } = MyFeatures.DeltaP;

        public void Transform(Signature signature)
        {
            var p = signature.GetFeature(P);
            List<double> deltas = new List<double>() { 0 };
            for (int i = 1; i < p.Count; i++)
            {
                double delta = p[i] - p[i - 1   ];
                deltas.Add(delta);
            }
            signature.SetFeature(OutputDeltaP, deltas);
        }
    }
}
