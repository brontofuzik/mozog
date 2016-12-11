using System;
using System.Text;
using SimulatedAnnealing.Examples.ObjectiveFunctions;
using SimulatedAnnealing.Examples.SimulatedAnnealings;

namespace SimulatedAnnealing.Examples
{
    /// <remarks>
    /// The simulated annealing test suite.
    /// </remarks>
    internal class Program
    {
        /// <summary>
        /// The test harness.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        public static void Main( string[] args )
        {
            // Best solution : [0]
            // Best solution's evaluation : 0
            Test< double >( 1, "Simulated annealing (SA) : The one dimensional (1D) function", new RealFunctionSimulatedAnnealing(), new OneDimensionalFunction(),
                1000000, Double.MinValue,
                1000, 0.001
            );
            Console.WriteLine();


            // Best solution : [-0.26, -1.47]
            // Best solution's evalaution : -2.94
            Test< double >( 2, "Simulated annealing (SA) : The two dimensional (2D) function", new RealFunctionSimulatedAnnealing(), new TwoDimensionalFunction(),
                1000000, Double.MinValue,
                1000, 0.001
            );
            Console.WriteLine();
        }

        /// <summary>
        /// Tests a simulated annealing.
        /// </summary>
        /// <typeparam name="TGene"></typeparam>
        /// <param name="testNumber">The number of the test.</param>
        /// <param name="testDescription">The description of the test.</param>
        /// <param name="simulatedAnnealing">The simulated annealing to test.</param>
        /// <param name="objectiveFunction">The objective funtion to test.</param>
        /// 
        /// <param name="maxIterationCount">The maximum number of iterations (the computational budget).</param>
        /// <param name="acceptableEnergy">The acceptable energy.</param>
        /// 
        /// <param name="initialTemperature">The initial temperature.</param>
        /// <param name="finalTemperature">The final temperature.</param>
        /// <apra
        private static void Test< T >( int testNumber, string testDescription, SimulatedAnnealing< T > simulatedAnnealing, ObjectiveFunction< T > objectiveFunction,
            int maxIterationCount, double acceptableEnergy,
            double initialTemperature, double finalTemperature )
        {
            // Print the number of the test and its description.
            Console.WriteLine( "Test " + testNumber + " : " + testDescription );

            // Run the simulated annealing.
            DateTime startTime = DateTime.Now;
            int usedIterationCount;
            double achievedEnergy;
            T[] solution = simulatedAnnealing.Run( objectiveFunction,
                maxIterationCount, out usedIterationCount, acceptableEnergy, out achievedEnergy,
                initialTemperature, finalTemperature
            );
            DateTime endTime = DateTime.Now;

            // Build the solution string.
            StringBuilder solutionSB = new StringBuilder();
            solutionSB.Append( "[" );
            foreach (T component in solution)
            {
                solutionSB.Append( component + ", " );
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
            Console.WriteLine( "Test " + testNumber + " : Best solution's evaluation : " + achievedEnergy );
        }
    }
}
