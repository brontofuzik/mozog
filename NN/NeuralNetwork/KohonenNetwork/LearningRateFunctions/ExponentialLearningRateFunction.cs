using System;

namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    // LR = LR_i * ((LR_f / LR_i)^(1/TIC))^TII
    public class ExponentialLearningRateFunction
        : AbstractLearningRateFunction
    {
        #region Public members

        #region Instance constructors

        public ExponentialLearningRateFunction(int trainingIterationCount, double initialLearningRate, double finalLearningRate)
            : base(trainingIterationCount, initialLearningRate, finalLearningRate)
        {
            _learningRateParameter = Math.Pow((finalLearningRate / initialLearningRate), 1 / (double)trainingIterationCount);
        }

        public ExponentialLearningRateFunction(int trainingIterationCount)
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
            return InitialLearningRate * Math.Pow(_learningRateParameter, trainingIterationIndex);
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
