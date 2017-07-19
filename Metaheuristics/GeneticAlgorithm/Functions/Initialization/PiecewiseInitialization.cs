using System;

namespace GeneticAlgorithm.Functions.Initialization
{
    public class PiecewiseInitialization<TGene> : InitializationFunction<TGene>
    {
        private readonly Func<TGene> initialize;

        public PiecewiseInitialization(Func<TGene> initialize)
        {
            this.initialize = initialize;
        }

        public override TGene[] Initialize(int chromosomeSize)
        {
            TGene[] genes = new TGene[chromosomeSize];
            for (int i = 0; i < chromosomeSize; i++)
            {
                genes[i] = initialize();
            }
            return genes;
        }
    }
}