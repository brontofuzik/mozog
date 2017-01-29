using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using static GeneticAlgorithm.Functions;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A travelling salesman problem (TSP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    static class TravellingSalesmanProblem
    {
        private static readonly Map Map = new Map()
            .AddRoad('A', 'B', 20)
            .AddRoad('A', 'C', 42)
            .AddRoad('A', 'D', 35)
            .AddRoad('B', 'C', 30)
            .AddRoad('B', 'D', 34)
            .AddRoad('C', 'D', 12);

        public static GeneticAlgorithm<char> Algorithm => new GeneticAlgorithm<char>(Map.CityCount,
            objective: ObjectiveFunction<char>.Minimize(
                chromosome => Map.TotalDistance(chromosome)),
            initialization: _ => Random.Shuffle(new[] {'A', 'B', 'C', 'D'}),
            crossover: PartiallyMatchedCrossover<char>(),
            mutation: (offspring, args) =>
            {
                // SwapArrays two consecutive (tour-wisely) cities.
                int from = Random.Int(0, args.ChromosomeSize);
                int to = (from + 1) % args.ChromosomeSize;
                
                Misc.Swap(ref offspring[from], ref offspring[to]);
            }
        );
    }

    class Map
    {
        private readonly Dictionary<(char from, char to), double> roads = new Dictionary<(char from, char to), double>();

        public Map AddRoad(char from, char to, double distance)
        {
            roads[(from, to)] = distance;
            return this;
        }

        public int CityCount => roads.Select(r => r.Key.from).Distinct().Count();

        public double TotalDistance(char[] genes)
        {
            double totalDistance = 0.0;
            for (int i = 0; i < 4; i++)
            {
                char from = genes[i];
                char to = genes[(i + 1) % genes.Length];
                totalDistance += from != to ? roads[((char)Math.Min(from, to), (char)Math.Max(from, to))] : 0;
            }
            return totalDistance;
        }
    }
}
