namespace onlab.Functions.SimpleClassifiers;

public class MinimumSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "Minimum";

    public override double Decide(List<double> values, double threshold)
    {
        return ToSigmoid(values.Min(), threshold);
    }
}