namespace GeneticAlgorithm.Functions.Termination
{
    public interface ITerminationFunction<TGene> : IFunction<TGene>
    {
        bool ShouldTerminate();
    }
}