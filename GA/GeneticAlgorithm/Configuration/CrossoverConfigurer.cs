using GeneticAlgorithm.Functions.Crossover;

namespace GeneticAlgorithm.Configuration
{
    public class CrossoverConfigurer<TGene> : FunctionConfigurer<TGene>
    {
        public CrossoverConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer)
            : base(algo, algoConfigurer)
        {
        }

        public AlgorithmConfigurer<TGene> SinglePoint()
            => SetCrossover(new SinglePointCrossover<TGene>());

        public AlgorithmConfigurer<TGene> TwoPoint()
            => SetCrossover(new TwoPointCrossover<TGene>());

        public AlgorithmConfigurer<TGene> PartiallyMatched()
            => SetCrossover(new PartiallyMatchedCrossover<TGene>());

        private AlgorithmConfigurer<TGene> SetCrossover(ICrossoverOperator<TGene> crossover)
        {
            Algo.Crossover = crossover;
            return AlgoConfigurer;
        }
    }
}
