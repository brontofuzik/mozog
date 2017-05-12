using System.Collections.Generic;
using System.Linq;

namespace Mozog.Examples
{
    public static class KnapsackProblem
    {
        /// <summary>
        /// Dataset: https://people.sc.fsu.edu/~jburkardt/datasets/knapsack_01/knapsack_01.html (P01)
        /// Solution: [1, 1, 1, 1, 0, 1, 0, 0, 0, 0]
        /// Evaluation: 309
        /// </summary>
        public static readonly Knapsack Knapsack = new Knapsack(165.0)
            .AddItem(weight: 23.0, value: 92.0)
            .AddItem(weight: 31.0, value: 57.0)
            .AddItem(weight: 29.0, value: 49.0)
            .AddItem(weight: 44.0, value: 68.0)
            .AddItem(weight: 53.0, value: 60.0)
            .AddItem(weight: 38.0, value: 43.0)
            .AddItem(weight: 63.0, value: 67.0)
            .AddItem(weight: 85.0, value: 84.0)
            .AddItem(weight: 89.0, value: 87.0)
            .AddItem(weight: 82.0, value: 72.0);
    }

    public class Knapsack
    {
        private readonly double capacity;
        private readonly IList<(double weight, double value)> items = new List<(double weight, double value)>();

        public Knapsack(double capacity)
        {
            this.capacity = capacity;
        }

        public Knapsack AddItem(double weight, double value)
        {
            items.Add((weight, value));
            return this;
        }

        public int ItemCount => items.Count;

        public double Evaluate(int[] chromosome) => TotalWeight(chromosome) <= capacity ? TotalValue(chromosome) : 0.0;

        private double TotalWeight(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].weight : 0.0).Sum();

        private double TotalValue(int[] genes) => genes.Select((g, i) => g == 1 ? items[i].value : 0.0).Sum();
    }
}