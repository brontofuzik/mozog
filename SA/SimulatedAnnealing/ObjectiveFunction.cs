namespace SimulatedAnnealing
{
    /// <remarks>
    /// The objective function.
    /// </remarks>
    /// <typeparam name="T">The type of the state.</typeparam>
    public abstract class ObjectiveFunction<T >
    {
        protected ObjectiveFunction(int dimension, Objective objective)
        {
            Dimension = dimension;
            Objective = objective;
        }

        public int Dimension { get; }

        public Objective Objective { get; }

        /// <summary>
        /// Evaluates the objective function.
        /// </summary>
        /// <param name="state">The state to evalaute.</param>
        /// <returns>
        /// The evaluation of the state.
        /// </returns>
        public abstract double Evaluate(T[] state);
    }

    /// <summary>
    /// The objective - minimize or maximize the objective function?
    /// </summary>
    public enum Objective
    {
        Minimize,
        Maximize
    }
}
