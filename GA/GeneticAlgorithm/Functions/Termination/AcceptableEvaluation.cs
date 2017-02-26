namespace GeneticAlgorithm.Functions.Termination
{
    public class AcceptableEvaluation<TGene> : TerminationFunction<TGene>
    {
        private readonly double acceptableEvaluation;

        public AcceptableEvaluation(double acceptableEvaluation)
        {
            this.acceptableEvaluation = acceptableEvaluation;
        }

        public override bool ShouldTerminate() => Algo.Fitness.IsAcceptable(Algo.BestEvaluation, acceptableEvaluation);
    }
}
