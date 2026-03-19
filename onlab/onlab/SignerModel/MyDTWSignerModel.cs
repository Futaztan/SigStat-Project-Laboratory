using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.SignerModel
{
    internal class MyDTWSignerModel : ISignerModel
    {
        public string SignerID { get; set; }

        public double Threshold { get; set; }

       

        public List<Signature> GenuineSignatures { get; set; }
        public List<double[][]> GenuineFeatures { get; set; }




    }
}
