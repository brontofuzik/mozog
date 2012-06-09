using System;
using System.Text;

using AntColonyOptimization;
using AntColonyOptimizationTest.AntColonyOptimizations;
using AntColonyOptimizationTest.ObjectiveFunctions;

namespace AntColonyOptimizationTest
{
    /// <summary>
    /// The ant colony optimization test suite.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The test harness.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main(string[] args)
        {
            // Best solution : [0, 0, 0, 0, 0, 0]
            // best solution's evaluation : 0
            Test( 1, "Ant colony optimization (ACO) : The sphere model (SM) function", new RealFunctionAntColonyOptimization(), new SphereModelFunction( 6 ),
                10000, Double.MinValue,
                8, 3, 0.0001
            );
            Console.WriteLine();

            // Best solution : [0, -1]
            // best solution's evaluation : 3
            Test( 2, "Ant colony optimization (ACO) : The Goldstein and Price (GP) function", new RealFunctionAntColonyOptimization(), new GoldsteinAndPriceFunction( 2 ),
                10000, Double.MinValue,
                6, 4, 0.0001
            );
            Console.WriteLine();

            // Best solution : [1, 1]
            // best solution's evaluation : 0
            Test( 3, "Ant colony optimization (ACO) : The Rosenbrock (R2) function", new RealFunctionAntColonyOptimization(), new RosenbrockFunction( 2 ),
                10000, Double.MinValue,
                30, 8, 0.003
            );
            Console.WriteLine();

            // Best solution : [0, 0]
            // best solution's evaluation : 0
            Test( 4, "Ant colony optimization (ACO) : The Zakharov (Z2) function", new RealFunctionAntColonyOptimization(), new ZakharovFunction( 2 ),
                10000, Double.MinValue,
                8, 4, 0.0001
            );
            Console.WriteLine();
        }

        /// <summary>
        /// Test an ant colony optimization.
        /// </summary>
        /// <param name="testNumber">The number of the test.</param>
        /// <param name="testDescription">The description of the test.</param>
        /// <param name="antColonyOptimization">The ant colony optimization to test.</param>
        /// <param name="objectiveFunction">The objective function to test.</param>
        /// 
        /// <param name="maxIterationCount">The maximum number of iterations (the computational budget).</param>
        /// <param name="acceptableEvaluation">The acceptable evalaution.</param>
        /// 
        /// <param name="antCount">The number of ants.</param>
        /// <param name="normalPDFCount">The number of PDFs in each pheromone distribution.</param>
        /// <param name="requiredAccuracy">The required accuracy.</param>
        private static void Test( int testNumber, string testDescription, AntColonyOptimization.AntColonyOptimization antColonyOptimization, ObjectiveFunction objectiveFunction,
            int maxIterationCount, double acceptableEvaluation,
            int antCount, int normalPDFCount, double requiredAccuracy
        )
        {
            // Print the number of the test and its description.
            Console.WriteLine( "Test " + testNumber + " : " + testDescription );

            // Run the ant colony optimization.
            DateTime startTime = DateTime.Now;
            int usedIterationCount;
            double achievedEvalaution;
            double[] solution = antColonyOptimization.Run( objectiveFunction,
                maxIterationCount, out usedIterationCount, acceptableEvaluation, out achievedEvalaution,
                antCount, normalPDFCount, requiredAccuracy
            );
            DateTime endTime = DateTime.Now;

            // Build the solution string.
            StringBuilder solutionSB = new StringBuilder();
            solutionSB.Append( "[" );
            foreach (double component in solution)
            {
                solutionSB.Append( component.ToString( "F2" ) + ", " );
            }
            if (solution.Length != 0)
            {
                solutionSB.Remove( solutionSB.Length - 2, 2 );
            }
            solutionSB.Append( "]" );
        
            // Print the results.
            Console.WriteLine( "Test " + testNumber + " : Duration: " + (endTime - startTime) );
            Console.WriteLine( "Test " + testNumber + " : Number of iterations taken : " + usedIterationCount );
            Console.WriteLine( "Test " + testNumber + " : Best solution : " + solutionSB.ToString() );
            Console.WriteLine( "Test " + testNumber + " : Best solution's evaluation : " + achievedEvalaution );
        }
    }
}
