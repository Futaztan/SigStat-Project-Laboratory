namespace onlab.Functions.EnsembleClassifiers;

public class PermissiveEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Permissive";
    public double Decide(List<Result> functionPairs)
    {
        if (functionPairs.Any(f => f.SimpleProbability >= 0.5))
            return 1;
        return 0;
    }
}