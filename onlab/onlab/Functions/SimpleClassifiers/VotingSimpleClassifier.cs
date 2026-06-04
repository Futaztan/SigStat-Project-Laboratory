namespace onlab.Functions.SimpleClassifiers;

public class VotingSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "Voting";
    public override double Decide(List<double> values, double threshold)
    {
        List<double> list = new();
        values.ForEach(v => list.Add(ToSigmoid(v, threshold)));
        return list.Average();
    }
}