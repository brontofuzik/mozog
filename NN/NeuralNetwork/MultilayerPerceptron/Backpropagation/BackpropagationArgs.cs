using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationArgs : TrainingArgs
    {
        public BackpropagationArgs(BackpropagationType type, Func<IOptimizer> optimizer, double learningRate, double maxError, int maxIterations, int resetInterval)
            : base(maxIterations, maxError)
        {
            Type = type;
            OptimizerFactory = optimizer;
            LearningRate = learningRate;
            ResetInterval = resetInterval;
        }

        public static BackpropagationArgs Batch(Func<IOptimizer> optimizer, double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, optimizer, learningRate, maxError, maxIterations, resetInterval);

        public static BackpropagationArgs Stochastic(Func<IOptimizer> optimizer, double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, optimizer, learningRate, maxError, maxIterations, resetInterval);

        public BackpropagationType Type { get; }

        public Func<IOptimizer> OptimizerFactory { get; }

        public double LearningRate { get; }   

        public int ResetInterval { get; }
    }

    public enum BackpropagationType
    {
        Batch,
        MiniBatch, // Not supported
        Stochastic
    }
}