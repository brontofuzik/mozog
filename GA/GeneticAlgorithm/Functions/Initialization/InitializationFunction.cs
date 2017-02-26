using System;

namespace GeneticAlgorithm.Functions.Initialization
{
    public abstract class InitializationFunction<TGene> : FunctionBase<TGene>, IInitializationFunction<TGene>
    {
        public abstract TGene[] Initialize(int chromosomeSize);
    }
}