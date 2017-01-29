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
        /// Solution: [1, 1, 1, 0, 1]
        /// Evaluation: 15
        /// </summary>
        private static void RunKnapsack()
        {
            Run(1, "Knapsack problem (KP)",
                KnapsackProblem.Algorithm,
                populationSize: 10,
                crossoverRate: 0.80,
                mutationRate: 0.05,
                acceptableEvaluation: double.MaxValue,
                maxGenerations: 1000);
        }

        /// <summary>
        /// Solution : [A, B, C, D]
        /// Evaluation: 97
        /// </summary>
        private static void RunTravellingSalesman()
        {
            Run(2, "Travelling salesman problem (TSP)",
                TravellingSalesmanProblem.Algorithm,
                populationSize: 100,
                crossoverRate: 0.80,
                mutationRate: 0.05,
                acceptableEvaluation: double.MinValue,
                maxGenerations: 1000);
        }

        /// <summary>
        /// Tests a genetic algorithm.
        /// </summary>
        /// <typeparam name="TGene">The type of the gene.</typeparam>
        /// <param name="testNumber">The number of the test.</param>
        /// <param name="testDescription">The description of the test.</param>
        /// <param name="geneticAlgorithm">The genetic algorithm to test.</param>
        /// <param name="maxGenerationCount">The maximum number of generations (the computational budget).</param>
        /// <param name="acceptableEvaluation">The acceptable evaluation.</param>
        /// <param name="populationSize">The size of the population.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        private static void Run<TGene>(int testNumber, string testDescription, GeneticAlgorithm<TGene > geneticAlgorithm,
            int populationSize, double crossoverRate, double mutationRate,
            double acceptableEvaluation, int maxGenerations)
        {
            Console.WriteLine($"Test {testNumber}: {testDescription}");

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            Result<TGene> result = geneticAlgorithm.Run(populationSize, crossoverRate, mutationRate, false, acceptableEvaluation, maxGenerations);
            stopwatch.Stop();
        
            // Print the results.
            Console.WriteLine($"Test {testNumber}: Duration: {stopwatch.Elapsed.Milliseconds} ms");
            Console.WriteLine($"Test {testNumber}: Number of generations taken: {result.Generations}");
            Console.WriteLine($"Test {testNumber}: Best solution: {Chromosome<TGene>.Print(result.Solution)}");
            Console.WriteLine($"Test {testNumber}: Best solution's evaluation: {result.Evaluation}");
        }
    }
}
