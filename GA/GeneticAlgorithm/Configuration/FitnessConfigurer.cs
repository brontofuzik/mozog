using System;
using GeneticAlgorithm.Functions.Fitness;

namespace GeneticAlgorithm.Configuration
{
    public class FitnessConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public FitnessConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> Maximize(Func<TGene[], double> objectiveFunction)
            => SetFitness(FitnessFunction<TGene>.Maximize(objectiveFunction));

        public AlgorithmConfigurer<TGene> Minimize(Func<TGene[], double> objectiveFunction)
            => SetFitness(FitnessFunction<TGene>.Minimize(objectiveFunction));

        private AlgorithmConfigurer<TGene> SetFitness(IFitnessFunction<TGene> fitness)
        {
            Algo.Fitness = fitness;
            return AlgoConfigurer;
        }
    }
}
