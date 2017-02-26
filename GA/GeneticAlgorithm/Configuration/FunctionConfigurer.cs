namespace GeneticAlgorithm.Configuration
{
    public abstract class FunctionConfigurer<TGene> : Configurer<TGene>
    {
        public AlgorithmConfigurer<TGene> AlgoConfigurer { get; }

        protected FunctionConfigurer(GeneticAlgorithm<TGene> algo, AlgorithmConfigurer<TGene> algoConfigurer) : base(algo)
        {
            AlgoConfigurer = algoConfigurer;
        }
    }
}
