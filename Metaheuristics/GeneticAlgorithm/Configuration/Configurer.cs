namespace GeneticAlgorithm.Configuration
{
    public abstract class Configurer<TGene>
    {
        protected GeneticAlgorithm<TGene> Algo { get; private set; }

        protected Configurer(GeneticAlgorithm<TGene> algo)
        {
            Algo = algo;
        }
    }
}
