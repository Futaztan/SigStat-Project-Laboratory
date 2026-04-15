using System;
using System.Collections.Generic;
using System.Text;
using static OfficeOpenXml.ExcelErrorValue;

namespace onlab.Functions
{
    public class TrainFunctions
    {
        public List<(TrainFunctionName Name, Func<IEnumerable<double>, double> Method)> funcs = new List<(TrainFunctionName Name, Func<IEnumerable<double> , double> Method)>();

        public TrainFunctions()
        {
            funcs.Add((TrainFunctionName.Average,calculateThresholdAVG));
            funcs.Add((TrainFunctionName.Max, calculateThresholdMax));
            funcs.Add((TrainFunctionName.Szórás, calculateThresholdSzoras));
            funcs.Add((TrainFunctionName.Medián, calculateThresholdMedian));
            funcs.Add((TrainFunctionName.Percentilis, calculateThresholdPercentilis));
        }
        public double calculateThresholdAVG(IEnumerable<double> values)
        {
            return values.Average() * 1.1;
        }

        public double calculateThresholdMedian(IEnumerable<double> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            double median = sorted[sorted.Count / 2];
            return median * 1.5;
        }
        public double calculateThresholdSzoras(IEnumerable<double> values)
        {
            double avg = values.Average();
            double sum = values.Select(v => Math.Pow(v - avg, 2)).Sum();
            double szoras = Math.Sqrt(sum / values.Count());

            return avg + (2 * szoras); 
        }
        public double calculateThresholdMax(IEnumerable<double> values)
        {
            return values.Max();
        }

        //95%os
        public double calculateThresholdPercentilis(IEnumerable<double> values)
        {
            values = values.Order();
            int which = (int)Math.Floor(values.Count() * 0.95);
            
            return values.ElementAt(which);
        }
    }
}
