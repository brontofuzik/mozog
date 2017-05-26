using System;
using Mozog.Utils;

namespace SimulatedAnnealing.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Run("Rastrigin function", FunctionOptimization.RastriginFunction);
            //Run("Ackley function", FunctionOptimization.AckleyFunction);
            //Run("Sphere function", FunctionOptimization.SphereFunction);
            //Run("Rosenbrock function", FunctionOptimization.RosenbrockFunction);
            //Run("Function 1", FunctionOptimization.Function1);
            //Run("Function 2", FunctionOptimization.Function2);

            Run("Travelling salesman problem (TSP)", TravellingSalesmanProblem.Algorithm());
        }

        private static void Run<T>(string testName, SimulatedAnnealing<T> simulatedAnnealing, int maxIterations = 1_000_000,
            double initialTemperature = 1_000, double finalTemperature = 0.001, double targetEnergy = Double.MinValue)
        {
            Console.WriteLine(testName);

            var (result, elapsedTime) = Misc.MeasureTime(() => simulatedAnnealing.Run(initialTemperature, finalTemperature, targetEnergy, maxIterations));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Solution: {result.State}");
        }
    }
}
