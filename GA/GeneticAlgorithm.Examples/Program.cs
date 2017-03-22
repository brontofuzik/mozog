using System;
using System.Diagnostics;

namespace GeneticAlgorithm.Examples
{
    /// <remarks>
    /// The genetic algorithm test suite.
    /// </remarks>
    internal class Program
    {
        /// <summary>
        /// The test harness.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            RunKnapsack();
            //RunTravellingSalesman();
        }

        /// <summary>
        /// Solution: [1, 1, 1, 1, 0, 1, 0, 0, 0, 0]
        /// Evaluation: ?
        /// </summary>
        private static void RunKnapsack()
        {
            Run("Knapsack problem (KP)",
                KnapsackProblem.Algorithm(maxGenerations: 20),
                populationSize: 20,
                crossoverRate: 0.80,
                mutationRate: 0.05);
        }

        /// <summary>
        /// Solution : [A, B, C, D]
        /// Evaluation: 97
        /// </summary>
        private static void RunTravellingSalesman()
        {
            Run("Travelling salesman problem (TSP)",
                TravellingSalesmanProblem.Algorithm(maxGenerations: 10),
                populationSize: 20,
                crossoverRate: 0.80,
                mutationRate: 0.05);
        }

        /// <summary>
        /// Tests a genetic algorithm.
        /// </summary>
        /// <typeparam name="TGene">The type of the gene.</typeparam>
        /// <param name="testName">The description of the test.</param>
        /// <param name="geneticAlgorithm">The genetic algorithm to test.</param>
        /// <param name="populationSize">The size of the population.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        /// <param name="maxGenerationCount">The maximum number of generations (the computational budget).</param>
        /// <param name="acceptableEvaluation">The acceptable evaluation.</param>
        private static void Run<TGene>(string testName, GeneticAlgorithm<TGene> geneticAlgorithm, int populationSize, double crossoverRate, double mutationRate)
        {
            Console.WriteLine($"{testName}:");

            geneticAlgorithm.Notify += (_, state) => Console.WriteLine($"{state.Generations}: {state.Evaluation}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Result<TGene> result = geneticAlgorithm.Run(populationSize, crossoverRate, mutationRate);
            stopwatch.Stop();
        
            // Print the results.
            Console.WriteLine($"Duration: {stopwatch.Elapsed.Milliseconds} ms");
            Console.WriteLine($"Number of generations taken: {result.Generations}");
            Console.WriteLine($"Best solution: {Chromosome<TGene>.Print(result.Solution)}");
            Console.WriteLine($"Best solution's evaluation: {result.Evaluation}");
        }
    }
}
