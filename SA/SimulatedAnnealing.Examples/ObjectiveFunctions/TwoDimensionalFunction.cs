using System;

namespace SimulatedAnnealing.Examples.ObjectiveFunctions
{
    /// <summary>
    /// A two dimensional (2D) function.
    /// </summary>
    internal class TwoDimensionalFunction
        : ObjectiveFunction< double >
    {
        /// <summary>
        /// Creates a new two-dimensional (2D) function.
        /// </summary>
        public TwoDimensionalFunction()
            : base( 2, Objective.MINIMIZE )
        {
        }

        /// <summary>
        /// Evalautes the objective function.
        /// </summary>
        /// <param name="weights">The state of the system to evaluate.</param>
        /// <returns>
        /// The evaluation of the state of the system (i.e. the internal energy of the system).
        /// </returns>
        public override double Evaluate( double[] state )
        {
            double x = state[ 0 ];
            double y = state[ 1 ];
            double exponent1 = - Math.Pow( x, 2 ) - Math.Pow( (y + 1), 2 );
            double exponent2 = - Math.Pow( x, 2 ) - Math.Pow( y, 2 );
            double exponent3 = - Math.Pow( (x + 1), 2 ) - Math.Pow( y, 2 );
            return 8 - 3 * Math.Pow( (1 - x), 2 ) * Math.Exp( exponent1 ) - 10 * ((1 / 5.0) * x - Math.Pow( x, 3 ) - Math.Pow( y, 5 )) * Math.Exp( exponent2 ) - (1 / 3.0) * Math.Exp( exponent3 );
        }
    }
}
