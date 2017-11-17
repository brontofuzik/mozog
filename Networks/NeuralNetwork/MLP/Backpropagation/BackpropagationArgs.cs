using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.MLP.Backpropagation
{
    public class BackpropagationArgs : TrainingArgs
    {
        #region Factories

        // Batch
        public static BackpropagationArgs Batch(Func<IOptimizer> optimizer, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, optimizer, maxError, maxIterations);

        // Bacth
        public static BackpropagationArgs Batch(double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, Optimizer.Default(learningRate), maxError, maxIterations);

        // Stochastic
        public static BackpropagationArgs Stochastic(Func<IOptimizer> optimizer, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, optimizer, maxError, maxIterations);

        // Stochastic
        public static BackpropagationArgs Stochastic(double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, Optimizer.Default(learningRate), maxError, maxIterations);

        #endregion // Factories

        public BackpropagationArgs(BackpropagationType type, Func<IOptimizer> optimizer, double maxError, int maxIterations)
            : base(maxIterations, maxError)
        {
            Type = type;
            OptimizerFactory = optimizer;
        }

        public BackpropagationType Type { get; }

        public Func<IOptimizer> OptimizerFactory { get; }
    }

    public enum BackpropagationType
    {
        Batch,
        MiniBatch, // Not supported
        Stochastic
    }
}