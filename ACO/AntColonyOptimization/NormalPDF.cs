using System;

namespace AntColonyOptimization
{
    internal class NormalPDF
    {
        private static readonly Random random = new Random();

        public NormalPDF(double mean, double standardDeviation)
        {
            Mean = mean;
            StandardDeviation = standardDeviation;
            Age = 0;
        }

        public double Mean { get; }

        public double StandardDeviation { get; }

        public int Age { get; private set; }

        public double NextDouble()
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double z1 = Mean + StandardDeviation * Math.Sqrt(-2 * Math.Log(u1)) * Math.Cos(2 * Math.PI * u2);
            double z2 = Mean + StandardDeviation * Math.Sqrt(-2 * Math.Log(u1)) * Math.Sin(2 * Math.PI * u2);
            
            return z1;
        }

        public void Mature()
        {
            Age++;
        }
    }
}
