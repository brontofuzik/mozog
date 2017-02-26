namespace GeneticAlgorithm.Functions.Selection
{
    public abstract class SelectionFunction<TGene> : FunctionBase<TGene>, ISelectionFunction<TGene>
    {
        protected Population<TGene> Population { get; set; }

        public abstract void Initialize(Population<TGene> population);

        public abstract Chromosome<TGene> Select();
    }
}