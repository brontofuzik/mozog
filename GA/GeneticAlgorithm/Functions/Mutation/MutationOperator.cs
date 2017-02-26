namespace GeneticAlgorithm.Functions.Mutation
{
    public abstract class MutationOperator<TGene> : FunctionBase<TGene>, IMutationOperator<TGene>
    {
        public abstract void Mutate(TGene[] offspring);
    }
}