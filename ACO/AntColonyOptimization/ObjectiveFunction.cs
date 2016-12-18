namespace AntColonyOptimization
{
    public abstract class ObjectiveFunction
    {
        protected ObjectiveFunction(int dimension, Objective objective)
        {
            Dimension = dimension;
            Objective = objective;
        }

        public int Dimension { get; }

        public Objective Objective { get; }

        public abstract double Evaluate(double[] steps);
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
