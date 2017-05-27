using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace AntColonyOptimization
{
    public class AntColonyOptimization
    {
        private List<Ant> antColony;
        private List<PheromoneDistribution> pheromoneTrail;
        private Ant globalBestAnt;

        private double targetEvaluation;
        private int maxIterations;

        private const double accuracy = 0.001;

        public AntColonyOptimization(int dimension)
        {
            Dimension = dimension;
        }

        public int Dimension { get; }

        private Objective Objective => ObjectiveFunc.Objective;

        #region Functions

        public ObjectiveFunction ObjectiveFunc { get; set; }

        #endregion // Functions

        public Result<double> Run(int antCount, int gaussianCount, double? targetEvaluation = null, int? maxIterations = null)
        {
            this.maxIterations = maxIterations ?? Int32.MaxValue;
            this.targetEvaluation = targetEvaluation ?? (Objective == Objective.Minimize ? Double.MinValue : Double.MaxValue);

            Initialize(gaussianCount, antCount);

            int iteration = 0;
            while (!IsDone(iteration))
            {
                EvaluateAntColony();
                UpdatePheromoneTrail(iteration + 1, accuracy);

                iteration++;
            }

            return new Result<double>(globalBestAnt.Steps, globalBestAnt.Evaluation, iteration);
        }

        private void Initialize(int gaussianCount, int antCount)
        {
            pheromoneTrail = Dimension.Times(() => new PheromoneDistribution(gaussianCount)).ToList();
            antColony = antCount.Times(() => new Ant(this)).ToList();
        }

        private void EvaluateAntColony()
        {
            foreach (var ant in antColony)
            {
                ant.ConstructSolution(pheromoneTrail);
                ant.Evaluate();

                // Elitism
                if (globalBestAnt == null || ant.Evaluation < globalBestAnt.Evaluation)
                {
                    globalBestAnt = ant;
                }
            }
        }

        private void UpdatePheromoneTrail(int iterationCount, double accuracy)
        {
            var iterationBestAnt = antColony.Aggregate((bestAnt, ant) => ant.Evaluation < bestAnt.Evaluation ? ant : bestAnt);

            // Use the iteration-best ant to update the pheromone trail.
            for (int i = 0; i < pheromoneTrail.Count; i++)
            {
                double mean = iterationBestAnt.Steps[i];
                double max = antColony.Select(a => a.Steps[i]).Max();
                double min = antColony.Select(a => a.Steps[i]).Min();
                double stdDev = Math.Max((max - min) / Math.Sqrt(iterationCount), accuracy);

                pheromoneTrail[i].Update(mean, stdDev);
            }
        }

        private bool IsDone(int iteration)
            => ObjectiveFunc.IsAcceptable(globalBestAnt.Steps, targetEvaluation) || iteration >= maxIterations;
    }

    public struct Result<T>
    {
        public T[] Solution { get; }

        public double Evaluation { get; }

        public int Iterations { get; }

        public Result(T[] solution, double evaluation, int iterations)
        {
            Solution = solution;
            Evaluation = evaluation;
            Iterations = iterations;
        }
    }
}
