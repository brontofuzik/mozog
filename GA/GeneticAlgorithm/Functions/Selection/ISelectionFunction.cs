namespace GeneticAlgorithm.Functions.Selection
{
    public interface ISelectionFunction<TGene> : IFunction<TGene>
    {
        void Initialize(Population<TGene> population);

        Chromosome<TGene> Select();
    }
}
