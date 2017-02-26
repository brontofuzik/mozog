using System;

namespace GeneticAlgorithm.Functions.Mutation
{
    public class LambdaMutation<TGene> : MutationOperator<TGene>
    {
        private readonly Action<TGene[]> mutate;

        public LambdaMutation(Action<TGene[]> mutate)
        {
            this.mutate = mutate;
        }

        public override void Mutate(TGene[] offspring) => mutate(offspring);
    }
}