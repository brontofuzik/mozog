using System;

namespace AntColonyOptimization
{
    internal class NormalPDF
    {
        #region Private static fields

        private static Random random;

        #endregion // Private static fields

        #region Private instance fields

        double mean;

        double standardDeviation;

        int age;

        #endregion // Private instance fields

        #region Public instance properties

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

        #endregion // Public instance properties

        #region Static constructor

        static NormalPDF()
        {
            random = new Random();
        }

        #endregion // Static constructor

        #region Public instance constructors

        public NormalPDF( double mean, double standardDeviation )
        {
            this.mean = mean;
            this.standardDeviation = standardDeviation;
            age = 0;
        }

        #endregion // Public instance constructors

        #region Public instance methods

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

        #endregion // Public instance methods
    }
}
