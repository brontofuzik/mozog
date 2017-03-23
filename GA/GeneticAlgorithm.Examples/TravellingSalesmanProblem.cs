using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Threading;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A travelling salesman problem (TSP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    static class TravellingSalesmanProblem
    {
        public static GeneticAlgorithm<char> Algorithm(int maxGenerations) =>
            new GeneticAlgorithm<char>(Map.CityCount).Configure(cfg =>
                cfg.Fitness.Minimize(chromosome => Map.TotalDistance(chromosome))
                .Initialization.Lambda(_ => StaticRandom.Shuffle(new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O' }))
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
                .Parallelizer(new TplParallelizer())
            );

        /// <summary>
        /// Dataset: https://people.sc.fsu.edu/~jburkardt/datasets/tsp/tsp.html (P01)
        /// Solution : [A, M, B, O, I, E, G, C, L, N, J, H, F, D, K]
        /// Evaluation: 291
        /// </summary>
        private static readonly Map Map = new Map()
            .From('A').To('B', 29).To('C', 82).To('D', 46).To('E', 68).To('F', 52).To('G', 72).To('H', 42).To('I', 51).To('J', 55).To('K', 29).To('L', 74).To('M', 23).To('N', 72).To('O', 46).End()
            .From('B').To('C', 55).To('D', 46).To('E', 42).To('F', 43).To('G', 43).To('H', 23).To('I', 23).To('J', 31).To('K', 41).To('L', 51).To('M', 11).To('N', 52).To('O', 21).End()
            .From('C').To('D', 68).To('E', 46).To('F', 55).To('G', 23).To('H', 43).To('I', 41).To('J', 29).To('K', 79).To('L', 21).To('M', 64).To('N', 31).To('O', 51).End()
            .From('D').To('E', 82).To('F', 15).To('G', 72).To('H', 31).To('I', 62).To('J', 42).To('K', 21).To('L', 51).To('M', 51).To('N', 43).To('O', 64).End()
            .From('E').To('F', 74).To('G', 23).To('H', 52).To('I', 21).To('J', 46).To('K', 82).To('L', 58).To('M', 46).To('N', 65).To('O', 23).End()
            .From('F').To('G', 61).To('H', 23).To('I', 55).To('J', 31).To('K', 33).To('L', 37).To('M', 51).To('N', 29).To('O', 59).End()
            .From('G').To('H', 42).To('I', 23).To('J', 31).To('K', 77).To('L', 37).To('M', 51).To('N', 46).To('O', 33).End()
            .From('H').To('I', 33).To('J', 15).To('K', 37).To('L', 33).To('M', 33).To('N', 31).To('O', 37).End()
            .From('I').To('J', 29).To('K', 62).To('L', 46).To('M', 29).To('N', 51).To('O', 11).End()
            .From('J').To('K', 51).To('L', 21).To('M', 41).To('N', 23).To('O', 37).End()
            .From('K').To('L', 65).To('M', 42).To('N', 59).To('O', 61).End()
            .From('L').To('M', 61).To('N', 11).To('O', 55).End()
            .From('M').To('N', 62).To('O', 23).End()
            .From('N').To('O', 59).End();
    }

    class Map
    {
        private readonly Dictionary<(char from, char to), double> roads = new Dictionary<(char from, char to), double>();

        public FromBuilder From(char from) => new FromBuilder(from, this);

        public Map AddRoad(char from, char to, double distance)
        {
            roads[(from, to)] = distance;
            return this;
        }

        public int CityCount => roads.Select(r => r.Key.from).Distinct().Count();

        public double TotalDistance(char[] genes)
        {
            double totalDistance = 0.0;
            for (int i = 0; i < genes.Length; i++)
            {
                char from = genes[i];
                char to = genes[(i + 1) % genes.Length];
                if (from > to) Misc.Swap(ref from, ref to);
                totalDistance += from != to ? roads[(from, to)] : 0.0;
            }
            return totalDistance;
        }

        internal class FromBuilder
        {
            private readonly char from;
            private readonly Map map;

            public FromBuilder(char from, Map map)
            {
                this.from = from;
                this.map = map;
            }

            public FromBuilder To(char to, double distance)
            {
                map.AddRoad(from, to, distance);
                return this;
            }

            public Map End() => map;
        }
    }
}
