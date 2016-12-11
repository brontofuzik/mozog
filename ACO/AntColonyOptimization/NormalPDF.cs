using System;

namespace AntColonyOptimization
{
    internal class NormalPDF
    {
        private static Random random;

        double mean;

        double standardDeviation;

        int age;

        static NormalPDF()
        {
            random = new Random();
        }

        public NormalPDF( double mean, double standardDeviation )
        {
            this.mean = mean;
            this.standardDeviation = standardDeviation;
            age = 0;
        }

        public double Mean
        {
            get
            {
                return mean;
            }
        }

        public double StandardDeviation
        {
            get
            {
                return standardDeviation;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
        }

        public double NextDouble()
        {
            double u1 = random.NextDouble();
            double u2 = random.NextDouble();
            double z1 = mean + standardDeviation * Math.Sqrt( -2 * Math.Log( u1 )) * Math.Cos( 2 * Math.PI * u2 );
            double z2 = mean + standardDeviation * Math.Sqrt( -2 * Math.Log( u1 )) * Math.Sin( 2 * Math.PI * u2 );
            
            return z1;
        }

        public void Mature()
        {
            age++;
        }
    }
}
