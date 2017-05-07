using System;
using Mozog.Utils;

namespace SimulatedAnnealing.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            RunFunctionOptimization1();
            RunFunctionOptimization2();
        }

        // Best solution : [0]
        // Best solution's evaluation : 0
        private static void RunFunctionOptimization1()
        {
            Run("One-dimensional function",
                FunctionOptimization.Algorithm1,
                maxIterations: 1_000_000,
                targetEnergy: Double.MinValue,
                initialTemperature: 1000,
                finalTemperature: 0.001);
        }

        // Best solution : [-0.26, -1.47]
        // Best solution's evalaution : -2.94
        private static void RunFunctionOptimization2()
        {
            Run("Two-dimensional function",
                FunctionOptimization.Algorithm2,
                maxIterations: 1_000_000,
                targetEnergy: Double.MinValue,
                initialTemperature: 1000,
                finalTemperature: 0.001);
        }

        private static void Run<T>(string testName, SimulatedAnnealing<T> simulatedAnnealing,
            int maxIterations, double targetEnergy, double initialTemperature, double finalTemperature)
        {
            Console.WriteLine($"{testName}:");

            var (result, elapsedTime) = Misc.MeasureTime(() => simulatedAnnealing.Run(maxIterations, targetEnergy, initialTemperature, finalTemperature));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Best solution: {State<T>.Print(result.State.S)}");
            Console.WriteLine($"Best solution's evaluation: {result.State.E}");
        }
    }
}
