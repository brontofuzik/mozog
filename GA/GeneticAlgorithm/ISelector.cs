namespace GeneticAlgorithm
{
    public interface ISelector<TGene>
    {
        void Initialize(Population<TGene> population);

        Chromosome<TGene> Select();
    }
}
