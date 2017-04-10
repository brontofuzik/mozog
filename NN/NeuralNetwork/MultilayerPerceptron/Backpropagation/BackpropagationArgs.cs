using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationArgs : TrainingArgs
    {
        #region Factories

        // Batch
        public static BackpropagationArgs Batch(Func<IOptimizer> optimizer, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, optimizer, maxError, maxIterations, resetInterval);

        // Bacth
        public static BackpropagationArgs Batch(double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, Optimizer.Default(learningRate), maxError, maxIterations, resetInterval);

        // Stochastic
        public static BackpropagationArgs Stochastic(Func<IOptimizer> optimizer, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, optimizer, maxError, maxIterations, resetInterval);

        // Stochastic
        public static BackpropagationArgs Stochastic(double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue, int resetInterval = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, Optimizer.Default(learningRate), maxError, maxIterations, resetInterval);

        #endregion // Factories

        public BackpropagationArgs(BackpropagationType type, Func<IOptimizer> optimizer, double maxError, int maxIterations, int resetInterval)
            : base(maxIterations, maxError)
        {
            Type = type;
            OptimizerFactory = optimizer;
            ResetInterval = resetInterval;
        }

        public BackpropagationType Type { get; }

        public Func<IOptimizer> OptimizerFactory { get; }

        public int ResetInterval { get; }
    }

    public enum BackpropagationType
    {
        Batch,
        MiniBatch, // Not supported
        Stochastic
    }
}