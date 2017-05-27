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

        private bool Minimize => Objective == Objective.Minimize;

        public double Evaluate(double[] steps)
            => Minimize ? EvaluateInternal(steps) : 1 / EvaluateInternal(steps);

        protected abstract double EvaluateInternal(double[] steps);

        public bool IsAcceptable(double[] steps, double targetEvaluation)
        {
            var eval = EvaluateInternal(steps);
            return Minimize ? eval <= targetEvaluation : eval >= targetEvaluation;
        }
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
