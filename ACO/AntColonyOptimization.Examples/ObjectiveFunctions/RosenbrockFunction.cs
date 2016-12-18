using System;

namespace AntColonyOptimization.Examples.ObjectiveFunctions
{
    /// <summary>
    /// A Rosenbrock (R) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page2537.htm
    // Also known as Rosenbrock's valley or Rosenbrock's banana.
    /// </summary>
    internal class RosenbrockFunction : ObjectiveFunction
    {
        /// <summary>
        /// Creates a Rosenbrock (R) function.
        /// </summary>
        /// <param name="dimension">The dimension of the R function.</param>
        public RosenbrockFunction( int dimension )
            : base( dimension, Objective.Minimize )
        {
        }

        public override double Evaluate( double[] steps )
        {
            double output = 0.0;
            // f( x ) = SUM[ i = 0 .. n - 2 ]f( x )_i
            for (int i = 0; i < steps.Length - 1; i++)
            {
                // f( x )_i = (1 - x_i)^2 + 100 * (x_i+1 - (x_i)^2)^2 
                output += Math.Pow( 1 - steps[ i ], 2 ) + 100 * Math.Pow( steps[ i + 1 ] - Math.Pow( steps[ i ], 2 ), 2 );
            }
            return output;
        }
    }
}
