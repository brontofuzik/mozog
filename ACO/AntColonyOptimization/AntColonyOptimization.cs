using System;
using System.Collections.Generic;

namespace AntColonyOptimization
{
    /// <summary>
    /// ACO for Continuous and Mixed-Variable Optimization (Krzysztof Socha)
    /// http://www.springerlink.com/content/qhx703cmtgrk3pv8/fulltext.pdf
    /// </summary>
    public class AntColonyOptimization
    {
        protected Random random;

        private ObjectiveFunction objectiveFunction;

        private List< PheromoneDistribution > pheromoneTrail;

        private List< Ant > antColony;

        private Ant globalBestAnt;

        /// <summary>
        /// Creates a new ant colony optimization.
        /// </summary>
        public AntColonyOptimization()
        {
            random = new Random();
            objectiveFunction = null;
            pheromoneTrail = new List< PheromoneDistribution >();
            antColony = new List< Ant >();
            globalBestAnt = null;
        }

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public int Dimension
        {
            get
            {
                return objectiveFunction.Dimension;
            }
        }

        /// <summary>
        /// Gets the objective (minimize or maximize).
        /// </summary>
        /// <value>
        /// The objective (minimize or mazimize).
        /// </value>
        public Objective Objective
        {
            get
            {
                return objectiveFunction.Objective;
            }
        }

        public double[] Run(ObjectiveFunction objectiveFunction,
            int maxIterationCount, out int usedIterationCount, double acceptableEvaluation, out double achievedEvalation,
            int antCount, int normalPDFCount, double requiredAccuracy
       )
        {
            if (objectiveFunction == null)
            {
                throw new ArgumentNullException("objectiveFunction");
            }
            this.objectiveFunction = objectiveFunction;

            // Pheromone Maintenance - Initialization
            CreatePheromoneTrail(normalPDFCount);

            CreateAntColony(antCount);

            // Keep track of the global-best ant for elitist purposes.
            globalBestAnt = antColony[0];
            EvaluateAnt(globalBestAnt);

            // Repeat while the computational budget is not spend and an acceptable solution is not found.
            int iterationIndex = 0;
            while (iterationIndex < maxIterationCount && !IsAcceptableSolutionFound(acceptableEvaluation))
            {
                // Solution Construction
                EvaluateAntColony();

                // Pheromone Maintenance - Update
                UpdatePheromoneTrail(iterationIndex + 1, requiredAccuracy);

                iterationIndex++;
            }

            // Return the (global-best) solution.
            usedIterationCount = iterationIndex;
            achievedEvalation = objectiveFunction.Objective == Objective.Minimize ? globalBestAnt.Evaluation : 1 / globalBestAnt.Evaluation;
            return globalBestAnt.Steps;
        }

        ///// <summary>
        ///// Runs the ant colony optimization.
        ///// </summary>
        ///// <param name="iterationCount">The number of iterations.</param>
        ///// <returns>
        ///// The (global-best) solution.
        ///// </returns>
        //public double[] Run(int iterationCount)
        //{
        //    return Run(iterationCount, 100, 5, 0.001, true);
        //}

        ///// <summary>
        ///// Runs the ant colony optimization "ruby-way".
        ///// </summary>
        ///// <param name="args">The arguments.</param>
        ///// <returns>
        ///// The (global-best) solution.
        ///// </returns>
        //public double[] Run(Dictionary<string, object > args)
        //{
        //    // Validate the presence of compulasory parameters.
        //    // The number of iterations.
        //    int iterationCount;
        //    try
        //    {
        //        iterationCount = (int)args["iterationCount"];
        //    }
        //    catch
        //    {
        //        throw new ArgumentException("iterationCount");
        //    }

        //    // Validate the presence of optional parameters.
        //    // The number of ants
        //    int antCount = (args.ContainsKey("antCOunt")) ? (int)args["antCount"] : 100;
            
        //    // The number of normal PDFs.
        //    double normalPDFCount = (args.ContainsKey("normalPDFCount")) ? (double)args["normalPDFCount"] : 5;
            
