using System;
using System.Collections.Generic;
using System.Text;

namespace onlab.Functions
{
    public class TestFunctions
    {
        public List<(string Name, Func<List<double>, double, double> Method)> funcs = new List<(string Name, Func<List<double>, double, double> Method)>();

        public TestFunctions()
        {
            funcs.Add(("K nearest",testSignatureKNN));
            funcs.Add(("Probability",testSignatureProbability));
            funcs.Add(("Average",testSignaturesAVG));
            funcs.Add(("Min",testSignaturesMIN));
            funcs.Add(("Voting",testSignatureVoting));
        }
        public double testSignaturesAVG(List<double> values, double threshold)
        {
            if (values.Average() < threshold) return 1;
            return 0;
        }

        public double testSignaturesMIN(List<double> values, double threshold)
        {
            if (values.Min() < threshold) return 1;
            return 0;
        }
        //k-Nearest Neighbor
        public double testSignatureKNN(List<double> values, double threshold)
        {
            int k = 3; // Nézzük a 3 legközelebbi mintát
            var kBestValues = values.OrderBy(v => v).Take(k);
            return kBestValues.Average() < threshold ? 1.0 : 0.0;
        }
        public double testSignatureProbability(List<double> values, double threshold)
        {
            double dist = values.Min();
            if (dist == 0) return 1.0;

            // Egy egyszerű fordított arányosság: minél nagyobb a távolság, annál kisebb a pontszám
            double score = threshold / (threshold + dist);
            return score;
        }
        //Ha van 5 tanító mintánk, és az új aláírás legalább 3-hoz közelebb van, mint a küszöb, akkor elfogadjuk.
        public double testSignatureVoting(List<double> values, double threshold)
        {
            int votes = values.Count(v => v < threshold);
            // Ha a minták több mint feléhez közel van
            return (votes > values.Count / 2) ? 1.0 : 0.0;
        }
    }
}
