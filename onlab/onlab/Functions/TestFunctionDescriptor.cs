using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class TestFunctionDescriptor
    {
     
       public TestFunctionName Name { get; set; }
       public Func<List<double>, double, double> Method { get; set; }
    }
}