        //    // The required accuracy.
        //    double requiredAccuracy = (args.ContainsKey("requiredAccuracy")) ? (double)args["requiredAccuracy"] : 0.001;
            
        //    // The elitism flag.
        //    bool elitism  = (args.ContainsKey("elitism")) ? (bool)args["elitism"] : true;

        //    return Run(iterationCount, antCount, normalPDFCount, requiredAccuracy, elitism);
        //}

        /// <summary>
        /// Creates the pheromone trail.
        /// </summary>
        /// <param name="normalPDFCount">The number of PDFs in each pheromone distribution.</param>
        private void CreatePheromoneTrail(int normalPDFCount)
        {
            // Clear the pheromone trail, ...
            pheromoneTrail.Clear();

            // ... and populate it with new pheromone distributions.
            for (int i = 0; i < Dimension; i++)
            {
                // Create a random pheromone distribution, ...
                PheromoneDistribution pheromoneDistribution = new PheromoneDistribution(normalPDFCount);
                
                // ... and add it to the pheromone trail.
                pheromoneTrail.Add(pheromoneDistribution);
            }
        }

        /// <summary>
        /// Creates the ant colony.
        /// </summary>
        /// <param name="antCount">The number of ants.</param>
        private void CreateAntColony(int antCount)
        {
            // Clear the ant colony, ...
            antColony.Clear();

            // ... and populate it with new ants.
            for (int i = 0; i < antCount; i++)
            {
                // Create a new ant, ...
                Ant ant = new Ant(Dimension);

                // ... and add it to the ant colony.
                antColony.Add(ant);
            }
        }

        /// <summary>
        /// Evaluates the ant colony.
        /// </summary>
        private void EvaluateAntColony()
        {
            foreach (Ant ant in antColony)
            {
                // Solution Construction
                ant.ConstructSolution(pheromoneTrail);
                EvaluateAnt(ant);

                // Update the global-best ant.
                if (ant.Evaluation < globalBestAnt.Evaluation)
                {
                    globalBestAnt = ant;
                }
            }
        }

        /// <summary>
        /// Updates the pheromone trail.
        /// </summary>
        /// <param name="iterationIndex">The index of the iteration.</param>
        /// <param name="requiredAccuracy">The required accuracy.</param>
        private void UpdatePheromoneTrail(int iterationCount, double requiredAccuracy)
        {
            // Find the iteration-best ant.
            Ant iterationBestAnt = antColony[0];;
            foreach (Ant ant in antColony)
            {
                if (ant.Evaluation < iterationBestAnt.Evaluation)
                {
                    iterationBestAnt = ant;
                }
            }

            // Use the iteration-best ant to update the oheromone trail.
            for (int i = 0; i < pheromoneTrail.Count; i++)
            {
                double mean = iterationBestAnt.Steps[i];
                double max = Double.MinValue;
                double min = Double.MaxValue;
                foreach (Ant ant in antColony)
                {
                    max = Math.Max(max, ant.Steps[i]);
                    min = Math.Min(min, ant.Steps[i]);
                }
                double standardDeviation = Math.Max((max - min) / Math.Sqrt(iterationCount), requiredAccuracy);
                pheromoneTrail[i].Update(mean, standardDeviation);
            }
        }

        /// <summary>
        /// Evalautes an ant.
        /// </summary>
        /// <param name="ant">The ant to evalaute.</param>
        private void EvaluateAnt(Ant ant)
        {
            ant.Evaluation = Objective == Objective.Minimize ? objectiveFunction.Evaluate(ant.Steps) : 1 / objectiveFunction.Evaluate(ant.Steps);
        }

        /// <summary>
        /// Is acceptable solution found?
        /// </summary>
        /// <param name="acceptableEvaluation">The acceptable evaluation (i.e. evaluation sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <returns>
        /// <c>True</c> if an acceptable solution is found, <c>false</c> otherwise.
        /// </returns>
        private bool IsAcceptableSolutionFound(double acceptableEvaluation)
        {
            return Objective == Objective.Minimize ? globalBestAnt.Evaluation <= acceptableEvaluation : 1 / globalBestAnt.Evaluation >= acceptableEvaluation;
        }
    }
}
