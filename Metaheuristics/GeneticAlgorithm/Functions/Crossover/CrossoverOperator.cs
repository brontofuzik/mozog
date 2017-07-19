namespace GeneticAlgorithm.Functions.Crossover
{
    public abstract class CrossoverOperator<TGene> : FunctionBase<TGene>, ICrossoverOperator<TGene>
    {
        public abstract void CrossOver(TGene[] offspring1, TGene[] offspring2);
    }
}