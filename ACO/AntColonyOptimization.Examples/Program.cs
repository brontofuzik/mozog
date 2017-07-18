using System;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace AntColonyOptimization.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            //Run("Goldstein and Price function (GP)", FunctionOptimization.GoldsteinPriceFunction,
            //    ants: 6, pdfs: 4);
            //Run("Rosenbrock function (R2)", FunctionOptimization.RosenbrockFunction,
            //    ants: 30, pdfs: 8);
            Run("Sphere model function (SM)", FunctionOptimization.SphereFunction,
                ants: 8, pdfs: 3);
            //Run("Zakharov function (Z2)", FunctionOptimization.ZakharovFunction,
            //    ants: 8, pdfs: 4);
        }

        private static void Run(string testDescription, AntColonyOptimization algo, int ants, int pdfs, int? maxIterations = 10_000, double? targetEvaluation = null)
        {
            Console.WriteLine(testDescription);

            var (result, elapsedTime) = Misc.MeasureTime(() => algo.Run(ants, pdfs, maxIterations, targetEvaluation));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Solution: {Vector.ToString(result.Solution)} = {result.Evaluation:F2}");
            Console.WriteLine(Separator);
        }

        private static readonly string Separator = new String('=', 80);
    }
}
