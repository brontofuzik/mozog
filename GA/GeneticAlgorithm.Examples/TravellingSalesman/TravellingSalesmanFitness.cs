using System;
using System.Collections.Generic;

namespace GeneticAlgorithm.Examples.TravellingSalesman
{
    internal class TravellingSalesmanFitness : ObjectiveFunction<char>
    {
        private readonly Map map;

        public TravellingSalesmanFitness()
            : base(4, Objective.Minimize)
        {
            map = new Map()
            .AddRoad('A', 'B', 20)
            .AddRoad('A', 'C', 42)
            .AddRoad('A', 'D', 35)
            .AddRoad('B', 'C', 30)
            .AddRoad('B', 'D', 34)
            .AddRoad('C', 'D', 12);
        }

        public override double Evaluate(char[] genes) => map.TotalDistance(genes);

        private class Map
        {
            private readonly Dictionary<(char from, char to), double> roads = new Dictionary<(char from, char to), double>();

            public Map AddRoad(char from, char to, double distance)
            {
                roads[(from, to)] = distance;
                return this;
            }

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
}