using GeneticAlgorithm.Selectors;

namespace GeneticAlgorithm
{
    /// <remarks>
    /// A Genetic Algorithm Tutorial - The Canonical Genetic Algorithm (Darell Whitley)
    /// samizdat.mines.edu/ga_tutorial/ga_tutorial.ps
    /// </remarks>
    /// <typeparam name="TGene">The type of the gene.</typeparam>
    public class GeneticAlgorithm<TGene>
    {
        // Params
        public InitializationFunction<TGene> InitializationFunction { get; set; }
        public ObjectiveFunction<TGene> ObjectiveFunction { get; set; }
        public FitnessFunction FitnessFunction { get; set; }
        public ISelector<TGene> Selector { get; set; }
        public CrossoverOperator<TGene> CrossoverOperator { get; set; }
        public MutationOperator<TGene> MutationOperator { get; set; }
        public TerminationFunction<TGene> TerminationFunction { get; set; }

        public int ChromosomeSize { get; private set; }
        public int PopulationSize { get; private set; }
        public double CrossoverRate { get; private set; }
        public double MutationRate { get; private set; }
        public bool Scaling { get; private set; }

        public double AcceptableEvaluation { get; private set; }
        public int MaxGenerations { get; private set; }

        /// <summary>
        /// The current population (pseudo-class).
        /// </summary>
        private Population<TGene> population;

        /// <summary>
        /// The global-best chromosome.
        /// </summary>
        private Chromosome<TGene> globalBestChromosome;

        public GeneticAlgorithm(int chromosomeSize)
        {
            ChromosomeSize = chromosomeSize;
            FitnessFunction = DefaultFitnessFunction;
            Selector = new RouletteWheelSelector<TGene>(); // Default
            TerminationFunction = DefaultTerminationFunction;
        }

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
        public Result<TGene> Run(int populationSize, double crossoverRate, double mutationRate, bool scaling, double acceptableEvaluation, int maxGenerations)
        {
            //
            // Arguments
            //
            PopulationSize = populationSize;
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;
            Scaling = scaling;
            AcceptableEvaluation = acceptableEvaluation;
            MaxGenerations = maxGenerations;
            
            // Algorithm

            population = Population<TGene>.CreateInitial(this);
            var generationChampion = population.EvaluateFitness();
            ObjectiveFunction.UpdateBestChromosome(ref globalBestChromosome, generationChampion);

            while (!TerminationFunction(population.Generation, globalBestChromosome.Evaluation, this))
            {
                var generationBestChromosome = population.EvaluateFitness();
                ObjectiveFunction.UpdateBestChromosome(ref globalBestChromosome, generationBestChromosome);

                population = population.BreedNewGeneration();
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
        private static double DefaultFitnessFunction(double evaluation, double averageEvaluation, Objective objective)
            => objective == Objective.Maximize ? evaluation / averageEvaluation : averageEvaluation / evaluation;

        private bool DefaultTerminationFunction(int generation, double bestEvaluation, GeneticAlgorithm<TGene> args)
            => ObjectiveFunction.IsAcceptable(bestEvaluation, AcceptableEvaluation) || generation >= MaxGenerations;
    }
}
