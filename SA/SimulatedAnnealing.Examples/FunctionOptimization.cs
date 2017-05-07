using System;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;

namespace SimulatedAnnealing.Examples
{
    static class FunctionOptimization
    {
        public static SimulatedAnnealing<double> Algorithm1 =>
            new SimulatedAnnealing<double>(1)
            {
                Objective = ObjectiveFunction<double>.Minimize(state => 1 - Math.Sin(state[0]) / state[0]),
                Initializer = Initializer,
                Perturbator = Perturbator
            };

        public static SimulatedAnnealing<double> Algorithm2 =>
            new SimulatedAnnealing<double>(2)
            {
                Objective = ObjectiveFunction<double>.Minimize(state =>
                {
                    double x = state[0];
                    double y = state[1];
                    double exponent1 = -Math.Pow(x, 2) - Math.Pow(y + 1, 2);
                    double exponent2 = -Math.Pow(x, 2) - Math.Pow(y, 2);
                    double exponent3 = -Math.Pow(x + 1, 2) - Math.Pow(y, 2);
                    return 8 - 3 * Math.Pow(1 - x, 2) * Math.Exp(exponent1) - 10 * (1 / 5.0 * x - Math.Pow(x, 3) - Math.Pow(y, 5)) * Math.Exp(exponent2) - 1 / 3.0 * Math.Exp(exponent3);
                }),
                Initializer = Initializer,
                Perturbator = Perturbator
            };

        private static IInitializationFunction<double> Initializer
            => new LambdaInitialization<double>(dimension => new[] {StaticRandom.Double(-20, +20)});

        private static IPerturbationFunction<double> Perturbator
            => new LambdaPerturbation<double>(state =>
            {
                double[] newState = new double[state.Length];
                Array.Copy(state, newState, state.Length);

                newState[StaticRandom.Int(0, newState.Length)] = StaticRandom.Double(-20, +20);

                return newState;
            });
    }
}
