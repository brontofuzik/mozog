using System;
using System.Diagnostics;

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

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Result<TGene> result = geneticAlgorithm.Run(populationSize, crossoverRate, mutationRate);
            stopwatch.Stop();
        
            // Print the results.
            Console.WriteLine($"Duration: {stopwatch.Elapsed.TotalSeconds} s");
            Console.WriteLine($"Number of generations taken: {result.Generations}");
            Console.WriteLine($"Best solution: {Chromosome<TGene>.Print(result.Solution)}");
            Console.WriteLine($"Best solution's evaluation: {result.Evaluation}");
        }
    }
}
