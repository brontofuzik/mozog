using System;
using System.Collections.Specialized;
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
        public Parameters<TGene> args = new Parameters<TGene>(); 

        /// <summary>
        /// The current population (pseudo-class).
        /// </summary>
        private Population<TGene> population;

        /// <summary>
        /// The global-best chromosome.
        /// </summary>
        private Chromosome<TGene> globalChampion;

        public GeneticAlgorithm(int chromosomeSize, InitializationFunction<TGene> initialization = null,
            ObjectiveFunction<TGene> objective = null, FitnessFunction fitness = null,
            ISelector<TGene> selector = null,
            CrossoverOperator<TGene> crossover = null, MutationOperator<TGene> mutation = null,
            TerminationFunction<TGene> termination = null)
        {
            args.InitializationFunction = initialization;
            args.ObjectiveFunction = objective;
            args.FitnessFunction = fitness ?? DefaultFitnessFunction;
            args.Selector = selector ?? new RouletteWheelSelector<TGene>();
            args.CrossoverOperator = crossover;
            args.MutationOperator = mutation;
            args.TerminationFunction = termination ?? DefaultTerminationFunction;

            args.ChromosomeSize = chromosomeSize;
        }

        public Action<int, double> Notify;

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
            args.PopulationSize = populationSize;
            args.CrossoverRate = crossoverRate;
            args.MutationRate = mutationRate;
            args.Scaling = scaling;
            args.AcceptableEvaluation = acceptableEvaluation;
            args.MaxGenerations = maxGenerations;
            
            // Algorithm

            do
            {
                population = population?.BreedNewGeneration() ?? Population<TGene>.CreateInitial(args);

                var generationChampion = population.EvaluateFitness();
                args.ObjectiveFunction.UpdateBestChromosome(ref globalChampion, generationChampion);

                Notify(population.Generation, globalChampion.Evaluation);
            }
            while (!args.TerminationFunction(population.Generation, globalChampion.Evaluation, args));

            // Return value

            return new Result<TGene>
            {
                Solution = globalChampion.Genes,
                Evaluation = globalChampion.Evaluation,
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

        private bool DefaultTerminationFunction(int generation, double bestEvaluation, Parameters<TGene> args)
            => args.ObjectiveFunction.IsAcceptable(bestEvaluation, args.AcceptableEvaluation) || generation >= args.MaxGenerations;
    }
}
