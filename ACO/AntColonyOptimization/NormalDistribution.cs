using System;
using Mozog.Utils.Math;

namespace AntColonyOptimization
{
    internal class NormalDistribution
    {
        private readonly double mean;
        private readonly double stdDev;

        public NormalDistribution(double mean, double stdDev)
        {
            this.mean = mean;
            this.stdDev = stdDev;
            Age = 0;
        }

        public int Age { get; set; }

        public double GetRandomDouble() => StaticRandom.Normal(mean, stdDev);

        public void Mature()
        {
            Age++;
        }
    }
}
