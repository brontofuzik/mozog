namespace GeneticAlgorithm.Functions
{
    public interface IFunction<TGene>
    {
        GeneticAlgorithm<TGene> Algo { get; set; }
    }

    public abstract class FunctionBase<TGene> : IFunction<TGene>
    {
        public GeneticAlgorithm<TGene> Algo { get; set; }
    }
}