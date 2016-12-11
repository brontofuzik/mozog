using System;

namespace AntColonyOptimization.Examples.ObjectiveFunctions
{
    /// <summary>
    /// A Goldstein and Price (GP) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page1760.htm
    /// </summary>
    internal class GoldsteinAndPriceFunction : ObjectiveFunction
    {
        /// <summary>
        /// Creates a Goldstein and Price (GP) function.
        /// </summary>
        /// <param name="dimension">The dimension of the GP function.</param>
        public GoldsteinAndPriceFunction( int dimension )
            : base( dimension, Objective.MINIMIZE )
        {
        }

        public override double Evaluate( double[] steps )
        {
            double x = steps[ 0 ];
            double y = steps[ 1 ];
            return (1 + Math.Pow( (x + y + 1), 2 ) * (19 - 14 * x + 3 * Math.Pow( x, 2 ) - 14 * y + 6 * x * y + 3 * Math.Pow( y, 2 )))
                 * (30 + Math.Pow( (2 * x - 3 * y), 2 ) * (18 - 32 * x + 12 * Math.Pow( x, 2 ) + 48 * y - 36 * x * y + 27 * Math.Pow( y, 2 )));    
        }
    }
}
