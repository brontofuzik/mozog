using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;
using static System.Math;

namespace SimulatedAnnealing.Examples
{
    static class FunctionOptimization
    {
        // Rastrigin function
        // Minimum: f(0, 0) = 0
        public static SimulatedAnnealing<double> RastriginFunction
            => Optimizer(2, s =>
            {
                double x = s[0];
                double y = s[1];

                return 20 + (Pow(x,2) - 10 * Cos(2*PI*x)) + (Pow(y, 2) - 10 * Cos(2*PI*y));
            }, -5.12, +5.12);

        // Ackley function
        // Minimum: f(0, 0) = 0
        public static SimulatedAnnealing<double> AckleyFunction
            => Optimizer(2, s =>
            {
                double x = s[0];
                double y = s[1];

                return -20 * Exp(-0.2 * Sqrt(0.5 * (Pow(x,2) + Pow(y,2)))) - Exp(0.5 * (Cos(2*PI*x) + Cos(2*PI*y))) + E + 20;
            }, -5, +5);

        // Sphere function
        // Minimum: f(0, 0) = 0
        public static SimulatedAnnealing<double> SphereFunction
            => Optimizer(2, s =>
            {
                double x = s[0];
                double y = s[1];

                return Pow(x, 2) + Pow(y, 2);

            }, -10, +10);

        // Rosenbrock function
        // Minimum: f(1, 1) = 0
        public static SimulatedAnnealing<double> RosenbrockFunction
            => Optimizer(2, s =>
            {
                double x = s[0];
                double y = s[1];

                return 100 * Pow(y - Pow(x,2), 2) + Pow(x-1, 2);
            }, -10, +10);

        // Unknown 1D function
        // Minimum: f(x) = 0
        public static SimulatedAnnealing<double> Function1
            => Optimizer(1, s => 1 - Sin(s[0]) / s[0], -10, +10);

        // Unknown 2D function
        // Minimum: f(-0.26, -1.47) = -2.94
        public static SimulatedAnnealing<double> Function2
            => Optimizer(2, s =>
            {
                double x = s[0];
                double y = s[1];

                double exp1 = -Pow(x, 2) - Pow(y + 1, 2);
                double exp2 = -Pow(x, 2) - Pow(y, 2);
                double exp3 = -Pow(x + 1, 2) - Pow(y, 2);

                return 8 - 3 * Pow(1-x,2) * Exp(exp1) - 10 * (1 / 5.0*x - Pow(x,3) - Pow(y,5)) * Exp(exp2) - 1 / 3.0 * Exp(exp3);
            }, -10, +10);

        private static SimulatedAnnealing<double> Optimizer(int dimension, Func<double[], double> func, double min, double max)
            => new SimulatedAnnealing<double>(dimension)
            {
                Objective = ObjectiveFunction<double>.Minimize(func),
                Initializer = Initializer(min, max),
                Perturbator = Perturbator(min, max)
            };

        private static IInitializationFunction<double> Initializer(double min, double max)
            => new LambdaInitialization<double>(dimension => dimension.Times(() => StaticRandom.Double(min, max)).ToArray());

        private static IPerturbationFunction<double> Perturbator(double min, double max)
        {
            double minSqrt = -Sqrt(Abs(min));
            double maxSqrt = Sqrt(max);
            return new LambdaPerturbation<double>(state =>
            {
                return state.Select(s => (s + StaticRandom.Double(minSqrt, maxSqrt)).Clamp(min, max)).ToArray();
            });
        }
    }
}
