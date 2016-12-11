namespace GeneticAlgorithm.Examples.Problems
{
    /// <summary>
    /// A knapsack problem (KP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    internal class KnapsackProblemGeneticAlgorithm
        : GeneticAlgorithm< int >
    {
        /// <summary>
        /// <para>
        /// The initialization function.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to initialize.</param>
        protected override Chromosome< int > GeneratorFunction()
        {
            Chromosome< int > chromosome = new Chromosome< int >( Dimension );
            for (int i = 0; i < Dimension; i++)
            {
                chromosome.Genes[ i ] = random.Next( 0, 2 );
            }
            return chromosome;
        }

        /// <summary>
        /// <para>
        /// The binary-coded one-point (1-PX) crossover function.
        /// </para>
        /// <para>
        /// Crossovers the chromosome with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// <para>
        /// Crossover Operators (Sibel Malkos)
        /// http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
        /// </para>
        /// </summary>
        /// <param name="parent1">The first parent to crossover.</param>
        /// <param name="parent2">The second parent to crossover.</param>
        /// <param name="offspring1">The first offpsring.</param>
        /// <param name="offspring2">The second offspring.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        protected override void CrossoverFunction( Chromosome< int > parent1, Chromosome< int > parent2, out Chromosome< int > offspring1, out Chromosome< int > offspring2, double crossoverRate )
        {
            // Breed the first offspring from the first parent.
            offspring1 = parent1.Clone();

            // Breed the second offspring from the second parent.
            offspring2 = parent2.Clone();

            // Perform a binary-coded one-point (1-PX) crossover.
            if (random.NextDouble() < crossoverRate)
            {
                // Choose a point randomly.
                // The point must be located after the first and before the last gene; the point is from the interval [1, chromosomeSize). 
                int point = random.Next( 1, Dimension );

                // Crossover all genes from the point (including) to the end.
                for (int i = point; i < Dimension; i++)
                {
                    int tmpGene = offspring1.Genes[ i ];
                    offspring1.Genes[ i ] = offspring2.Genes[ i ];
                    offspring2.Genes[ i ] = tmpGene;
                }
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
        protected override void MutationFunction( Chromosome< int > chromosome, double mutationRate )
        {
            for (int i = 0; i < chromosome.Size; i++)
            {
                if (random.NextDouble() < mutationRate)
                {
                    chromosome.Genes[ i ] = (chromosome.Genes[ i ] == 0) ? 1 : 0;
                }
            }
        }

        //private static void KnapsackUniformCrossoverFunction( int[] parent1Genes, int[] parent2Genes, out int[] offspring1Genes, out int[] offspring2Genes, double crossoverRate )
        //{
        //    int chromosomeSize = parent1Genes.Length;

        //    // Breed the first offspring from the first parent.
        //    offspring1Genes = new int[ chromosomeSize ];

        //    // Breed the second offspring from the second parent.
        //    offspring2Genes = new int[ chromosomeSize ];

        //    // Perform a uniform crossover.
        //    if (random.NextDouble() < crossoverRate)
        //    {
        //        for (int i = 0; i < chromosomeSize; i++)
        //        {
        //            if (random.NextDouble() < 0.5)
        //            {
        //                offspring1Genes[ i ] = parent1Genes[ i ];
        //                offspring2Genes[ i ] = parent2Genes[ i ];
        //            }
        //            else
        //            {
        //                offspring1Genes[ i ] = parent2Genes[ i ];
        //                offspring2Genes[ i ] = parent1Genes[ i ];
        //            }
        //        }
        //    }
        //}
    }
}