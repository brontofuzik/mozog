using System;

namespace GeneticAlgorithm.Functions.Initialization
{
    public class LambdaInitialization<TGene> : InitializationFunction<TGene>
    {
        private readonly Func<int, TGene[]> initialize;

        public LambdaInitialization(Func<int, TGene[]> initialize)
        {
            this.initialize = initialize;
        }

        public override TGene[] Initialize(int chromosomeSize) => initialize(chromosomeSize);
    }
}
