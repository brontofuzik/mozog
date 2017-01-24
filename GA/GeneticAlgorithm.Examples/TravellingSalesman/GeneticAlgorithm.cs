using System.Collections.Generic;
using Mozog.Utils;

namespace GeneticAlgorithm.Examples.TravellingSalesman
{
    /// <summary>
    /// A travelling salesman problem (TSP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    class GeneticAlgorithm : GeneticAlgorithm<char>
    {
        /// <summary>
        /// <para>
        /// The initialization function.
        /// </para>
        /// </summary>
        /// <param name="chromosome">The chromosome to initialize.</param>
        protected override Chromosome<char> InitializationFunction()
        {
            return new Chromosome<char>(Dimension)
            {
                Genes = Random.Shuffle(new[] {'A', 'B', 'C', 'D'})
            };
        }

        /// <summary>
        /// <para>
        /// The permutation-based partially matched crossover (PMX) function.
        /// </para>
        /// <para>
        /// Crossovers the chromosome with some other chromosome to create (two) offsprings with some probability p_c.
        /// </para>
        /// <para>
        /// Partially matched crossover (PMX) may be viewed as a crossover of permutations,
        /// that guarantees that all positions are found exactly once in each offspring,
        /// i.e. both offspring receive a full complement of genes, followed by the corresponding filling
        /// in of alleles from their parents.
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
        protected override void CrossoverFunction(Chromosome<char> parent1, Chromosome<char> parent2, out Chromosome<char> offspring1, out Chromosome<char> offspring2, double crossoverRate)
        {
            offspring1 = parent1.Clone();
            offspring2 = parent2.Clone();

            if (Random.Double() < crossoverRate)
            {
                // 1. Select a substring uniformly in two parents at random.
                // Choose two points randomly.
                // The points must be located after the first and before the last gene; the points are from the interval [1, chromosomeSize).
                int point1 = Random.Int(1, Dimension);
                int point2 = Random.Int(1, Dimension);
                if (point1 > point2)
                {
                    Misc.Swap(ref point1, ref point2);
                }

                // 2. Exchange these two substrings to produce proto-offspring.
                // Crossover all genes from the first point (including) to the second point (excluding).
                for (int i = point1; i < point2; i++)
                {
                    Misc.Swap(ref offspring1.Genes[i], ref offspring2.Genes[i]);
                }

                // 3. Determine the mapping relationship according to these two substrings.
                Dictionary<char, char> mapping = new Dictionary<char, char>();
                for (int i = point1; i < point2; i++)
                {
                    mapping[offspring1[i]] = offspring2[i];
                }

                // 4. Legalize proto-offspring with the mapping relationship.
                for (int i = 0; i < Dimension; i++)
                {
                    if (point1 <= i && i < point2) continue;

                    char tmpGene = offspring1[i];
                    while (mapping.ContainsKey(offspring1[i]))
                    {
                        offspring1[i] = mapping[offspring1[i]];
                    }
                    
                    // There has been a swap.
                    if (tmpGene != offspring1[i])
                    {
                        for (int j = 0; j < Dimension; j++)
                        {
                            if (point1 <= j && j < point2) continue;

                            if (offspring2[j] == offspring1[i])
                            {
                                offspring2[j] = tmpGene;
                            }
                        }
                    }
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
        protected override void MutationFunction(Chromosome<char> chromosome, double mutationRate)
        {
            if (Random.Double() < mutationRate)
            {
                // Swap two consecutive (tour-wisely) cities.
                int from = Random.Int(0, Dimension);
                int to = (from + 1) % Dimension;
                
                Misc.Swap(ref chromosome.Genes[from], ref chromosome.Genes[to]);
            }
        }
    }
}
