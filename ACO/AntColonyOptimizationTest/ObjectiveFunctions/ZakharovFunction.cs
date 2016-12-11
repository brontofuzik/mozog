using System;

using AntColonyOptimization;

namespace AntColonyOptimizationTest.ObjectiveFunctions
{
    /// <summary>
    /// A Zakharov (Z) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page3088.htm
    /// </summary>
    internal class ZakharovFunction
        : ObjectiveFunction
    {
        #region Public instance constructors

        /// <summary>
        /// Creates a Zakharov (Z) function.
        /// </summary>
        /// <param name="dimension">The dimension of the Z function.</param>
        public ZakharovFunction( int dimension )
            : base( dimension, Objective.MINIMIZE )
        {
        }

        #endregion // Public instance constructors

        #region Public insatnce methods

        public override double Evaluate( double[] steps )
        {
            int n = 2;
            double sum1 = 0.0;
            double sum2 = 0.0;
            for (int i = 0; i < n; i++)
            {
                sum1 += Math.Pow( steps[ i ], 2 );
                sum2 += 0.5 * i * steps[ i ];
            }
            return sum1 + Math.Pow( sum2,2 ) + Math.Pow( sum2,4 );
        }

        #endregion // Public instance methods
    }
}
