using System;

namespace SimulatedAnnealing.Functions.Objective
{
    public class ObjectiveFunction<T> : FunctionBase<T>, IObjectiveFunction<T>
    {
        public static ObjectiveFunction<T> Maximize(Func<T[], double> func)
            => new ObjectiveFunction<T>(func, Objective.Maximize);

        public static ObjectiveFunction<T> Minimize(Func<T[], double> func)
            => new ObjectiveFunction<T>(func, Objective.Minimize);

        private ObjectiveFunction(Func<T[], double> func, Objective objective)
        {
            Func = func;
            Objective = objective;
        }

        public Func<T[], double> Func { get; }

        public Objective Objective { get; }

        public double Evaluate(T[] state) => Objective == Objective.Minimize ? Func(state) : 1 / Func(state);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
