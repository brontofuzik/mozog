using System;
using Random = Mozog.Utils.Random;

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
            int index = Random.Int(0, offspring.Length);
            offspring[index] = mutate(offspring[index]);
        }
    }
}