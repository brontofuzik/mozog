namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    // LR = ((LR_f - LR_i) / TIC) * TII + LR_i
    public class LinearLearningRateFunction
        : AbstractLearningRateFunction
    {
        #region Public members

        #region Instance constructors

        public LinearLearningRateFunction(int trainingIterationCount, double initialLearningRate, double finalLearningRate)
            : base(trainingIterationCount, initialLearningRate, finalLearningRate)
        {
            _learningRateParameter = ((finalLearningRate - initialLearningRate) / trainingIterationCount);
        }

        public LinearLearningRateFunction(int trainingIterationCount)
            : base(trainingIterationCount, AbstractLearningRateFunction.MinLearningRate, AbstractLearningRateFunction.MaxLearningRate)
        {
        }

        #endregion // Instance constructors

        #region Instance methods

        /// <summary>
        /// Calculates the learning rate for the specified trianing iteration.
        /// </summary>
        /// <param name="trainingIterationIndex">The trianing iteration.</param>
        /// <returns>The learning rate.</returns>
        public override double CalculateLearningRate(int trainingIterationIndex)
        {
            return InitialLearningRate + _learningRateParameter * trainingIterationIndex;
        }

        #endregion // Instance methods

        #endregion // Public members


        #region Private members

        #region Instance fields

        private double _learningRateParameter;

        #endregion // Instance fields

        #endregion // Private members
    }
}
