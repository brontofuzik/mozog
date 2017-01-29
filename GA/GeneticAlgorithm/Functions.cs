using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm 
{
    public static class Functions
    {
        public static InitializationFunction<TGene> PiecewiseInitialization<TGene>(Func<Parameters<TGene>, TGene> initialize) =>
            args =>
            {
                TGene[] genes = new TGene[args.ChromosomeSize];
                for (int i = 0; i < args.ChromosomeSize; i++)
                {
                    genes[i] = initialize(args);
                }
                return genes;
            };

        // The binary-coded one-point (1-PX) crossover function.
        // ---
        // Crossover Operators (S. Malkos)
        // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
        public static CrossoverOperator<TGene> SinglePointCrossover<TGene>() =>
            (offspring1, offspring2, args) =>
            {
                // Choose a point from itnerval [1, chromosomeSize) randomly.
                int point = Random.Int(1, args.ChromosomeSize);

                // Swap all genes from the point (including) to the end.
                Misc.SwapArrays(offspring1, point, offspring2, point, args.ChromosomeSize - point);
            };

        public static CrossoverOperator<TGene> TwoPointCrossover<TGene>() =>
            (offspring1, offspring2, args) =>
            {
                // Choose two points from interval [1, chromosomeSize) randomly.
                int point1 = Random.Int(1, args.ChromosomeSize - 1);
                int point2 = Random.Int(point1 + 1, args.ChromosomeSize);

                // Swap all genes from the point1 (including) to the point2 (excluding).
                Misc.SwapArrays(offspring1, point1, offspring2, point1, point2 - point1);
            };

        public static CrossoverOperator<TGene> UniformCrossover<TGene>() =>
            (offspring1, offspring2, args) =>
            {
                for (int i = 0; i < args.ChromosomeSize; i++)
                {
                    if (Random.Double() < 0.5)
                    {
                        Misc.Swap(ref offspring1[i], ref offspring2[i]);
                    }
                }
            };

        // The permutation-based partially matched crossover (PMX) function.
        // ---
        // Partially matched crossover (PMX) may be viewed as a crossover of permutations,
        // that guarantees that all positions are found exactly once in each offspring,
        // i.e. both offspring receive a full complement of genes, followed by the corresponding filling
        // in of alleles from their parents.
        // ---
        // Crossover Operators (S. Malkos)
        // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
        public static CrossoverOperator<TGene> PartiallyMatchedCrossover<TGene>() =>
            (offspring1, offspring2, args) =>
            {
                // 1. Select a substring uniformly in two parents at random.
                // Choose two points from interval [1, chromosomeSize) randomly.
                int point1 = Random.Int(1, args.ChromosomeSize - 1);
                int point2 = Random.Int(point1 + 1, args.ChromosomeSize);

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
            };

        public static MutationOperator<TGene> RandomPointMutation<TGene>(Func<TGene, Parameters<TGene>, TGene> mutate) =>
            (chromosome, args) =>
            {
                int index = Random.Int(0, args.ChromosomeSize);
                chromosome[index] = mutate(chromosome[index], args);
            };
    }

    public delegate TGene[] InitializationFunction<TGene>(Parameters<TGene> args);

    public delegate double ObjectiveFunc<TGene>(TGene[] chromosome);

    public delegate double FitnessFunction(double evaluation, double averageEvaluation, Objective objective);

    public delegate void CrossoverOperator<TGene>(TGene[] offspring1, TGene[] offspring2, Parameters<TGene> args);

    public delegate void MutationOperator<TGene>(TGene[] chromosome, Parameters<TGene> args);

    public delegate bool TerminationFunction<TGene>(int generation, double bestEvaluation, Parameters<TGene> args);
}