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
    public abstract class ObjectiveFunction<TGene >
    {
        int dimension;

        Objective objective;

        protected ObjectiveFunction(int dimension, Objective objective)
        {
            this.dimension = dimension;
            this.objective = objective;
        }

        public int Dimension
        {
            get
            {
                return dimension;
            }
        }

        public Objective Objective
        {
            get
            {
                return objective;
            }
        }

        /// <summary>
        /// Evaluates the objective function.
        /// </summary>
        /// <param name="genes">The genes of the chromosome to evalaute.</param>
        /// <returns>
        /// The evaluation of the chromosome.
        /// </returns>
        public abstract double Evaluate(TGene[] genes);
    }

    /// <summary>
    /// The objective - minimize or maximize the objective function?
    /// </summary>
    public enum Objective
    {
        MINIMIZE,
        MAXIMIZE
    }
}
