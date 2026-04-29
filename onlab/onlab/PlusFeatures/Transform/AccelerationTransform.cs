using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class AccelerationTransform : PipelineBase, ITransformation
    {
        public required FeatureDescriptor<List<double>> T { get; set; } = Features.T;
        
        public required FeatureDescriptor<List<double>> V { get; set; } = MyFeatures.Speed;
       

        public required FeatureDescriptor<List<double>> OutPutAcceleration { get; set; } = MyFeatures.Acceleration;

        public void Transform(Signature signature)
        {
            var ts = signature.GetFeature(T);
            var vs = signature.GetFeature(V);
            List<double> accs = new List<double> { 0 };
            for (int i = 1; i < ts.Count; i++)
            {
                double acc = (vs[i] - vs[i - 1] ) / ( ts[i] - ts[i - 1]);
                accs.Add(acc);
            }
            signature.SetFeature(OutPutAcceleration, accs);
        }
    }
}
