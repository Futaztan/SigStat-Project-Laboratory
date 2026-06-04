namespace onlab.Functions.SimpleClassifiers;

public interface ISimpleClassifier
{
    string Name { get; }
    double Decide(List<double> values, double threshold);
}