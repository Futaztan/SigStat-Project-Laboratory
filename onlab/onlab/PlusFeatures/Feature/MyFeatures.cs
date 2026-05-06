using SigStat.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.PlusFeatures.Feature
{
    //https://ietresearch.onlinelibrary.wiley.com/doi/full/10.1049/iet-bmt.2013.0081
    public static class MyFeatures
    {
        public static readonly FeatureDescriptor<List<double>> Speed = FeatureDescriptor.Get<List<double>>("Speed");
        public static readonly FeatureDescriptor<List<double>> SpeedX = FeatureDescriptor.Get<List<double>>("SpeedX");
        public static readonly FeatureDescriptor<List<double>> SpeedY = FeatureDescriptor.Get<List<double>>("SpeedY");
        public static readonly FeatureDescriptor<List<double>> DeltaP = FeatureDescriptor.Get<List<double>>("DeltaP");
        public static readonly FeatureDescriptor<List<double>> Acceleration = FeatureDescriptor.Get<List<double>>("Acceleration");
        public static readonly FeatureDescriptor<List<double>> Sin = FeatureDescriptor.Get<List<double>>("Sin");
        public static readonly FeatureDescriptor<List<double>> Cos = FeatureDescriptor.Get<List<double>>("Cos");
        // vonás hosszának és szélességének aránya az adott ablakban
        public static readonly FeatureDescriptor<List<double>> StrokeLengthToWidthRatio = FeatureDescriptor.Get<List<double>>("StrokeLengthToWidthRatio");
        // A görbületi sugár logaritmusa. Azt méri, mekkora kör írható le az adott kanyarban
        public static readonly FeatureDescriptor<List<double>> LogCurvatureRadius = FeatureDescriptor.Get<List<double>>("LogCurvatureRadius");
        //aláírás mértani közepéhez viszonyít
        public static readonly FeatureDescriptor<List<double>> CentroidDistance = FeatureDescriptor.Get<List<double>>("CentroidDistance");
        public static readonly FeatureDescriptor<List<double>> ConvertedPenDown = FeatureDescriptor.Get<List<double>>("ConvertedPenDown");
    }
}
