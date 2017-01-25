using System;

namespace GeneticAlgorithm
{
    /// <remarks>
    /// A Genetic Algorithm Tutorial - The Canonical Genetic Algorithm (Darell Whitley)
    /// samizdat.mines.edu/ga_tutorial/ga_tutorial.ps
    /// </remarks>
    /// <typeparam name="TGene">The type of the gene.</typeparam>
    public abstract class GeneticAlgorithm<TGene>
    {
        /// <summary>
        /// The objective function.
        /// </summary>
        private ObjectiveFunction<TGene> objectiveFunction;

        /// <summary>
        /// The current population (pseudo-class).
        /// </summary>
        private Population<TGene> population;

        /// <summary>
        /// The intermediate population (pseudo-class).
        /// </summary>
        private Population<TGene> intermediatePopulation;

        /// <summary>
        /// The next population (pseudo-class).
        /// </summary>
        private Population<TGene> nextPopulation;

        /// <summary>
        /// The global-best chromosome.
        /// </summary>
        private Chromosome<TGene> globalBestChromosome;

        /// <summary>
        /// Useful when the objective function is to be minimized.
        /// </summary>
        //private double maxObjectiveFunctionValue;

        /// <summary>
        /// Gets the dimension.
        /// </summary>
        /// <value>
        /// The dimension.
        /// </value>
        public int Dimension => objectiveFunction.Arity;

        /// <summary>
        /// Gets the objective (minimize or maximize).
        /// </summary>
        /// <value>
        /// The objective (minimize or mazimize).
        /// </value>
        public Objective Objective => objectiveFunction.Objective;

        /// <summary>
        /// Runs the genetic algorithm.
        /// </summary>
        /// <param name="objectiveFunction">The objective function.</param>
        /// 
        /// <param name="maxGenerations">The maximum number of generations.</param>
        /// <param name="usedGenerationCount">The number of generation taken.</param>
        /// <param name="acceptableEvaluation">The acceptable evaluation (i.e. evaluation sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <param name="achievedEvalaution">The achieved evalaution.</param>
        ///
        /// <param name="populationSize">The size of the population (i.e. the number of chromosomes in the population).</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        /// <param name="scaling">The scaling flag.</param>
        /// <returns>
        /// The best solution (the global-best chromosome).
        /// </returns>
        public Result<TGene> Run(ObjectiveFunction<TGene> objectiveFunction, int maxGenerations, double acceptableEvaluation,
            int populationSize, double crossoverRate, double mutationRate, bool scaling = false)
        {
            //
            // Arguments
            //

            if (objectiveFunction == null)
            {
                throw new ArgumentNullException(nameof(objectiveFunction));
            }
            this.objectiveFunction = objectiveFunction;

            ISelector<TGene> selector = new RouletteWheelSelector<TGene>();

            // Algorithm

            population = Population<TGene>.CreateRandom(populationSize, InitializationFunction);
            var generationChampion = population.Evaluate(objectiveFunction, Objective, scaling);
            objectiveFunction.UpdateBestChromosome(ref globalBestChromosome, generationChampion);

            while (!ShouldTerminate(maxGenerations, acceptableEvaluation))
            {
                var generationBestChromosome = population.Evaluate(objectiveFunction, Objective, scaling);
                objectiveFunction.UpdateBestChromosome(ref globalBestChromosome, generationBestChromosome);
                population.EvaluateFitness(FitnessFunction, Objective);

                population = population.BreedNewGeneration(selector, CrossoverFunction, crossoverRate, MutationFunction, mutationRate);
            }

            // Return value

            return new Result<TGene>
            {
                Solution = globalBestChromosome.Genes,
                Evaluation = globalBestChromosome.Evaluation,
                Generations = population.Generation
            };
        }

        /// <summary>
        /// <para>
        /// The generator function.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <returns>
        /// A random chromosome.
        /// </returns>
        protected abstract Chromosome<TGene> InitializationFunction();

        /// <summary>
        /// <para>
        /// The fitness function.
        /// </para>
        /// <para>
        /// The <c>fitness function</c> transforms that measure (measure provided by the <i>evaluation function</i>)
        /// into an allocation of reproductiove opportunities.
        /// </para>
        /// <para>
        /// The fitness of that string, however, is always defined with respect to other members of the current population.
        /// </para>
        /// <para>
        /// In the caconical genetic algorithm, fitness is defined by: f_i / f_avg where f_i is the evaluation associated with string i
        /// and f_avg is the average evaluation of all the strings in the population. Fitness can be assigned based on a string's rank
        /// in the population or by sampling methods, such as tournament selection.
        /// </para>
        /// <para>
        /// Can be overriden.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to be ranked.</param>
        /// <param name="averageEvaluation">The average evaluation of all chromosomes in the (current) population.</param>
        protected virtual double FitnessFunction(double evaluation, double averageEvaluation, Objective objective)
            => objective == Objective.Maximize ? evaluation / averageEvaluation : averageEvaluation / evaluation;

        /// <summary>
        /// <para>
        /// The crossover function.
        /// </para>
        /// <para>
        /// Crossovers the chromosome with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <param name="parent1">The first parent to crossover.</param>
        /// <param name="parent2">The second parent to crossover.</param>
        /// <param name="offspring1">The first offpsring.</param>
        /// <param name="offspring2">The second offspring.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        protected abstract void CrossoverFunction(Chromosome<TGene> parent1, Chromosome<TGene> parent2,
            out Chromosome<TGene> offspring1, out Chromosome<TGene> offspring2, double crossoverRate);

        /// <summary>
        /// <para>
        /// The mutation function.
        /// </para>
        /// <para>
        /// Mutates the chromosome with some probability p_m.
        /// </para>
        /// <para>
        /// Must be overriden.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to mutate.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        protected abstract void MutationFunction(Chromosome<TGene> chromosome, double mutationRate);

        protected virtual bool ShouldTerminate(int maxGenerations, double acceptableEvaluation)
            => IsAcceptableSolutionFound(acceptableEvaluation) || IsComputingBudgetSpent(maxGenerations);

        /// <summary>
        /// Is acceptable solution found?
        /// </summary>
        /// <param name="acceptableEvaluation">The acceptable evaluation (i.e. evaluation sufficiently low (when minimizing) or sufficiently high (when maximizing)).</param>
        /// <returns>
        /// <c>True</c> if an acceptable solution is found, <c>false</c> otherwise.
        /// </returns>
        private bool IsAcceptableSolutionFound(double acceptableEvaluation) => objectiveFunction.IsAcceptable(globalBestChromosome.Evaluation, acceptableEvaluation);

        private bool IsComputingBudgetSpent(int maxGenerations) => population.Generation >= maxGenerations;
    }

    public delegate Chromosome<TGene> InitializationFunction<TGene>();

    public delegate double FitnessFunction(double evaluation, double averageEvaluation, Objective objective);

    public delegate void CrossoverFunction<TGene>(Chromosome<TGene> parent1, Chromosome<TGene> parent2,
        out Chromosome<TGene> offspring1, out Chromosome<TGene> offspring2, double crossoverRate);

    public delegate void MutationFunction<TGene>(Chromosome<TGene> chromosome, double mutationRate);
}
