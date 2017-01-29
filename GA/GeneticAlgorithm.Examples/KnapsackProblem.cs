using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A knapsack problem (KP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    static class KnapsackProblem
    {
        private static Knapsack Knapsack => new Knapsack()
            .AddItem(1.0, 1.0)
            .AddItem(1.0, 2.0)
            .AddItem(2.0, 2.0)
            .AddItem(12.0, 4.0)
            .AddItem(4.0, 10.0);

        public static GeneticAlgorithm<int> Algorithm => new GeneticAlgorithm<int>(Knapsack.ItemCount)
        {
            ObjectiveFunction = new KnapsackFitness(Knapsack),

            InitializationFunction = args =>
            {
                int[] genes = new int[args.ChromosomeSize];
                for (int i = 0; i < args.ChromosomeSize; i++)
                {
                    genes[i] = Random.Int(0, 2);
                }
                return genes;
            },

            // The binary-coded one-point (1-PX) crossover function.
            // ---
            // Crossover Operators (S. Malkos)
            // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
            CrossoverOperator = (offspring1, offspring2, args) =>
            {
                // Choose a point randomly.
                // The point must be located after the first and before the last gene; the point is from the interval [1, chromosomeSize). 
                int point = Random.Int(1, args.ChromosomeSize);

                // Crossover all genes from the point (including) to the end.
                for (int i = point; i < args.ChromosomeSize; i++)
                {
                    Misc.Swap(ref offspring1[i], ref offspring2[i]);
                }
            },

            MutationOperator = (offspring, args) =>
            {
                int index = Random.Int(0, args.ChromosomeSize);
                offspring[index] = offspring[index] == 0 ? 1 : 0;
            }
        };

        class KnapsackFitness : ObjectiveFunction<int>
        {
            private readonly Knapsack knapsack;

            public KnapsackFitness(Knapsack knapsack)
                : base(Objective.Maximize)
            {
                this.knapsack = knapsack;
            }

            public override double Evaluate(int[] genes)
                => knapsack.TotalWeight(genes) <= 15.0 ? knapsack.TotalValue(genes) : 0.0;
        }

        //private static void KnapsackUniformCrossoverFunction(int[] parent1Genes, int[] parent2Genes, out int[] offspring1Genes, out int[] offspring2Genes, double crossoverRate)
        //{
        //    int chromosomeSize = parent1Genes.Length;

        //    // Breed the first offspring from the first parent.
        //    offspring1Genes = new int[chromosomeSize];

        //    // Breed the second offspring from the second parent.
        //    offspring2Genes = new int[chromosomeSize];

        //    // Perform a uniform crossover.
        //    if (random.NextDouble() < crossoverRate)
        //    {
        //        for (int i = 0; i < chromosomeSize; i++)
        //        {
        //            if (random.NextDouble() < 0.5)
        //            {
        //                offspring1Genes[i] = parent1Genes[i];
        //                offspring2Genes[i] = parent2Genes[i];
        //            }
        //            else
        //            {
        //                offspring1Genes[i] = parent2Genes[i];
        //                offspring2Genes[i] = parent1Genes[i];
        //            }
        //        }
        //    }
        //}
    }

    class Knapsack
    {
        private readonly IList<(double weight, double value)> items = new List<(double weight, double value)>();

        public Knapsack AddItem(double weight, double value)
        {
            items.Add((weight, value));
            return this;
        }

        public int ItemCount => items.Count;

        public double TotalWeight(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].weight : 0.0).Sum();

        public double TotalValue(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].value : 0.0).Sum();
    }
}