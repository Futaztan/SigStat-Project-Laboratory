namespace onlab.Functions.EnsembleClassifiers;

public class VotingEnsembleClassifier : IEnsembleClassifier
{
    public string Name => "Voting";
    public double Decide(List<Result> functionPairs)
    {
        if (functionPairs.Count == 0) return 0;
        int votes = functionPairs.Count(f => f.SimpleProbability > 0.5);
        return (double)votes / functionPairs.Count();
    }
}