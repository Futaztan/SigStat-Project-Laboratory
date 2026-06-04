namespace onlab.Functions.SimpleClassifiers;

public class KNearestSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "K Nearest";
    public override double Decide(List<double> values, double threshold)
    {
        int k = 10;
        var kBestValues = values.Order().Take(k);
        return ToSigmoid(kBestValues.Average(), threshold);
    }
}