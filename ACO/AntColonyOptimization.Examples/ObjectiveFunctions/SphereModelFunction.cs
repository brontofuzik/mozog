using System;

using AntColonyOptimization;

namespace AntColonyOptimization.Examples.ObjectiveFunctions
{
    /// <summary>
    /// A sphere model (SM) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page1113.htm
    /// </summary>
    internal class SphereModelFunction
        : ObjectiveFunction
    {
        #region Public instance constructors

        /// <summary>
        /// Creates a sphere model (SM) function.
        /// </summary>
        /// <param name="dimension">The dimension of the SM function.</param>
        public SphereModelFunction( int dimension )
            : base( dimension, Objective.MINIMIZE )
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        public override double Evaluate( double[] steps )
        {
            double output = 0.0;
            for (int i = 0; i < steps.Length; i++)
            {
                output += Math.Pow( steps[ i ], 2 );
            }
            return output;
        }

        #endregion // Public instance methods
    }
}
