using Mozog.Utils.Math;
using Mozog.Utils.Threading;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A knapsack problem (KP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Knapsack_problem
    /// </summary>
    static class KnapsackProblem
    {
        public static GeneticAlgorithm<int> Algorithm(int maxGenerations)
        {
            var knapsack = Mozog.Examples.KnapsackProblem.Knapsack;
            return new GeneticAlgorithm<int>(knapsack.ItemCount).Configure(cfg => cfg
                .Fitness.Maximize(chromosome => knapsack.Evaluate(chromosome))
                .Initialization.Piecewise(() => StaticRandom.Int(0, 2))
                .Selection.RouletteWheel()
                .Crossover.SinglePoint()
                .Mutation.RandomPoint(gene => gene == 0 ? 1 : 0)
                .Termination.MaxGenerations(maxGenerations)
                .Parallelizer(new TplParallelizer()));
        }
    }
}