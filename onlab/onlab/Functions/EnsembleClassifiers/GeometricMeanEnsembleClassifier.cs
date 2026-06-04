namespace onlab.Functions.EnsembleClassifiers;

public class GeometricMeanEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Geometric Mean";
    public double Decide(List<Result> functionPairs)
    {
        if (functionPairs.Count == 0) return 0;

        double product = 1.0;
        foreach (var f in functionPairs)
        {

            product *= f.SimpleProbability;
        }

        return Math.Pow(product, 1.0 / functionPairs.Count);
    }
}