using System;
using System.Collections.Generic;

namespace AntColonyOptimization
{
    internal class PheromoneDistribution
    {
        #region Private static fields

        private static Random random;

        #endregion // Private static fields

        #region Private instance fields

        private double[] weights;

        private List< NormalPDF > normalPDFs;
        
        #endregion // Private instance fields

        #region Static constructor

        static PheromoneDistribution()
        {
            random = new Random();
        }

        #endregion // Static constructor

        #region Public instance constructors

        /// <summary>
        /// Creates a new pheromone distribution.
        /// </summary>
        /// <param name="normalPDFCount">The number of normal PDFs.</param>
        public PheromoneDistribution( int normalPDFCount )
        {
            // Create the weights.
            double weight = 1.0 / (double)normalPDFCount;
            weights = new double[ normalPDFCount ];
            for (int i = 0; i < normalPDFCount; i++)
            {
                weights[ i ] = weight;
            }

            // Create the normal PDFs.
            double a = -1.0;
            double b = 1.0;
            double standardDeviation = (b - a) / (double)(2 * normalPDFCount);

            normalPDFs = new List< NormalPDF >( normalPDFCount );
            for (int i = 0; i < normalPDFCount; i++)
            {
                double mean = 2 * random.NextDouble() - 1;
                // ALT:
                // double mean = a + (2 * i - 1) * ((b - a) / (double)(2 * normalPDFCount));

                normalPDFs.Add( new NormalPDF( mean, standardDeviation ) );
            }
        }

        #endregion // Public instance constructors

        #region Public instance methods

        public double GetSolutionComponent()
        {
            // 1. Choose probabilistically a single normal PDF from the mixture.
            double weightSum = 0.0;
            foreach (double weight in weights)
            {
                weightSum += weight;
            }
            double[] probabilities = new double[ weights.Length ];
            for (int i = 0; i < probabilities.Length; i++)
            {
                probabilities[ i ] = weights[ i ] / weightSum;
            }

            // Create the roulette-wheel.
            List< double > rouletteWheel = new List< double >( probabilities.Length );
            double previousPocketCount = 0.0;
            foreach (double probability in probabilities)
            {
                double currentPocketCount = previousPocketCount + probability;
                rouletteWheel.Add( currentPocketCount );
                previousPocketCount = currentPocketCount;
            }

            // Spin the roulette-wheel.
            double pocket = random.NextDouble();
            int normalPDFIndex = rouletteWheel.BinarySearch( pocket );
            if (normalPDFIndex < 0)
            {
                normalPDFIndex = ~normalPDFIndex;
            }

            // 2. Generates a random number according to the chosen PDF.
            return normalPDFs[ normalPDFIndex ].NextDouble();
        }

        public void Update( double mean, double standardDeviation )
        {
            foreach (NormalPDF normalPDF in normalPDFs)
            {
                normalPDF.Mature();
            }
            PositiveUpdate( mean, standardDeviation );
            NegativeUpdate();
        }

        #endregion // Public instance methods

        #region Private instance methods

        private void PositiveUpdate( double mean, double standardDeviation )
        {
            normalPDFs.Add( new NormalPDF( mean, standardDeviation ) );
        }

        private void NegativeUpdate()
        {
            NormalPDF oldestNormalPDF = normalPDFs[ 0 ];
            foreach (NormalPDF normalPDF in normalPDFs)
            {
                if (normalPDF.Age > oldestNormalPDF.Age)
                {
                    oldestNormalPDF = normalPDF;
                }
            }
            normalPDFs.Remove( oldestNormalPDF );
        }

        #endregion // Private insatnce methods
    }
}
