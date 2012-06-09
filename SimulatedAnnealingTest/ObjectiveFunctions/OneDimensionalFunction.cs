using System;

using SimulatedAnnealing;

namespace SimulatedAnnealingTest.ObjectiveFunctions
{
    /// <summary>
    /// A one dimensional (1D) function.
    /// </summary>
    internal class OneDimensionalFunction
        : ObjectiveFunction< double >
    {
        #region Public instance constructors

        /// <summary>
        /// Creates a new one-dimensional (1D) function.
        /// </summary>
        public OneDimensionalFunction()
            : base( 1, Objective.MINIMIZE )
        {
        }

        #endregion // Public insatnce constructors

        #region Public instance methods

        /// <summary>
        /// Evalautes the objective function.
        /// </summary>
        /// <param name="state">The state of the system to evaluate.</param>
        /// <returns>
        /// The evaluation of the state of the system (i.e. the internal energy of the system).
        /// </returns>
        public override double Evaluate( double[] state )
        {
            return 1 - (Math.Sin( state[ 0 ] ) / state[ 0 ]);
        }

        #endregion // Public instance methods
    }
}
