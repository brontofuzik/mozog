using System;
using System.Collections.Generic;

namespace GeneticAlgorithm
{
    /// <summary>
    /// An interface of a chromosome.
    /// </summary>
    public interface IChromosome< TGene >
        : IComparable< IChromosome< TGene > >
    {
        #region Properties

        /// <summary>
        /// Gets the genes of the chromosome.
        /// </summary>
        /// <value>
        /// The genes of the chromosome.
        /// </value>
        TGene[] Genes
        {
            get;
        }

        /// <summary>
        /// The size of the chromosome.
        /// </summary>
        /// <value>
        /// The size of the chromosome.
        /// </value>
        int Size
        {
            get;
        }

        /// <summary>
        /// Gets or sets the evaluation of the chromosome.
        /// </summary>
        /// <value>
        /// The evaluation of the chromosome.
        /// </value>
        double Evaluation
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the fitness of the chromosome.
        /// </summary>
        /// <value>
        /// The fitness of the chromosome.
        /// </value>
        double Fitness
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Initializes the chromosome.
        /// </summary>
        void Initialize();

        /// <summary>
        /// <para>
        /// The evaluation function.
        /// </para>
        /// <para>
        /// The <i>evaluation function</i>, or <i>objective function</i>, provides a measure of performance
        /// with respect to a particular set of parameters.
        /// </para>
        /// <para>
        /// The evaluation of a string representing a set of parameters is independent of the evaluation of any other string.
        /// </para>
        /// </summary>
        void Evaluate();

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
        /// </summary>
        /// <param name="averageEvaluation">The average evaluation of all chromosomes in the (current) population.</param>
        void FitnessFunction( double averageEvaluation );

        /// <summary>
        /// <para>
        /// The crossover operator.
        /// </para>
        /// <para>
        /// Crosses the chromosome over with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// </summary>
        /// <param name="parent2">The second parent.</param>
        /// <param name="offspring1">The first offpsring.</param>
        /// <param name="offspring2">The second offspring.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        void Crossover( IChromosome< TGene > parent2, out IChromosome< TGene > offspring1, out IChromosome< TGene > offspring2, double crossoverRate );

        /// <summary>
        /// <para>
        /// The mutation operator.
        /// </para>
        /// <para>
        /// Mutates the chromosome.
        /// </para>
        /// </summary>
        /// <param name="mutationRate">The rate of mutation.</param>
        void Mutate( double mutationRate );
         
        #endregion // Methods
    }
}
