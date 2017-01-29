namespace GeneticAlgorithm
{
    /// <remarks>
    /// <para>
    /// The objective function.
    /// </para>
    /// <para>
    /// The <i>evaluation function</i>, or <i>objective function</i>, provides a measure of performance
    /// with respect to a particular set of parameters.
    /// </para>
    /// <para>
    /// The evaluation of a string representing a set of parameters is independent of the evaluation of any other string.
    /// </para>
    /// </remarks>
    /// <typeparam name="TGene">The type of the gene.</typeparam>
    public abstract class ObjectiveFunction<TGene>
    {
        protected ObjectiveFunction(Objective objective)
        {
            Objective = objective;
        }

        public Objective Objective { get; }

        private bool Maximizing => Objective == Objective.Maximize;
        private bool Minimizing => Objective == Objective.Minimize;

        /// <summary>
        /// Evaluates the objective function.
        /// </summary>
        /// <param name="genes">The genes of the chromosome to evalaute.</param>
        /// <returns>
        /// The evaluation of the chromosome.
        /// </returns>
        public abstract double Evaluate(TGene[] genes);

        internal void UpdateBestChromosome(ref Chromosome<TGene> bestChromosome, Chromosome<TGene> chromosome)
        {
            if (bestChromosome == null
                || Maximizing && chromosome.Evaluation > bestChromosome.Evaluation
                || Minimizing && chromosome.Evaluation < bestChromosome.Evaluation)
            {
                bestChromosome = chromosome;
            }
        }

        internal void UpdateWorstChromosome(ref Chromosome<TGene> worstChromosome, Chromosome<TGene> chromosome)
        {
            if (worstChromosome == null
                || Maximizing && chromosome.Evaluation < worstChromosome.Evaluation
                || Minimizing && chromosome.Evaluation > worstChromosome.Evaluation)
            {
                worstChromosome = chromosome;
            }
        }

        public bool IsAcceptable(double evaluation, double acceptableEvaluation)
            => Objective == Objective.Maximize ? evaluation >= acceptableEvaluation : evaluation <= acceptableEvaluation;
    }

    /// <summary>
    /// The objective - minimize or maximize the objective function?
    /// </summary>
    public enum Objective
    {
        Minimize,
        Maximize
    }
}
