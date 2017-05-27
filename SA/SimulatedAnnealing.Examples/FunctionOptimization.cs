using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using SimulatedAnnealing.Functions.Cooling;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;
using static System.Math;

namespace SimulatedAnnealing.Examples
{
    static class FunctionOptimization
    {
        public static SimulatedAnnealing<double> RastriginFunction
            => Optimizer(2, Mozog.Examples.Functions.Rastrigin, -5.12, +5.12);

        public static SimulatedAnnealing<double> AckleyFunction
            => Optimizer(2, Mozog.Examples.Functions.Ackley, -5, +5);

        public static SimulatedAnnealing<double> SphereFunction
            => Optimizer(2, Mozog.Examples.Functions.Sphere, -10, +10);

        public static SimulatedAnnealing<double> RosenbrockFunction
            => Optimizer(2, Mozog.Examples.Functions.Rosenbrock, -10, +10);

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
        {
            double neighborhoodRadius = Sqrt(Abs(max - min));
            return new SimulatedAnnealing<double>(dimension)
            {
                Objective = ObjectiveFunction<double>.Minimize(func),
                Initialization = new LambdaInitialization<double>(dim => dim.Times(() => StaticRandom.Double(min, max)).ToArray()),
                Perturbation = new LambdaPerturbation<double>(state => state.Select(s => (s + StaticRandom.Normal(0, neighborhoodRadius)).Clamp(min, max)).ToArray()),
                Cooling = LambdaCooling.Exponential
            };
        }
    }
}
