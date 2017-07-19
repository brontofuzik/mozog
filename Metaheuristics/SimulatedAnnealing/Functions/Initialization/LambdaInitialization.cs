using System;

namespace SimulatedAnnealing.Functions.Initialization
{
    public class LambdaInitialization<T> : InitializationFunction<T>
    {
        private readonly Func<int, T[]> initialize;

        public LambdaInitialization(Func<int, T[]> initialize)
        {
            this.initialize = initialize;
        }

        public override T[] Initialize(int dimension) => initialize(dimension);
    }
}
