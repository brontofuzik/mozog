using System;
using GeneticAlgorithm.Functions.Initialization;

namespace GeneticAlgorithm.Configuration
{
    public class InitializationConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public InitializationConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> Piecewise(Func<TGene> initialize)
            => SetInitialization(new PiecewiseInitialization<TGene>(initialize));

        public AlgorithmConfigurer<TGene> Lambda(Func<int, TGene[]> initialize)
            => SetInitialization(new LambdaInitialization<TGene>(initialize));

        private AlgorithmConfigurer<TGene> SetInitialization(IInitializationFunction<TGene> initialization)
        {
            Algo.Initializer = initialization;
            return AlgoConfigurer;
        }
    }
}
