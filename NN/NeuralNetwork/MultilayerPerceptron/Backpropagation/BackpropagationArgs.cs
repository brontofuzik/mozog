using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationArgs : TrainingArgs
    {
        public BackpropagationArgs(BackpropagationType type, double learningRate, double momentum, double maxError, int maxIterations, int resetInterval)
            : base(maxIterations, maxError)
        {
            Type = type;
            LearningRate = learningRate;
            Momentum = momentum;
            ResetInterval = resetInterval;
        }

        public static BackpropagationArgs Batch(double learningRate, double momentum, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, learningRate, momentum, maxError, maxIterations, resetInterval);

        public static BackpropagationArgs Stochastic(double learningRate, double momentum, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, learningRate, momentum, maxError, maxIterations, resetInterval);

        public BackpropagationType Type { get; }

        public double LearningRate { get; }

        public double Momentum { get; }

        public int ResetInterval { get; }
    }

    public enum BackpropagationType
    {
        Batch,
        MiniBatch, // Not supported
        Stochastic
    }
}