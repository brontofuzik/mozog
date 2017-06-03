using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using ParticleSwarmOptimization.Functions.Objective;

namespace ParticleSwarmOptimization.Examples
{
    static class FunctionOptimization
    {
        public static Swarm BealeFunction => Optimizer(Functions.Beale, Functions.BealeDimension);

        public static Swarm GriewankFunction => Optimizer(Functions.Griewank, Functions.GriewankDimension);

        public static Swarm RosenbrockFunction => Optimizer(Functions.Rosenbrock, Functions.RosenbrockDimension);

        public static Swarm SphereFunction => Optimizer(Functions.Sphere, Functions.SphereDimension);

        private static Swarm Optimizer(Func<double[], double> func, int dimension)
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
