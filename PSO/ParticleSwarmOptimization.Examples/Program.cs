using System;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace ParticleSwarmOptimization.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Run("Beale function", FunctionOptimization.BealeFunction);
            Run("Griewank function", FunctionOptimization.GriewankFunction);
            Run("Rosenbrock function", FunctionOptimization.RosenbrockFunction);
            Run("Sphere function", FunctionOptimization.SphereFunction);
        }

        private static void Run(string testName, Swarm optimizer, int maxIterations = 1_000_000)
        {
            Console.WriteLine(testName);

            var (result, elapsedTime) = Misc.MeasureTime(() => optimizer.Optimize());

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: TODO");
            Console.WriteLine($"Solution: {Vector.ToString(result.position)} = {result.error}");
        }
    }
}
