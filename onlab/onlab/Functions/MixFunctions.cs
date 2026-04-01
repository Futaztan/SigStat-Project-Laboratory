using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    internal class MixFunctions
    {

        public double calculateThresholdAVG(IEnumerable<double> values, double szorzo = 1.1)
        {
            return values.Average() * szorzo;
        }
        public double calculateThresholdMedian(IEnumerable<double> values, double szorzo = 1.5)
        {
            var sorted = values.OrderBy(v => v).ToList();
            double median = sorted[sorted.Count / 2];
            return median * szorzo;
        }

        public double calculateThresholdSzoras(IEnumerable<double> values, double szorzo = 2)
        {
            double avg = values.Average();
            double sum = values.Select(v => Math.Pow(v - avg, 2)).Sum();
            double szoras = Math.Sqrt(sum / values.Count());

            return avg + (szorzo * szoras);
        }
        public double calculateThresholdPercentilis(IEnumerable<double> values, double szorzo = 0.95)
        {
            values = values.Order();
            int which = (int)Math.Floor(values.Count() * szorzo);

            return values.ElementAt(which);
        }
    }
}
