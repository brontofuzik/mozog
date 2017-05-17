using System.Collections.Generic;
using System.Linq;
using Mozog.Utils.Math;

namespace AntColonyOptimization
{
    internal class PheromoneDistribution
    {
        private readonly double[] weights;

        private readonly List<NormalDistribution> distributions;

        public PheromoneDistribution(int count)
        {
            weights = Enumerable.Repeat(1.0 / count, count).ToArray();

            distributions = new List<NormalDistribution>(count);

            double a = -1.0;
            double b = 1.0;
            double stdDev = (b - a) / (2 * count);
       
            for (int i = 0; i < count; i++)
            {
                double mean = 2 * StaticRandom.Double() - 1;
                // ALT: mean = a + (2 * i - 1) * ((b - a) / (double)(2 * normalPDFCount));

                distributions.Add(new NormalDistribution(mean, stdDev));
            }
        }

        public double GetSolutionComponent()
        {
            var randomIndex = new RouletteWheel(weights).Spin();
            return distributions[randomIndex].GetRandomDouble();
        }

        public void Update(double mean, double standardDeviation)
        {
            distributions.ForEach(d => d.Mature());

            // TODO Can we switch this?
            distributions.Add(new NormalDistribution(mean, standardDeviation));
            distributions.Remove(GetOldestDistribution());
        }

        private NormalDistribution GetOldestDistribution()
            => distributions.Aggregate((max, x) => x.Age > max.Age ? x : max);

        private class RouletteWheel
        {
            private readonly List<double> wheel;

            public RouletteWheel(double[] weights)
            {
                double weightSum = weights.Sum();
                var probabilities = weights.Select(w => w / weightSum).ToArray();

                wheel = new List<double>(probabilities.Length);

                double pocket = 0.0;
                foreach (double p in probabilities)
                {
                    wheel.Add(pocket += p);
                }
            }

            public int Spin()
            {
                double pocket = StaticRandom.Double();
                int index = wheel.BinarySearch(pocket);
                if (index < 0)
                {
                    index = ~index;
                }
                return index;
            }
        }
    }
}
