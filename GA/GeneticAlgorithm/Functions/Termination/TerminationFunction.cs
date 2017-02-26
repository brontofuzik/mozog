using System.Collections.Generic;

namespace GeneticAlgorithm.Functions.Termination
{
    public abstract class TerminationFunction<TGene> : FunctionBase<TGene>, ITerminationFunction<TGene>
    {
        public abstract bool ShouldTerminate();
    }

    public static class Termination
    {
        public static ITerminationFunction<TGene> MaxGenerations<TGene>(int maxGenerations)
            => new MaxGenerations<TGene>(maxGenerations);

        public static ITerminationFunction<TGene> MinEvaluation<TGene>(double minEvaluation)
            => new MinEvaluation<TGene>(minEvaluation);

        public static ITerminationFunction<TGene> MaxGenerationsOrMinEvaluation<TGene>(int maxGenerations, double minEvaluation)
            => new AnyTermination<TGene>(new List<ITerminationFunction<TGene>>
            {
                new MaxGenerations<TGene>(maxGenerations),
                new MinEvaluation<TGene>(minEvaluation)
            });
    }
}