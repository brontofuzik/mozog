namespace AntColonyOptimization
{
    public abstract class ObjectiveFunction
    {
        int dimension;

        Objective objective;

        protected ObjectiveFunction( int dimension, Objective objective )
        {
            this.dimension = dimension;
            this.objective = objective;
        }

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

        public abstract double Evaluate( double[] steps );
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
