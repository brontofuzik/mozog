namespace GeneticAlgorithm.Functions.Initialization
{
    public interface IInitializationFunction<TGene> : IFunction<TGene>
    {
        TGene[] Initialize(int chromosomeSize);
    }
}