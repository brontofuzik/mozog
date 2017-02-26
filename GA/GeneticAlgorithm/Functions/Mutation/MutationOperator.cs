using System;

namespace GeneticAlgorithm.Functions.Mutation
{
    public abstract class MutationOperator<TGene> : FunctionBase<TGene>, IMutationOperator<TGene>
    {
        public abstract void Mutate(TGene[] offspring);
    }

    public static class Mutation
    {
        public static IMutationOperator<TGene> RandomPoint<TGene>(Func<TGene, TGene> mutate)
            => new RandomPointMutation<TGene>(mutate);

        public static IMutationOperator<TGene> Lambda<TGene>(Action<TGene[]> mutate)
            => new LambdaMutation<TGene>(mutate);
    }
}