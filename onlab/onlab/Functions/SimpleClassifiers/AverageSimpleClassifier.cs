namespace onlab.Functions.SimpleClassifiers;

public class AverageSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "Average";

    public override double Decide(List<double> values, double threshold)
    {
        return ToSigmoid(values.Average(), threshold);
    }
}