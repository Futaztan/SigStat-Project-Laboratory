using System;
using System.Collections.Generic;
using System.Text;

namespace onlab
{
    public class FunctionPair
    {
        public TestFunctionName TestFunction { get; set; }
        public TrainFunctionName TrainFunction { get; set; }
        public double Probability { get; set; }
        public double Threshold { get; set; }

    }
}
