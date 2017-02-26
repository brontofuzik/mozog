using System.Security.Policy;

namespace GeneticAlgorithm.Configuration
{
    public class AlgorithmConfigurer<TGene> : Configurer<TGene>
    {
        public AlgorithmConfigurer(GeneticAlgorithm<TGene> algo) : base(algo)
        {
        }

        public FitnessConfigurer<TGene> Fitness => new FitnessConfigurer<TGene>(Algo, this);

        public InitializationConfigurer<TGene> Initialization => new InitializationConfigurer<TGene>(Algo, this);

        public SelectionConfigurer<TGene> Selection => new SelectionConfigurer<TGene>(Algo, this);

        public CrossoverConfigurer<TGene> Crossover => new CrossoverConfigurer<TGene>(Algo, this);

        public MutationConfigurer<TGene> Mutation => new MutationConfigurer<TGene>(Algo, this);

        public TerminationConfigurer<TGene> Termination => new TerminationConfigurer<TGene>(Algo, this);
    }
}