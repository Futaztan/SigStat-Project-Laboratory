using onlab.PlusFeatures.Feature;
using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Transform
{
    internal class PenDownConverter : PipelineBase, ITransformation
    {
        required public FeatureDescriptor<List<bool>> PenDown { get; set; } = Features.PenDown;
        required public FeatureDescriptor<List<double>> Output { get; set; } = MyFeatures.ConvertedPenDown;

        public void Transform(Signature signature)
        {
            var input = signature.GetFeature(PenDown);
            List<double> output = input.Select(b => b ? 1.0 : 0.0).ToList();

            signature.SetFeature(Output, output);
        }
    }
}
