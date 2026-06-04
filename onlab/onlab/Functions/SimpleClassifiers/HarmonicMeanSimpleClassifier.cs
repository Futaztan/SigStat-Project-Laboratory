namespace onlab.Functions.SimpleClassifiers;

public class HarmonicMeanSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "HarmonicMean";
    public override double Decide(List<double> values, double threshold)
    {
        if (values.Any(v => v ==0)) return ToSigmoid(0, threshold);

        double sumOfInverses = values.Sum(v => 1.0 / v);
        double harmonicMean = values.Count / sumOfInverses;

        return ToSigmoid(harmonicMean, threshold);
    }
}