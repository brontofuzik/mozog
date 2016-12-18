using System;

namespace SimulatedAnnealing.Examples.ObjectiveFunctions
{
    /// <summary>
    /// A one dimensional (1D) function.
    /// </summary>
    internal class OneDimensionalFunction
        : ObjectiveFunction<double>
    {
        /// <summary>
        /// Creates a new one-dimensional (1D) function.
        /// </summary>
        public OneDimensionalFunction()
            : base(1, Objective.MINIMIZE)
        {
        }

        /// <summary>
        /// Evalautes the objective function.
        /// </summary>
        /// <param name="state">The state of the system to evaluate.</param>
        /// <returns>
        /// The evaluation of the state of the system (i.e. the internal energy of the system).
        /// </returns>
        public override double Evaluate(double[] state)
        {
            return 1 - Math.Sin(state[0]) / state[0];
        }
    }
}
