namespace onlab.Functions.ThresholdFunctions;

public class MedianThresholdFunction : IThresholdFunction
{
    public string Name => "Median";
    public double CalculateThreshold(IEnumerable<double> values)
    {
        var sorted = values.OrderBy(v => v).ToList();
        double median = sorted[sorted.Count / 2];
        return median * 1.5;
    }
}