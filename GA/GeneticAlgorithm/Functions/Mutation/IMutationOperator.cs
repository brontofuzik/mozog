namespace GeneticAlgorithm.Functions.Mutation
{
    public interface IMutationOperator<TGene> : IFunction<TGene>
    {
        void Mutate(TGene[] offspring);
    }
}