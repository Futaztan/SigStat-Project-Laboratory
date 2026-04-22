using onlab.Functions.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using static OfficeOpenXml.ExcelErrorValue;

namespace onlab.Functions
{
    public class TrainFunctions
    {
        public List<TrainFunctionDescriptor> TrainFunctionList { get; } = new List<TrainFunctionDescriptor>();

        public TrainFunctions()
        {

            TrainFunctionList.Add(new TrainFunctionDescriptor { Name = TrainFunctionName.Average, Method = calculateThresholdAVG });
            TrainFunctionList.Add(new TrainFunctionDescriptor { Name = TrainFunctionName.Max, Method = calculateThresholdMax });
            TrainFunctionList.Add(new TrainFunctionDescriptor { Name = TrainFunctionName.Szórás, Method = calculateThresholdSzoras });
            TrainFunctionList.Add(new TrainFunctionDescriptor { Name = TrainFunctionName.Medián, Method = calculateThresholdMedian });
            TrainFunctionList.Add(new TrainFunctionDescriptor { Name = TrainFunctionName.Percentilis, Method = calculateThresholdPercentilis });
        }
        private double calculateThresholdAVG(IEnumerable<double> values)
        {
            return values.Average() * 1.1;
        }

        public double calculateThresholdMedian(IEnumerable<double> values)
        {
            var sorted = values.OrderBy(v => v).ToList();
            double median = sorted[sorted.Count / 2];
            return median * 1.5;
        }
        private double calculateThresholdSzoras(IEnumerable<double> values)
        {
            double avg = values.Average();
            double sum = values.Select(v => Math.Pow(v - avg, 2)).Sum();
            double szoras = Math.Sqrt(sum / values.Count());

            return avg + (2 * szoras); 
        }
        private double calculateThresholdMax(IEnumerable<double> values)
        {
            return values.Max();
        }

        //95%os
        private double calculateThresholdPercentilis(IEnumerable<double> values)
        {
            values = values.Order();
            int which = (int)Math.Floor(values.Count() * 0.95);
            
            return values.ElementAt(which);
        }
    }
}
