using System;

namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    // LR = LR_i * ((LR_f / LR_i)^(1/TIC))^TII
    public class ExponentialLearningRateFunction
        : AbstractLearningRateFunction
    {
        public ExponentialLearningRateFunction(int trainingIterationCount, double initialLearningRate, double finalLearningRate)
            : base(trainingIterationCount, initialLearningRate, finalLearningRate)
        {
            _learningRateParameter = Math.Pow(finalLearningRate / initialLearningRate, 1 / (double)trainingIterationCount);
        }

        public ExponentialLearningRateFunction(int trainingIterationCount)
            : base(trainingIterationCount, MinLearningRate, MaxLearningRate)
        {
        }

        /// <summary>
        /// Calculates the learning rate for the specified trianing iteration.
        /// </summary>
        /// <param name="trainingIterationIndex">The trianing iteration.</param>
        /// <returns>The learning rate.</returns>
        public override double CalculateLearningRate(int trainingIterationIndex)
        {
            return InitialLearningRate * Math.Pow(_learningRateParameter, trainingIterationIndex);
        }

        private double _learningRateParameter;
    }
}
