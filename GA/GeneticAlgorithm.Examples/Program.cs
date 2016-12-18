using System;
using System.Text;
using GeneticAlgorithm.Examples.Problems;
using GeneticAlgorithm.Examples.ObjectiveFunctions;

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
            // Best solution : [1, 1, 1, 0, 1]
            // Best solution's evalaution : 15
            Test<int>(1, "Genetic algorithm (GA) : The Knapsack problem (KP)", new KnapsackProblemGeneticAlgorithm(), new KnapsackProblemObjectiveFunction(),
                1000, Double.MaxValue,
                100, 0.80, 0.05, false
           );
            Console.WriteLine();
            
            // Best solution : [A, B, C, D]
            // Best solution's evalaution : 97
            Test<char>(2, "genetic algorithm (GA) : The travelling salesman problem (TSP)", new TravellingSalesmanProblemGeneticAlgorithm(), new TravellingSalesmanObjectiveFunction(),
                1000, Double.MinValue,
                100, 0.80, 0.05, false
           );
            Console.WriteLine();
        }

        /// <summary>
        /// Tests a genetic algorithm.
        /// </summary>
        /// <typeparam name="TGene">The type of the gene.</typeparam>
        /// <param name="testNumber">The number of the test.</param>
        /// <param name="testDescription">The description of the test.</param>
        /// <param name="geneticAlgorithm">The genetic algorithm to test.</param>
        /// <param name="objectiveFunction">The objective function to test.</param>
        /// 
        /// <param name="maxGenerationCount">The maximum number of generations (the computational budget).</param>
        /// <param name="acceptableEvaluation">The acceptable evaluation.</param>
        /// 
        /// <param name="populationSize">The size of the population.</param>
        /// <param name="crossoverRate">The rate of crossover.</param>
        /// <param name="mutationRate">The rate of mutation.</param>
        /// <param name="scaling">The scaling flag.</param>
        private static void Test<TGene >(int testNumber, string testDescription, GeneticAlgorithm<TGene > geneticAlgorithm, ObjectiveFunction<TGene > objectiveFunction,
            int maxGenerationCount, double acceptableEvaluation,
            int populationSize, double crossoverRate, double mutationRate, bool scaling)
        {
            // Print the number of the test and its description.
            Console.WriteLine("Test " + testNumber + " : " + testDescription);

            // Run the genetic algorithm.
            DateTime startTime = DateTime.Now;
            int takenGenerationCount;
            double achievedEvaluation;
            TGene[] solution = geneticAlgorithm.Run(objectiveFunction,
                maxGenerationCount, out takenGenerationCount, acceptableEvaluation, out achievedEvaluation,
                populationSize, crossoverRate, mutationRate, scaling
           );
            DateTime endTime = DateTime.Now;

            // Build the solution string.
            StringBuilder solutionSB = new StringBuilder();
            solutionSB.Append("[");
            foreach (TGene component in solution)
            {
                solutionSB.Append(component + ", ");
            }
            if (solution.Length != 0)
            {
                solutionSB.Remove(solutionSB.Length - 2, 2);
            }
            solutionSB.Append("]");
        
            // Print the results.
            Console.WriteLine("Test " + testNumber + " : Duration : " + (endTime - startTime));
            Console.WriteLine("Test " + testNumber + " : Number of generations taken : " + takenGenerationCount);
            Console.WriteLine("Test " + testNumber + " : Best solution : " + solutionSB);
            Console.WriteLine("Test " + testNumber + " : Best solution's evaluation : " + achievedEvaluation);
        }
    }
}
