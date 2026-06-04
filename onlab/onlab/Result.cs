namespace onlab
{
    public class Result
    {
        public string SimpleClassifierName { get; set; }
        public string ThresholdFunctionName { get; set; }
        public double AER { get; set; }
        public double FAR { get; set; }
        public double FRR { get; set; }

        public double SimpleProbability { get; set; }
        public double Threshold { get; set; }

        public string EnsembleClassifierName { get; set; }
        public List<string> FeatureNames { get; set; }


        public void Print()
        {
            Console.WriteLine("TEST METHOD: " + ThresholdFunctionName + "\t TRAIN METHOD: " + SimpleClassifierName);
            Console.WriteLine($"AER (Average Error Rate): {AER}");
            Console.WriteLine($"FAR (False Acceptance Rate): {FAR}");
            Console.WriteLine($"FRR (False Rejection Rate): {FRR}");
            Console.WriteLine("");
        }
    }
}