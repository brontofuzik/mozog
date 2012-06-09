using System;

using GeneticAlgorithm;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <summary>
    /// A genetic algorithm designed to train a neural network.
    /// </summary>
    internal class NetworkGeneticAlgorithm
        : GeneticAlgorithm< double >
    {
        #region Protected instance methods

        /// <summary>
        /// The generator function.
        /// </summary>
        /// <returns>
        /// A random chromosome.
        /// </returns>
        protected override Chromosome< double > GeneratorFunction()
        {
            Chromosome< double > chromosome = new Chromosome< double >( Dimension );
            for (int i = 0; i < Dimension; i++)
            {
                // TODO: ???
                chromosome.Genes[ i ] = random.NextDouble() + random.Next( -10, +10 );
            }
            return chromosome;
        }

        /// <summary>
        /// <para>
        /// The (1-point) crossover function.
        /// </para>
        /// <para>
        /// Crossovers the chromosome with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// </summary>
        /// <param name="parent1">The first parent to crossover.</param>
        /// <param name="parent2">The second parent to crossover.</param>
        /// <param name="offspring1">The first offpsring.</param>
        /// <param name="offspring2">The second offspring.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        protected override void CrossoverFunction(Chromosome<double> parent1, Chromosome<double> parent2, out Chromosome<double> offspring1, out Chromosome<double> offspring2, double crossoverRate)
        {
            // Breed the first offspring from the first parent.
            offspring1 = parent1.Clone();

            // Breed the second offspring from the second parent.
            offspring2 = parent2.Clone();

            // Perform a 1-point crossover.
            if (random.NextDouble() < crossoverRate)
            {
                // Choose a point randomly.
                // The point must be located after the first and before the last gene; the point is from the interval [1, chromosomeSize). 
                int point = random.Next( 1, Dimension );

                // Crossover all genes from point (including) to the end.
                // parent1:    [x1_0, x1_1, ..., x1_point-1, x1_point, ..., x1_size-1]
                // parent2:    [x2_0, x2_1, ..., x2_point-1, x2_point, ..., x2_size-1]
                // offspring1: [x1_0, x1_1, ..., x1_point-1, x2_point, ..., x2_size-1]
                // offspring2: [x2_0, x2_1, ..., x2_point-1, x1_point, ..., x1_size-1]

                int tmpGenesSize = Dimension - point;
                double[] tmpGenes = new double[ tmpGenesSize ];

                // "tmpGenes = offspring1Genes"
                Array.Copy( offspring1.Genes, point, tmpGenes, 0, tmpGenesSize );
                // "offspring1Genes = offspring2Genes"
                Array.Copy( offspring2.Genes, point, offspring1.Genes, point, tmpGenesSize );
                // "offspring2Genes = tmpGenes"
                Array.Copy( tmpGenes, 0, offspring2.Genes, point, tmpGenesSize );
            }
        }

        /// <summary>
        /// <para>
        /// The mutation function.
        /// </para>
        /// <para>
        /// Mutates the chromosome with some probability p_m.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to mutate.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        protected override void MutationFunction(Chromosome<double> chromosome, double mutationRate)
        {
            for (int i = 0; i < Dimension; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    // TODO: ???
                    chromosome.Genes[ i ] = (chromosome.Genes[ i ] + (random.NextDouble() + random.Next( -10, +10 ))) / 2.0;
                }
            }
        }

        #endregion // Protected istance methods
    }
}
