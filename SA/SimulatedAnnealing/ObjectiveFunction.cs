namespace SimulatedAnnealing
{
    public abstract class ObjectiveFunction<T>
    {
        protected ObjectiveFunction(int dimension, Objective objective)
        {
            Dimension = dimension;
            Objective = objective;
        }

        public int Dimension { get; }

        public Objective Objective { get; }

        public abstract double Evaluate(T[] state);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
