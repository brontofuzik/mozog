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
            => Swarm.Build(dimension, func, new Parameters
            {
                SwarmSize = 60,
                MaxNeighbours = 5,
                W = 0.7,
                C1 = 1.4,
                C2 = 1.4,
                RangeMin = -10.0,
                RangeMax = +10.0,
                MaxEpochs = 35_000,
                MaxStaticEpochs = 20_000,
            }.Validate());
    }
}
