namespace onlab.Functions.ThresholdFunctions;

public class PercentileThresholdFunction : IThresholdFunction
{
    public string Name => "Percentile";

    public double CalculateThreshold(IEnumerable<double> values)
    {
        values = values.Order();
        int which = (int)Math.Floor(values.Count() * 0.95);

        return values.ElementAt(which);
    }
}