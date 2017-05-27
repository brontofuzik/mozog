using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace AntColonyOptimization
{
    internal class PheromoneDistribution
    {
        private const double a = -1.0;
        private const double b = 1.0;

        private readonly List<Gaussian> gaussianKernel;
        private readonly double[] weights;

        public PheromoneDistribution(int count)
        {
            weights = Enumerable.Repeat(1.0 / count, count).ToArray();

            double stdDev = (b - a) / (2 * count);
            gaussianKernel = count.Times(() => new Gaussian(StaticRandom.Double(a, b), stdDev)).ToList();
        }

        public double GetSolutionComponent()
        {
            var randomIndex = new RouletteWheel(weights).Spin();
            return gaussianKernel[randomIndex].GetRandomDouble();
        }

        public void Update(double mean, double stdDev)
        {
            gaussianKernel.ForEach(d => d.Mature());

            // TODO Can we switch this?
            gaussianKernel.Remove(GetOldestDistribution());
            gaussianKernel.Add(new Gaussian(mean, stdDev));
        }

        private Gaussian GetOldestDistribution()
            => gaussianKernel.Aggregate((max, x) => x.Age > max.Age ? x : max);

        private class Gaussian
        {
            private readonly double mean;
            private readonly double stdDev;

            public Gaussian(double mean, double stdDev)
            {
                this.mean = mean;
                this.stdDev = stdDev;
                Age = 0;
            }

            public int Age { get; private set; }

            public double GetRandomDouble() => StaticRandom.Normal(mean, stdDev);

            public void Mature()
            {
                Age++;
            }
        }

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
