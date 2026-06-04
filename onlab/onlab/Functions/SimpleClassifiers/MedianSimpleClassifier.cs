namespace onlab.Functions.SimpleClassifiers;

public class MedianSimpleClassifier : SimpleClassifierBase
{
    public override string Name => "Median";
    public override double Decide(List<double> values, double threshold)
    {
        
        var sortedValues = values.OrderBy(v => v).ToList();
        int midIndex = sortedValues.Count / 2;

        double median;
        if (sortedValues.Count % 2 != 0)
        {
            median = sortedValues[midIndex];
        }
        else
        {
            median = (sortedValues[midIndex - 1] + sortedValues[midIndex]) / 2.0;
        }

        return ToSigmoid(median, threshold);
    }
}