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
    public class ObjectiveFunction<TGene>
    {
        public static ObjectiveFunction<TGene> Maximize(ObjectiveFunc<TGene> function)
            => new ObjectiveFunction<TGene>(Objective.Maximize, function);

        public static ObjectiveFunction<TGene> Minimize(ObjectiveFunc<TGene> function)
            => new ObjectiveFunction<TGene>(Objective.Minimize, function);

        private ObjectiveFunction(Objective objective, ObjectiveFunc<TGene> function)
        {
            Objective = objective;
            Function = function;
        }

        public Objective Objective { get; }
        public ObjectiveFunc<TGene> Function { get; }

        private bool Maximizing => Objective == Objective.Maximize;
        private bool Minimizing => Objective == Objective.Minimize;

        public double Evaluate(Chromosome<TGene> chromosome) => Function(chromosome.Genes);

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
            => Maximizing ? evaluation >= acceptableEvaluation : evaluation <= acceptableEvaluation;
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
