using System.Collections.Generic;

namespace GeneticAlgorithm.Examples.TravellingSalesman
{
    internal class TravellingSalesmanFitness : ObjectiveFunction<char>
    {
        private readonly Dictionary<(char from, char to), double> distances;

        public TravellingSalesmanFitness()
            : base(4, Objective.Minimize)
        {
            distances = new Dictionary<(char, char), double>
            {
                {('A', 'A'), 0.0},
                {('A', 'B'), 20.0},
                {('A', 'C'), 42},
                {('A', 'D'), 35.0},

                {('B', 'B'), 0.0},
                {('B', 'C'), 30.0},
                {('B', 'D'), 34.0},

                {('C', 'C'), 0.0},
                {('C', 'D'), 12.0},

                {('D', 'D'), 0.0}
            };
        }

        public override double Evaluate(char[] genes)
        {
            double totalDistance = 0.0;
            for (int i = 0; i < 4; i++)
            {
                char from = genes[i];
                char to = genes[(i + 1) % Arity];
                totalDistance += distances[(from, to)];
            }
            return totalDistance;
        }
    }
}