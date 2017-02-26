using System;
using GeneticAlgorithm.Functions.Mutation;

namespace GeneticAlgorithm.Configuration
{
    public class MutationConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public MutationConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> RandomPoint(Func<TGene, TGene> mutate)
            => SetMutation(new RandomPointMutation<TGene>(mutate));

        public AlgorithmConfigurer<TGene> Lambda(Action<TGene[]> mutate)
            => SetMutation(new LambdaMutation<TGene>(mutate));

        private AlgorithmConfigurer<TGene> SetMutation(IMutationOperator<TGene> mutation)
        {
            Algo.Mutator = mutation;
            return AlgoConfigurer;
        }
    }
}
