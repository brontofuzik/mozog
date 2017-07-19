using System;

namespace ParticleSwarmOptimization.Functions.Objective
{
    public class LambdaObjectiveFunction : IObjectiveFunction
    {
        private readonly Func<double[], double> func;
        private readonly Objective objective;

        public static LambdaObjectiveFunction Minimize(Func<double[], double> func, double min, double max)
            => new LambdaObjectiveFunction(Objective.Minimize, func, min, max);

        public static LambdaObjectiveFunction Maximize(Func<double[], double> func, double min, double max)
            => new LambdaObjectiveFunction(Objective.Maximize, func, min, max);

        public LambdaObjectiveFunction(Objective objective, Func<double[], double> func, double min, double max)
        {
            this.func = func;
            this.objective = objective;
            Min = min;
            Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        public double Evaluate(double[] input) => objective == Objective.Minimize ? func(input) : -func(input);
    }
}
