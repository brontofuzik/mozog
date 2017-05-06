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

        public double Evaluate(T[] state) => Objective == Objective.Minimize ? EvaluateInternal(state) : 1 / EvaluateInternal(state);

        public abstract double EvaluateInternal(T[] state);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
