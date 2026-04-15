using SigStat.Common;
using SigStat.Common.Pipeline;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.SignerModel
{
    internal class MultipleDTWSignerModel : ISignerModel
    {
        public string SignerID { get; set; }

        public double Threshold { get; set; }
        public FunctionPair FunctionPair { get; set; }


        public List<Signature> GenuineSignatures { get; set; }
        public List<double[][]> GenuineFeatures { get; set; }
        public List<FunctionPair> TrainResults { get; set; } = new List<FunctionPair>();
    }
}
