namespace SimulatedAnnealing
{
    public abstract class ObjectiveFunction<T>
    {
        protected ObjectiveFunction(Objective objective)
        {
            Objective = objective;
            //Dimension = dimension;
        }

        public Objective Objective { get; }

        //public int Dimension { get; }

        public double Evaluate(T[] state) => Objective == Objective.Minimize ? EvaluateInternal(state) : 1 / EvaluateInternal(state);

        public abstract double EvaluateInternal(T[] state);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
