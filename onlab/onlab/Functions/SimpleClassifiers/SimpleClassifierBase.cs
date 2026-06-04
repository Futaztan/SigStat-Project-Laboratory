using System.Runtime.Intrinsics.Arm;

namespace onlab.Functions.SimpleClassifiers;

public abstract class SimpleClassifierBase : ISimpleClassifier
{
    public abstract string Name { get; }
    public abstract double Decide(List<double> values, double threshold);
    
    protected double ToSigmoid(double dist, double threshold)
    {

        double steepness = 2; //50 /threshold;
        return 1.0 / (1.0 + Math.Exp((dist - threshold) *  steepness));
    }
}   