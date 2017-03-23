using System;
using StaticRandom = Mozog.Utils.StaticRandom;

namespace GeneticAlgorithm.Functions.Mutation
{
    public class ProbabilisticMutation<TGene> : MutationOperator<TGene>
    {
        private readonly IMutationOperator<TGene> mutator;
        private readonly Type type;
        private readonly double constMutationRate;
        private readonly Func<int, double> mutationRateFunc;

        public static IMutationOperator<TGene> Constant(IMutationOperator<TGene> mutator, double constMutationRate)
            => new ProbabilisticMutation<TGene>(mutator, Type.Constant, constMutationRate, null);

        public static IMutationOperator<TGene> Variable(IMutationOperator<TGene> mutator, Func<int, double> mutationRateFunc)
            => new ProbabilisticMutation<TGene>(mutator, Type.Variable, 0.0, mutationRateFunc);

        private ProbabilisticMutation(IMutationOperator<TGene> mutator, Type type, double constMutationRate, Func<int, double> mutationRateFunc)
        {
            this.mutator = mutator;
            this.type = type;
            this.constMutationRate = constMutationRate;
            this.mutationRateFunc = mutationRateFunc;
        }

        public override void Mutate(TGene[] offspring)
        {
            switch (type)
            {
                case Type.Constant:
                    Mutate(offspring, constMutationRate);
                    break;

                case Type.Variable:                
                    Mutate(offspring, mutationRateFunc(Algo.CurrentGeneration));
                    break;

                default:
                    break;
            }
        }

        private void Mutate(TGene[] offspring, double mutationRate)
        {
            if (StaticRandom.Double() < mutationRate)
            {
                mutator.Mutate(offspring);
            }
        }

        private enum Type
        {
            Constant,
            Variable
        }
    }
}
