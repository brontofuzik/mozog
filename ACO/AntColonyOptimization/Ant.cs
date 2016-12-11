using System.Collections.Generic;

namespace AntColonyOptimization
{
    internal class Ant
    {
        private double[] steps;

        private double evaluation;

        public Ant( int dimension )
        {
            steps = new double[ dimension ]; 
        }

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

        public void ConstructSolution( List< PheromoneDistribution > pheromoneTrail )
        {
            for (int i = 0; i < steps.Length; i++)
            {
                steps[ i ] = pheromoneTrail[ i ].GetSolutionComponent();
            }
        }
    }
}
