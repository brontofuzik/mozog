namespace SimulatedAnnealing
{
    /// <remarks>
    /// The objective function.
    /// </remarks>
    /// <typeparam name="T">The type of the state.</typeparam>
    public abstract class ObjectiveFunction< T >
    {
        #region Private instance fields

        int dimension;

        Objective objective;

        #endregion // Private insatnce fields

        #region Public insatnce properties

        public int Dimension
        {
            get
            {
                return dimension;
            }
        }

        public Objective Objective
        {
            get
            {
                return objective;
            }
        }

        #endregion // Public instance properties

        #region Protected instance constructors

        protected ObjectiveFunction( int dimension, Objective objective )
        {
            this.dimension = dimension;
            this.objective = objective;
        }

        #endregion // Protected instance constructors

        #region Public instance methods

        /// <summary>
        /// Evaluates the objective function.
        /// </summary>
        /// <param name="state">The state to evalaute.</param>
        /// <returns>
        /// The evaluation of the state.
        /// </returns>
        public abstract double Evaluate( T[] state );

        #endregion // Public instance methods
    }

    /// <summary>
    /// The objective - minimize or maximize the objective function?
    /// </summary>
    public enum Objective
    {
        MINIMIZE,
        MAXIMIZE
    }
}
