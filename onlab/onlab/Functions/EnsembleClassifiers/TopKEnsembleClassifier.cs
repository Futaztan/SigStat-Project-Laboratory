namespace onlab.Functions.EnsembleClassifiers;

public class TopKEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Top K";
    public double Decide(List<Result> functionPairs)
    {
        return functionPairs.OrderByDescending(r => r.SimpleProbability).Take(5).Average(r => r.SimpleProbability);
    }
}