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
            => new Swarm(dimension, swarmSize: 60, neighbours: 5)
            {
                ObjetiveFunction = LambdaObjectiveFunction.Minimize(func, -10, +10),
                InitializePosition = p => StaticRandom.DoubleArray(p.Dimension, p.Min, p.Max),
                InitializeVelocity = p =>
                {
                    var maxVelocity = p.Max - p.Min;
                    return StaticRandom.DoubleArray(p.Dimension, -maxVelocity, maxVelocity);
                }
            };
    }
}
