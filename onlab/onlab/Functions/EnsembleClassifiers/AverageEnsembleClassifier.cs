namespace onlab.Functions.EnsembleClassifiers;

public class AverageEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Average";
    public double Decide(List<Result> functionPairs)
    {
        return (functionPairs.Average(f => f.SimpleProbability));
    }
}