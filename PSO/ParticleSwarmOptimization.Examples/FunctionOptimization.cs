using System;

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
                ErrorFunc = func,
                Min = -10.0,
                Max = +10.0
            };
    }
}
