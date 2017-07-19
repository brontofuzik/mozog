using System;

namespace AntColonyOptimization
{
    public class ObjectiveFunction : IObjectiveFunction
    {
        public static ObjectiveFunction Maximize(Func<double[], double> func)
            => new ObjectiveFunction(func, Objective.Maximize);

        public static ObjectiveFunction Minimize(Func<double[], double> func)
            => new ObjectiveFunction(func, Objective.Minimize);

        private ObjectiveFunction(Func<double[], double> func, Objective objective)
        {
            Func = func;
            Objective = objective;
        }

        public Func<double[], double> Func { get; }

        public Objective Objective { get; }

        private bool IsMinimized => Objective == Objective.Minimize;

        public double Evaluate(double[] xs) => IsMinimized ? Func(xs) : 1 / Func(xs);

        public bool IsAcceptable(double[] xs, double targetEvaluation)
            => IsMinimized ? Func(xs) <= targetEvaluation : Func(xs) >= targetEvaluation;
    }

    public interface IObjectiveFunction
    {
        double Evaluate(double[] xs);
    }

    public enum Objective
    {
        Minimize,
        Maximize
    }
}
