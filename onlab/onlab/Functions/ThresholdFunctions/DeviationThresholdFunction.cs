namespace onlab.Functions.ThresholdFunctions;

public class DeviationThresholdFunction : IThresholdFunction
{
    public string Name => "Deviation";
    public double CalculateThreshold(IEnumerable<double> values)
    {
        double avg = values.Average();
        double sum = values.Select(v => Math.Pow(v - avg, 2)).Sum();
        double szoras = Math.Sqrt(sum / values.Count());

        return avg + (2 * szoras); 
    }
}