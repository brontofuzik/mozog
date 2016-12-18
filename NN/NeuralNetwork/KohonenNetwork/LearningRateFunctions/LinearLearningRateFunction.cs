namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    // LR = ((LR_f - LR_i) / TIC) * TII + LR_i
    public class LinearLearningRateFunction
        : AbstractLearningRateFunction
    {
        public LinearLearningRateFunction(int trainingIterationCount, double initialLearningRate, double finalLearningRate)
            : base(trainingIterationCount, initialLearningRate, finalLearningRate)
        {
            _learningRateParameter = (finalLearningRate - initialLearningRate) / trainingIterationCount;
        }

        public LinearLearningRateFunction(int trainingIterationCount)
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
            return InitialLearningRate + _learningRateParameter * trainingIterationIndex;
        }

        private double _learningRateParameter;
    }
}
