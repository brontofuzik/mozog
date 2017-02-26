using System.Collections.Generic;
using Mozog.Utils;

namespace GeneticAlgorithm.Functions.Crossover
{
    // The permutation-based partially matched crossover (PMX) function.
    // ---
    // Partially matched crossover (PMX) may be viewed as a crossover of permutations,
    // that guarantees that all positions are found exactly once in each offspring,
    // i.e. both offspring receive a full complement of genes, followed by the corresponding filling
    // in of alleles from their parents.
    // ---
    // Crossover Operators (S. Malkos)
    // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
    public class PartiallyMatchedCrossover<TGene> : CrossoverOperator<TGene>
    {
        public override void CrossOver(TGene[] offspring1, TGene[] offspring2)
        {
            // 1. Select a substring uniformly in two parents at random.
            // Choose two points from interval [1, chromosomeSize) randomly.
            int point1 = Random.Int(1, offspring1.Length - 1);
            int point2 = Random.Int(point1 + 1, offspring1.Length);

            // 2. Exchange these two substrings to produce proto-offspring.
            // Swap all genes from the point1 (including) to the point2 (excluding).
            Misc.SwapArrays(offspring1, point1, offspring2, point1, point2 - point1);

            // 3. Determine the mapping relationship according to these two substrings.
            var mapping = new Dictionary<TGene, TGene>();
            offspring1.ForEachWithinRange(point1, point2, i =>
            {
                if (!Equals(offspring1[i], offspring2[i]))
                {
                    mapping[offspring1[i]] = offspring2[i];
                }
            });

            if (mapping.None()) return;

            // 4. Legalize proto-offspring with the mapping relationship.
            offspring1.ForEachOutsideRange(point1, point2, i =>
            {
                TGene tmpGene = offspring1[i];

                while (mapping.ContainsKey(offspring1[i]))
                {
                    offspring1[i] = mapping[offspring1[i]];
                }

                // There has been a swap.
                if (!Equals(tmpGene, offspring1[i]))
                {
                    offspring2.ForEachOutsideRange(point1, point2, j =>
                    {
                        if (Equals(offspring2[j], offspring1[i]))
                        {
                            offspring2[j] = tmpGene;
                        }
                    });
                }
            });
        }
    }
}