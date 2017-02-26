namespace GeneticAlgorithm.Functions.Termination
{
    public class MinEvaluation<TGene> : TerminationFunction<TGene>
    {
        private readonly double minEvaluation;

        public MinEvaluation(double minEvaluation)
        {
            this.minEvaluation = minEvaluation;
        }

        public override bool ShouldTerminate() => Algo.Fitness.IsAcceptable(Algo.BestEvaluation, minEvaluation);
    }
}
