namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    public interface ILearningRateFunction
    {
        /// <summary>
        /// Calculates the learning rate for the specified trianing iteration.
        /// </summary>
        /// <param name="trainingIterationIndex">The trianing iteration.</param>
        /// <returns>The learning rate.</returns>
        double CalculateLearningRate(int trainingIterationIndex);
    }
}
