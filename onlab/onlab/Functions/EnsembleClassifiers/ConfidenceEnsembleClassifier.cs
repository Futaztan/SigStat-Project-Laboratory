namespace onlab.Functions.EnsembleClassifiers;

public class ConfidenceEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Confidence";
    public double Decide(List<Result> functionPairs)
    {
        double weightedSum = 0;
        double totalWeight = 0;

        foreach (var f in functionPairs)
        {
   
            double confidence = Math.Abs(f.SimpleProbability - 0.5);

            weightedSum += f.SimpleProbability * confidence;
            totalWeight += confidence;
        }
        return weightedSum / totalWeight;
    }
}