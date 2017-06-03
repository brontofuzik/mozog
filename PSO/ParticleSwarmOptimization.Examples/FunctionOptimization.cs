using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using ParticleSwarmOptimization.Functions.Objective;

namespace ParticleSwarmOptimization.Examples
{
    static class FunctionOptimization
    {
        public static Swarm BealeFunction => Optimizer(Functions.BealeDimension, Functions.Beale);

        public static Swarm GriewankFunction => Optimizer(Functions.GriewankDimension, Functions.Griewank);

        public static Swarm RosenbrockFunction => Optimizer(Functions.RosenbrockDimension, Functions.Rosenbrock);

        public static Swarm SphereFunction => Optimizer(Functions.SphereDimension, Functions.Sphere);

        private static Swarm Optimizer(int dimension, Func<double[], double> func)
            => new Swarm(dimension, swarmSize:60, neighbours: 5)
            {
                ObjetiveFunction = LambdaObjectiveFunction.Minimize(func, -10, +10),
                InitializePosition = algo => UniformArray(algo.Dimension, algo.Min, algo.Max),
                InitializeVelocity = algo => UniformArray(algo.Dimension, -(algo.Max - algo.Min), -(algo.Max - algo.Min))
            };

        private static double[] UniformArray(int dimension, double min, double max)
            => dimension.Times(() => StaticRandom.Double(min, max)).ToArray();
    }
}
