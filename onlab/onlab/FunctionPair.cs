using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace onlab
{
    public class FunctionPair
    {
        public TestFunctionName TestName{ get; set; }
        public Func<List<double>, double, double> TestFunction { get; set; }
        public TrainFunctionName TrainName { get; set; }
        public Func<IEnumerable<double>, double> TrainFunction { get; set; }
        public double Probability { get; set; }
        public double Threshold { get; set; }

    }
}
