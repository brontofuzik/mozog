using System;
using Mozog.Utils;

namespace GeneticAlgorithm.Examples
{
    class Program
    {
        public static void Main(string[] args)
        {
            //RunKnapsack();
            RunTravellingSalesman();
        }

        private static void RunKnapsack()
        {
            Run("Knapsack problem (KP)",
                KnapsackProblem.Algorithm(maxGenerations: 20),
                populationSize: 20,
                crossoverRate: 0.80,
                mutationRate: 0.05);
        }

        private static void RunTravellingSalesman()
        {
            Run("Travelling salesman problem (TSP)",
                TravellingSalesmanProblem.Algorithm(maxGenerations: 1000),
                populationSize: 1000,
                crossoverRate: 0.80,
                mutationRate: 0.1);
        }

        private static void Run<TGene>(string testName, GeneticAlgorithm<TGene> geneticAlgorithm, int populationSize, double crossoverRate, double mutationRate)
        {
            Console.WriteLine($"{testName}:");

            geneticAlgorithm.Notify += (_, state) => Console.WriteLine($"{state.Generations}: {state.Evaluation}");

            var (result, elapsedTime) = Misc.MeasureTime(() => geneticAlgorithm.Run(populationSize, crossoverRate, mutationRate));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of generations: {result.Generations}");
            Console.WriteLine($"Best solution: {Chromosome<TGene>.Print(result.Solution)}");
            Console.WriteLine($"Best solution's evaluation: {result.Evaluation}");
        }
    }
}
