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
    public abstract class ObjectiveFunction< TGene >
    {
        #region Private instance fields

        int dimension;

        Objective objective;

        #endregion // Private insatnce fields

        #region Public insatnce properties

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

        #endregion // Public instance properties

        #region Protected instance constructors

        protected ObjectiveFunction( int dimension, Objective objective )
        {
            this.dimension = dimension;
            this.objective = objective;
        }

        #endregion // Protected instance constructors

        #region Public instance methods

        /// <summary>
        /// Evaluates the objective function.
        /// </summary>
        /// <param name="genes">The genes of the chromosome to evalaute.</param>
        /// <returns>
        /// The evaluation of the chromosome.
        /// </returns>
        public abstract double Evaluate( TGene[] genes );

        #endregion // Public instance methods
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
