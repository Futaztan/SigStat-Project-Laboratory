using System;
using System.Collections.Generic;
using System.Text;
using static OfficeOpenXml.ExcelErrorValue;

namespace onlab.Functions
{
    public class TrainFunctions
    {
        public List<(string Name, Func<IEnumerable<double>, double> Method)> funcs = new List<(string Name, Func<IEnumerable<double> , double> Method)>();

        public TrainFunctions()
        {
            funcs.Add(("Average",calculateThresholdAVG));
            funcs.Add(("Max",calculateThresholdMax));
            funcs.Add(("Szoras",calculateThresholdSzoras));
            funcs.Add(("Median",calculateThresholdMedian));
        }
        public double calculateThresholdAVG(IEnumerable<double> values)
        {
            return values.Average() * 1.1;
        }

        public double calculateThresholdMedian(IEnumerable<double> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            double median = sorted[sorted.Count / 2];
            return median * 1.5; // Itt nagyobb szorzó kell, mint az átlagnál
        }
        public double calculateThresholdSzoras(IEnumerable<double> values)
        {
            double avg = values.Average();
            double sum = values.Select(v => Math.Pow(v - avg, 2)).Sum();
            double szoras = Math.Sqrt(sum / values.Count());

            return avg + (2 * szoras); // A '2'-es szorzóval érdemes kísérletezni (1.5 - 2.5 között)
        }
        public double calculateThresholdMax(IEnumerable<double> values)
        {
            return values.Max();
        }
    }
}
