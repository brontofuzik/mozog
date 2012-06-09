using System;

namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    public interface ILearningRateFunction
    {
        #region Methods

        /// <summary>
        /// Calculates the learning rate for the specified trianing iteration.
        /// </summary>
        /// <param name="trainingIterationIndex">The trianing iteration.</param>
        /// <returns>The learning rate.</returns>
        double CalculateLearningRate(int trainingIterationIndex);

        #endregion // Methods
    }
}
