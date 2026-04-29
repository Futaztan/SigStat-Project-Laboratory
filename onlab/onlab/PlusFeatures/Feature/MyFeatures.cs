using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Feature
{
    public static class MyFeatures
    {
        public static readonly FeatureDescriptor<List<double>> Speed = FeatureDescriptor.Get<List<double>>("Speed");
        public static readonly FeatureDescriptor<List<double>> DeltaP = FeatureDescriptor.Get<List<double>>("DeltaP");
        public static readonly FeatureDescriptor<List<double>> Acceleration = FeatureDescriptor.Get<List<double>>("Acceleration");
        public static readonly FeatureDescriptor<List<double>> Sin = FeatureDescriptor.Get<List<double>>("Sin");
        public static readonly FeatureDescriptor<List<double>> Cos = FeatureDescriptor.Get<List<double>>("Cos");
    }
}
