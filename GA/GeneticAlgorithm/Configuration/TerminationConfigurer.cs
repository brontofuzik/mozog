using System.Collections.Generic;
using GeneticAlgorithm.Functions.Termination;

namespace GeneticAlgorithm.Configuration
{
    public class TerminationConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public TerminationConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> MaxGenerations(int maxGenerations)
            => SetTermination(new MaxGenerations<TGene>(maxGenerations));

        public AlgorithmConfigurer<TGene> MinEvaluation(double minEvaluation)
            => SetTermination(new MinEvaluation<TGene>(minEvaluation));

        public AlgorithmConfigurer<TGene> MaxGenerationsOrMinEvaluation(int maxGenerations, double minEvaluation)
            => SetTermination(new AnyTermination<TGene>(new List<ITerminationFunction<TGene>>
            {
                new MaxGenerations<TGene>(maxGenerations),
                new MinEvaluation<TGene>(minEvaluation)
            }));

        private AlgorithmConfigurer<TGene> SetTermination(ITerminationFunction<TGene> termination)
        {
            Algo.Terminator = termination;
            return AlgoConfigurer;
        }
    }
}
