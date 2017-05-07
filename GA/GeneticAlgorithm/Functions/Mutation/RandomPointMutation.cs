using System;
using Mozog.Utils.Math;

namespace GeneticAlgorithm.Functions.Mutation
{
    public class RandomPointMutation<TGene> : MutationOperator<TGene>
    {
        private readonly Func<TGene, TGene> mutate;

        public RandomPointMutation(Func<TGene, TGene> mutate)
        {
            this.mutate = mutate;
        }

        public override void Mutate(TGene[] offspring)
        {
            int index = StaticRandom.Int(0, offspring.Length);
            offspring[index] = mutate(offspring[index]);
        }
    }
}