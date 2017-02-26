using System;

namespace GeneticAlgorithm.Functions.Initialization
{
    public abstract class InitializationFunction<TGene> : FunctionBase<TGene>, IInitializationFunction<TGene>
    {
        public abstract TGene[] Initialize(int chromosomeSize);
    }

    public static class Initialization
    {
        public static IInitializationFunction<TGene> Piecewise<TGene>(Func<TGene> initialize)
            => new PiecewiseInitialization<TGene>(initialize);

        public static IInitializationFunction<TGene> Lambda<TGene>(Func<int, TGene[]> initialize)
            => new LambdaInitialization<TGene>(initialize);
    }
}