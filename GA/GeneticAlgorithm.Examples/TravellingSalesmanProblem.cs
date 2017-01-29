using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Random = Mozog.Utils.Random;

namespace GeneticAlgorithm.Examples
{
    /// <summary>
    /// A travelling salesman problem (TSP) genetic algorithm.
    /// http://en.wikipedia.org/wiki/Travelling_salesman_problem
    /// </summary>
    static class TravellingSalesmanProblem
    {
        private static Map Map => new Map()
            .AddRoad('A', 'B', 20)
            .AddRoad('A', 'C', 42)
            .AddRoad('A', 'D', 35)
            .AddRoad('B', 'C', 30)
            .AddRoad('B', 'D', 34)
            .AddRoad('C', 'D', 12);

        public static GeneticAlgorithm<char> Algorithm => new GeneticAlgorithm<char>(Map.CityCount)
        {
            ObjectiveFunction = new TravellingSalesmanFitness(Map),

            InitializationFunction = args => Random.Shuffle(new[] {'A', 'B', 'C', 'D'}),

            // The permutation-based partially matched crossover (PMX) function.
            // ---
            // Partially matched crossover (PMX) may be viewed as a crossover of permutations,
            // that guarantees that all positions are found exactly once in each offspring,
            // i.e. both offspring receive a full complement of genes, followed by the corresponding filling
            // in of alleles from their parents.
            // ---
            // Crossover Operators (S. Malkos)
            // http://www3.itu.edu.tr/~okerol/CROSSOVER%20OPERATORS.pdf
            CrossoverOperator = (offspring1, offspring2, args) =>
            {
                // 1. Select a substring uniformly in two parents at random.
                // Choose two points randomly.
                // The points must be located after the first and before the last gene; the points are from the interval [1, chromosomeSize).
                int point1 = Random.Int(1, args.ChromosomeSize);
                int point2 = Random.Int(1, args.ChromosomeSize);
                if (point1 > point2)
                {
                    Misc.Swap(ref point1, ref point2);
                }

                // 2. Exchange these two substrings to produce proto-offspring.
                // Crossover all genes from the first point (including) to the second point (excluding).
                for (int i = point1; i < point2; i++)
                {
                    Misc.Swap(ref offspring1[i], ref offspring2[i]);
                }

                // 3. Determine the mapping relationship according to these two substrings.
                Dictionary<char, char> mapping = new Dictionary<char, char>();
                for (int i = point1; i < point2; i++)
                {
                    mapping[offspring1[i]] = offspring2[i];
                }

                // 4. Legalize proto-offspring with the mapping relationship.
                for (int i = 0; i < args.ChromosomeSize; i++)
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
                        for (int j = 0; j < args.ChromosomeSize; j++)
                        {
                            if (point1 <= j && j < point2) continue;

                            if (offspring2[j] == offspring1[i])
                            {
                                offspring2[j] = tmpGene;
                            }
                        }
                    }
                }
            },

            MutationOperator = (offspring, args) =>
            {
                // Swap two consecutive (tour-wisely) cities.
                int from = Random.Int(0, args.ChromosomeSize);
                int to = (from + 1) % args.ChromosomeSize;
                
                Misc.Swap(ref offspring[from], ref offspring[to]);
            }
        };

        class TravellingSalesmanFitness : ObjectiveFunction<char>
        {
            private readonly Map map;

            public TravellingSalesmanFitness(Map map)
                : base(Objective.Minimize)
            {
                this.map = map;
            }

            public override double Evaluate(char[] genes) => map.TotalDistance(genes);
        }
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
