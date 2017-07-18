using System;
using Mozog.Utils.Math;
using ParticleSwarmOptimization.Functions.Objective;

namespace ParticleSwarmOptimization.Examples
{
    static class FunctionOptimization
    {
        public static Swarm BealeFunction => Optimizer(Mozog.Examples.Functions.Beale, dimension: 2);

        public static Swarm GriewankFunction => Optimizer(Mozog.Examples.Functions.Griewank, dimension: 4);

        public static Swarm RosenbrockFunction => Optimizer(Mozog.Examples.Functions.Rosenbrock, dimension: 2);

        public static Swarm SphereFunction => Optimizer(Mozog.Examples.Functions.Sphere, dimension: 4);

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
