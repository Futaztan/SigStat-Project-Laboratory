using System;
using System.Collections.Generic;
using System.Text;

namespace onlab
{
    public class Result
    {
        public string TrainName { get; set; }
        public string TestName { get; set; }
        public double AER { get; set; }
        public double FAR { get; set; }
        public double FRR { get; set; }


        public void Print()
        {

            Console.WriteLine("TEST METHOD: " + TestName + "\t TRAIN METHOD: " + TrainName);
            Console.WriteLine($"AER (Average Error Rate): {AER}");
            Console.WriteLine($"FAR (False Acceptance Rate): {FAR}");
            Console.WriteLine($"FRR (False Rejection Rate): {FRR}");
            Console.WriteLine("");

        }
    }

}
