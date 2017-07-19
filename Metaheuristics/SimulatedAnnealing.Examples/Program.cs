using System;
using Mozog.Utils;

namespace SimulatedAnnealing.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Run("Travelling salesman problem (TSP)", TravellingSalesmanProblem.Algorithm());

            Run("Ackley function", FunctionOptimization.AckleyFunction);
            Run("Rastrigin function", FunctionOptimization.RastriginFunction);
            Run("Rosenbrock function", FunctionOptimization.RosenbrockFunction);
            Run("Sphere function", FunctionOptimization.SphereFunction);

            //Run("Function 1", FunctionOptimization.Function1);
            //Run("Function 2", FunctionOptimization.Function2);
        }

        private static void Run<T>(string testName, SimulatedAnnealing<T> simulatedAnnealing,
            double initialTemperature = 1_000, double finalTemperature = 0.001,
            int maxIterations = 1_000_000, double targetEnergy = Double.MinValue)
        {
            Console.WriteLine(testName);

            var (result, elapsedTime) = Misc.MeasureTime(() => simulatedAnnealing.Run(initialTemperature, finalTemperature, maxIterations, targetEnergy));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Solution: {result.State}");
            Console.WriteLine(Separator);
        }

        private static readonly string Separator = new String('=', 80);
    }
}
