namespace AntColonyOptimization
{
    public abstract class ObjectiveFunction
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

        public abstract double Evaluate( double[] steps );

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
