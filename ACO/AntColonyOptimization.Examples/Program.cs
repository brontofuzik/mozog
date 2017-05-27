﻿using System;
using Mozog.Utils;

namespace AntColonyOptimization.Examples
{
    internal class Program
    {
        public static void Main(string[] args)
        {        
            Run("Sphere model function (SM)", FunctionOptimization.SphereModelFunction, 
                ants: 8, pdfs: 3,
                targetEvaluation: Double.MinValue, maxIterations: 10_000);
            
            Run("Goldstein and Price function (GP)", FunctionOptimization.GoldsteinPriceFunction,
                ants: 6, pdfs: 4,
                targetEvaluation: Double.MinValue, maxIterations: 10_000);
            
            Run("Rosenbrock function (R2)", FunctionOptimization.RosenbrockFunction,
                ants: 30, pdfs: 8,
                targetEvaluation: Double.MinValue, maxIterations: 10_000);

            
            Run("Zakharov function (Z2)", FunctionOptimization.ZakharovFunction,
                ants: 8, pdfs: 4,
                targetEvaluation: Double.MinValue, maxIterations: 10_000);
        }

        private static void Run(string testDescription, AntColonyOptimization algo, int ants, int pdfs, double targetEvaluation, int maxIterations)
        {
            Console.WriteLine(testDescription);

            var (result, elapsedTime) = Misc.MeasureTime(() => algo.Run(ants, pdfs, targetEvaluation, maxIterations));

            // Print the results.
            Console.WriteLine($"Duration: {elapsedTime.TotalSeconds} s");
            Console.WriteLine($"Number of iterations: {result.Iterations}");
            Console.WriteLine($"Solution: {result.Solution}");
        }
    }
}
