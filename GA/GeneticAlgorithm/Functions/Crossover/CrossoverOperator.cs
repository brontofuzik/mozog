namespace GeneticAlgorithm.Functions.Crossover
{
    public abstract class CrossoverOperator<TGene> : FunctionBase<TGene>, ICrossoverOperator<TGene>
    {
        public abstract void CrossOver(TGene[] offspring1, TGene[] offspring2);
    }

    public static class Crossover
    {
        public static ICrossoverOperator<TGene> SinglePoint<TGene>() => new SinglePointCrossover<TGene>();

        public static ICrossoverOperator<TGene> TwoPoint<TGene>() => new TwoPointCrossover<TGene>();

        public static ICrossoverOperator<TGene> PartiallyMatched<TGene>() => new PartiallyMatchedCrossover<TGene>();
    }
}