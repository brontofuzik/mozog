using System.Collections.Generic;
using System.Linq;

namespace AntColonyOptimization
{
    internal class Ant
    {
        private readonly AntColonyOptimization algo;

        public Ant(AntColonyOptimization algo)
        {
            this.algo = algo;
        }

        public double[] Steps { get; private set; }

        public double Evaluation { get; set; }

        public void ConstructSolution(List<PheromoneDistribution> pheromoneTrail)
        {
            Steps = pheromoneTrail.Select(x => x.GetSolutionComponent()).ToArray();
        }

        public void Evaluate()
        {
            Evaluation = algo.ObjectiveFunc.Evaluate(Steps);
        }
    }
}
