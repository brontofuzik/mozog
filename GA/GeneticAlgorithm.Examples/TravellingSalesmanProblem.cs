using Mozog.Utils;
using Mozog.Utils.Math;
using Mozog.Utils.Threading;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A travelling salesman problem (TSP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    static class TravellingSalesmanProblem
    {
        public static GeneticAlgorithm<char> Algorithm(int maxGenerations)
        {
            var map = Mozog.Examples.TravellingSalesmanProblem.Map;
            return new GeneticAlgorithm<char>(map.CityCount).Configure(cfg => cfg
                .Fitness.Minimize(chromosome => map.TotalDistance(chromosome))
                .Initialization.Lambda(_ => StaticRandom.Shuffle(new[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O'}))
                .Selection.RankBased()
                .Crossover.PartiallyMatched()
                .Mutation.Lambda(offspring =>
                {
                    // SwapArrays two consecutive (tour-wisely) cities.
                    int from = StaticRandom.Int(0, offspring.Length);
                    int to = (from + 1) % offspring.Length;

                    Misc.Swap(ref offspring[from], ref offspring[to]);
                })
                .Termination.MaxGenerations(maxGenerations)
                .Parallelizer(new TplParallelizer()));
        }
    }
}
