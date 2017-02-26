namespace GeneticAlgorithm.Functions.Termination
{
    public abstract class TerminationFunction<TGene> : FunctionBase<TGene>, ITerminationFunction<TGene>
    {
        public abstract bool ShouldTerminate();
    }
}