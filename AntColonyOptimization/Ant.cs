using System.Collections.Generic;

namespace AntColonyOptimization
{
    internal class Ant
    {
        #region Private instance fields

        private double[] steps;

        private double evaluation;

        #endregion // Private instance fields

        #region Public instance properties

        public double[] Steps
        {
            get
            {
                return steps;
            }
            set
            {
                steps = value;
            }
        }

        public double Evaluation
        {
            get
            {
                return evaluation;
            }
            set
            {
                evaluation = value;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        public Ant( int dimension )
        {
            steps = new double[ dimension ]; 
        }

        #endregion // Public instance constructors

        #region Public instance methods

        public void ConstructSolution( List< PheromoneDistribution > pheromoneTrail )
        {
            for (int i = 0; i < steps.Length; i++)
            {
                steps[ i ] = pheromoneTrail[ i ].GetSolutionComponent();
            }
        }

        #endregion // Public instance methods
    }
}
