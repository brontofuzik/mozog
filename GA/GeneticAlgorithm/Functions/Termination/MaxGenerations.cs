namespace GeneticAlgorithm.Functions.Termination
{
    public class MaxGenerations<TGene> : TerminationFunction<TGene>
    {
        private readonly int maxGenerations;

        public MaxGenerations(int maxGenerations)
        {
            this.maxGenerations = maxGenerations;
        }

        public override bool ShouldTerminate() => Algo.CurrentGeneration >= maxGenerations;
    }
}
