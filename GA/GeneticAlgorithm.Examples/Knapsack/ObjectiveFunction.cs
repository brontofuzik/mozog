using System;
using System.Linq;

namespace GeneticAlgorithm.Examples.Knapsack
{
    class ObjectiveFunction : ObjectiveFunction<int>
    {
        private readonly Item[] items;

        public ObjectiveFunction()
            : base(5, Objective.Maximize)
        {
            items = new[]
            {
                new Item(1.0, 1.0),
                new Item(1.0, 2.0),
                new Item(2.0, 2.0),
                new Item(12.0, 4.0),
                new Item(4.0, 10.0)
            };
        }

        public override double Evaluate(int[] genes)
        {
            double totalWeight = genes.Select((g, i) => g == 1 ? items[i].Weight : 0.0).Sum();
            double totalValue = genes.Select((g, i) => g == 1 ? items[i].Value : 0.0).Sum();
            return totalWeight <= 15.0 ? totalValue : 0.0;
        }
    }

    class Item : Tuple<double, double>
    {
        public Item(double weight, double value)
            : base(weight, value) { }

        public double Weight => Item1;

        public double Value => Item2;
    }
}
