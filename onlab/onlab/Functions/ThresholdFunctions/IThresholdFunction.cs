namespace onlab.Functions.ThresholdFunctions;

public interface IThresholdFunction
{
    string Name { get; }
    double CalculateThreshold(IEnumerable<double> values);
}