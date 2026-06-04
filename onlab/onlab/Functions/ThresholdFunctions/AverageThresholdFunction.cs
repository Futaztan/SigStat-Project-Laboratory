namespace onlab.Functions.ThresholdFunctions;

public class AverageThresholdFunction : IThresholdFunction
{
    public string Name => "Average";
    public double CalculateThreshold(IEnumerable<double> values)
    {
        return values.Average() * 1.1;
    }
}