using System.Collections.Generic;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainingStrategy
    {
        public BackpropagationTrainingStrategy(int maxIterationCount, double maxNetworkError, bool batchLearning, double learningRate, double momentum)
        {
            MaxIterationCount = maxIterationCount;
            MaxNetworkError = maxNetworkError;
            BatchLearning = batchLearning;
            LearningRate = learningRate;
            Momentum = momentum;
        }

        public int MaxIterationCount { get; }

        public double MaxNetworkError { get; }

        public bool BatchLearning { get; }

        public double LearningRate { get; }

        public double Momentum { get; }

        public virtual bool IsDone(int iterationCount, double networkError)
            => iterationCount >= MaxIterationCount || networkError <= MaxNetworkError;
    }
}
