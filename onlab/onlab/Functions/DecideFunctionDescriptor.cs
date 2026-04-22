using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class DecideFunctionDescriptor
    {
        public DecideFunctionName Name { get; set; }
        public Func<List<FunctionPair>, double> Method { get; set; }
    }
}
