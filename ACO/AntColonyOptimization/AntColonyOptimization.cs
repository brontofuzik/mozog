using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace AntColonyOptimization
{
    public class AntColonyOptimization
    {
        private List<PheromoneDistribution> pheromoneTrail;
        private List<Ant> antColony;
        private Ant globalBestAnt;

        private double targetEvaluation;
        private int maxIterations;

        private const double requiredAccuracy = 0.001;

        public AntColonyOptimization(int dimension)
        {
            Dimension = dimension;
        }

        public int Dimension { get; }

        #region Functions

        private ObjectiveFunction objective;
        public ObjectiveFunction Objective
        {
            get => objective;
            set => objective = value;
        }

        #endregion // Functions

        public Result<double> Run(int antCount, int normalPdfCount, double targetEvaluation, int maxIterations = Int32.MaxValue)
        {
            this.maxIterations = maxIterations;
            this.targetEvaluation = targetEvaluation;

            Initialize(normalPdfCount, antCount);

            // Elitism
            globalBestAnt = antColony[0];
            globalBestAnt.Evaluate();

            int iteration = 0;
            while (!IsDone(iteration))
            {
                EvaluateAntColony();
                UpdatePheromoneTrail(iteration + 1, requiredAccuracy);

                iteration++;
            }

            return new Result<double>(globalBestAnt.Steps, objective.EvaluateObjective(globalBestAnt.Steps), iteration);
        }

        private void Initialize(int normalPdfCount, int antCount)
        {
            pheromoneTrail = Dimension.Times(() => new PheromoneDistribution(normalPdfCount)).ToList();
            antColony = antCount.Times(() => new Ant(this)).ToList();
        }

        private void EvaluateAntColony()
        {
            foreach (var ant in antColony)
            {
                ant.ConstructSolution(pheromoneTrail);
                ant.Evaluate();

                // Elitism
                if (ant.Evaluation < globalBestAnt.Evaluation)
                {
                    globalBestAnt = ant;
                }
            }
        }

        private void UpdatePheromoneTrail(int iterationCount, double requiredAccuracy)
        {
            var iterationBestAnt = antColony.Aggregate((bestAnt, ant) => ant.Evaluation < bestAnt.Evaluation ? ant : bestAnt);

            // Use the iteration-best ant to update the pheromone trail.
            for (int i = 0; i < pheromoneTrail.Count; i++)
            {
                double mean = iterationBestAnt.Steps[i];
                double max = antColony.Select(a => a.Steps[i]).Max();
                double min = antColony.Select(a => a.Steps[i]).Min();
                double stdDev = Math.Max((max - min) / Math.Sqrt(iterationCount), requiredAccuracy);

                pheromoneTrail[i].Update(mean, stdDev);
            }
        }

        private bool IsDone(int iteration)
            => objective.IsAcceptable(globalBestAnt.Steps, targetEvaluation) || iteration >= maxIterations;
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
