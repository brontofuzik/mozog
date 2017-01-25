using System;
using System.Collections.Generic;
using System.Linq;

namespace GeneticAlgorithm.Examples.Knapsack
{
    class KnapsackFitness : ObjectiveFunction<int>
    {
        private readonly Knapsack knapsack;

        public KnapsackFitness()
            : base(5, Objective.Maximize)
        {
            knapsack = new Knapsack()
            .AddItem(1.0, 1.0)
            .AddItem(1.0, 2.0)
            .AddItem(2.0, 2.0)
            .AddItem(12.0, 4.0)
            .AddItem(4.0, 10.0);
        }

        public override double Evaluate(int[] genes)
            => knapsack.TotalWeight(genes) <= 15.0 ? knapsack.TotalValue(genes) : 0.0;
    }

    class Knapsack
    {
        private readonly IList<(double weight, double value)> items = new List<(double weight, double value)>();

        public Knapsack AddItem(double weight, double value)
        {
            items.Add((weight, value));
            return this;
        }

        public double TotalWeight(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].weight : 0.0).Sum();

        public double TotalValue(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].value : 0.0).Sum();
    }
}
