namespace GeneticAlgorithm.Functions.Crossover
{
    public interface ICrossoverOperator<TGene> : IFunction<TGene>
    {
        void CrossOver(TGene[] offspring1, TGene[] offspring2);
    }
}