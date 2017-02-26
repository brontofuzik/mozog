using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Functions.Crossover;
using GeneticAlgorithm.Functions.Fitness;
using GeneticAlgorithm.Functions.Initialization;
using GeneticAlgorithm.Functions.Mutation;
using GeneticAlgorithm.Functions.Termination;
using Mozog.Utils;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A knapsack problem (KP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    static class KnapsackProblem
    {
        private static readonly Knapsack Knapsack = new Knapsack()
            .AddItem(1.0, 1.0)
            .AddItem(1.0, 2.0)
            .AddItem(2.0, 2.0)
            .AddItem(12.0, 4.0)
            .AddItem(4.0, 10.0);

        public static GeneticAlgorithm<int> Algorithm => new GeneticAlgorithm<int>(Knapsack.ItemCount)
        {
            Fitness = FitnessFunction<int>.Maximize(chromosome =>
                Knapsack.TotalWeight(chromosome) <= 15.0 ? Knapsack.TotalValue(chromosome) : 0.0),
            Initializer = Initialization.Piecewise(() => Random.Int(0, 2)),
            Crossover = Crossover.SinglePoint<int>(),
            Mutator = Mutation.RandomPoint<int>(gene => gene == 0 ? 1 : 0),
            Terminator = Termination.MaxGenerationsOrMinEvaluation<int>(maxGenerations: 100, minEvaluation: double.MaxValue)
        };
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