using System;
using GeneticAlgorithm.Configuration;
using GeneticAlgorithm.Functions.Crossover;
using GeneticAlgorithm.Functions.Fitness;
using GeneticAlgorithm.Functions.Initialization;
using GeneticAlgorithm.Functions.Mutation;
using GeneticAlgorithm.Functions.Selection;
using GeneticAlgorithm.Functions.Termination;
using Mozog.Utils.Threading;

namespace GeneticAlgorithm
{
    /// <remarks>
    /// A Genetic Algorithm Tutorial - The Canonical Genetic Algorithm (Darell Whitley)
    /// samizdat.mines.edu/ga_tutorial/ga_tutorial.ps
    /// </remarks>
    /// <typeparam name="TGene">The type of the gene.</typeparam>
    public class GeneticAlgorithm<TGene>
    {
        private Population<TGene> population;
        private Chromosome<TGene> champion;

        public GeneticAlgorithm(int chromosomeSize)
        {
            ChromosomeSize = chromosomeSize;
        }

        public int ChromosomeSize { get; }

        #region Functions & operators

        private IFitnessFunction<TGene> fitness;
        public IFitnessFunction<TGene> Fitness
        {
            get { return fitness; }
            set
            {
                fitness = value;
                fitness.Algo = this;
            }
        }

        private IInitializationFunction<TGene> initializer;
        public IInitializationFunction<TGene> Initializer
        {
            get { return initializer; }
            set
            {
                initializer = value;
                initializer.Algo = this;
            }
        }

        private ITerminationFunction<TGene> terminator;
        public ITerminationFunction<TGene> Terminator
        {
            get { return terminator; }
            set
            {
                terminator = value;
                terminator.Algo = this;
            }
        }

        private ISelectionFunction<TGene> selector;
        public ISelectionFunction<TGene> Selector
        {
            get { return selector; }
            set
            {
                selector = value;
                selector.Algo = this;
            }
        }

        private ICrossoverOperator<TGene> crossover;
        public ICrossoverOperator<TGene> Crossover
        {
            get { return crossover; }
            set
            {
                crossover = value;
                crossover.Algo = this;
            }
        }

        private IMutationOperator<TGene> mutator;
        public IMutationOperator<TGene> Mutator
        {
            get { return mutator; }
            set
            {
                mutator = value;
                mutator.Algo = this;
            }
        }

        public IParallelizer Parallelizer { get; set; }

        public double CrossoverRate { get; set; }

        public double MutationRate { get; set; }

        #endregion Functions & operators

        #region State

        public int CurrentGeneration => population.Generation;

        public TGene[] BestGenes => champion.Genes;

        public double BestEvaluation => champion.Evaluation;

        #endregion State

        public event EventHandler<Result<TGene>> Notify;

        public GeneticAlgorithm<TGene> Configure(Action<AlgorithmConfigurer<TGene>> configure)
        {
            var configurer = new AlgorithmConfigurer<TGene>(this);
            configure(configurer);
            return this;
        }

        /// <summary>
        /// Runs the genetic algorithm.
        /// </summary>
        /// <param name="populationSize">The size of the population (i.e. the number of chromosomes in the population).</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        /// <returns>
        /// The best solution (the global-best chromosome).
        /// </returns>
        public Result<TGene> Run(int populationSize, double crossoverRate, double mutationRate)
        {
            CrossoverRate = crossoverRate;
            MutationRate = mutationRate;

            do
            {
                population = population?.BreedNewGeneration() ?? Population<TGene>.CreateInitial(this, populationSize);

                var generationChampion = population.EvaluateFitness();
                Fitness.UpdateChampion(generationChampion, ref champion);

                Notify?.Invoke(this, GetStateSnapshot());
            }
            while (!Terminator.ShouldTerminate());

            return GetStateSnapshot();
        }

        private Result<TGene> GetStateSnapshot() => new Result<TGene>
        {
            Solution = BestGenes,
            Evaluation = BestEvaluation,
            Generations = CurrentGeneration
        };
    }
}