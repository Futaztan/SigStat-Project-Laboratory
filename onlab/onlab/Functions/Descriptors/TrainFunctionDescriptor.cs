using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions.Descriptors
{
    public class TrainFunctionDescriptor
    {
      
       public TrainFunctionName Name { get; set; }
       public Func<IEnumerable<double>, double> Method { get; set; }
    }
}
