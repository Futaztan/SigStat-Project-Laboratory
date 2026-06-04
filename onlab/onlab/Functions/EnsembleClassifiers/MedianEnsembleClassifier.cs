namespace onlab.Functions.EnsembleClassifiers;

public class MedianEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Median";
    public double Decide(List<Result> functionPairs)
    {
        var sorted = functionPairs.Select(r => r.SimpleProbability).OrderBy(p => p).ToList();
        int count = sorted.Count;
        if (count % 2 == 0)
            return (sorted[count / 2 - 1] + sorted[count / 2]) / 2;
        return sorted[count / 2];
    }
}