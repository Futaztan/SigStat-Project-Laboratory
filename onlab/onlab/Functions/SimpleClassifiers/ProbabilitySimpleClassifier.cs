namespace onlab.Functions.SimpleClassifiers;

public class ProbabilitySimpleClassifier : SimpleClassifierBase
{
    public override string Name => "Probability";
    public override double Decide(List<double> values, double threshold)
    {
        double dist = values.Average();
        double score = threshold / (threshold + dist);
        return score;
    }
}