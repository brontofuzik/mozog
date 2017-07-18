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

        private static void Run(string testName, Swarm optimizer, int maxIterations = 10_000)
        {
            Console.WriteLine(testName);

            var (result, elapsedTime) = Misc.MeasureTime(() => optimizer.Optimize(maxIterations));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Solution: {Vector.ToString(result.Position)} = {result.Error}");
            Console.WriteLine(Separator);
        }

        private static readonly string Separator = new String('=', 80);
    }
}
