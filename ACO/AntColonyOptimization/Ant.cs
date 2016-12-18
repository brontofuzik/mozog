using System.Collections.Generic;

namespace AntColonyOptimization
{
    internal class Ant
    {
        public Ant(int dimension)
        {
            Steps = new double[dimension]; 
        }

        public double[] Steps { get; set; }

        public double Evaluation { get; set; }

        public void ConstructSolution(List<PheromoneDistribution> pheromoneTrail)
        {
            for (int i = 0; i < Steps.Length; i++)
            {
                Steps[i] = pheromoneTrail[i].GetSolutionComponent();
            }
        }
    }
}
