namespace onlab.Functions.EnsembleClassifiers;

public interface IEnsembleClassifier
{
    string Name { get; }
    double Decide(List<Result> functionPairs);
}