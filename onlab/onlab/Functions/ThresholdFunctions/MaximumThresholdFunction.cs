namespace onlab.Functions.ThresholdFunctions;

public class MaximumThresholdFunction : IThresholdFunction
{
    public string Name => "Maximum";
    public double CalculateThreshold(IEnumerable<double> values)
    {
        return values.Max();
    }
}