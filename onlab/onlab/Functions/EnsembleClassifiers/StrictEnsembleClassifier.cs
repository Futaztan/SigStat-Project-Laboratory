namespace onlab.Functions.EnsembleClassifiers;

public class StrictEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Strict";
    public double Decide(List<Result> functionPairs)
    {
        if (functionPairs.Any(f => f.SimpleProbability < 0.5))
            return 0;
        return 1;
    }
}