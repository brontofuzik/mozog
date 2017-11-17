using System;

namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    // LR = LR_i * ((LR_f / LR_i)^(1/TIC))^TII
    public class ExponentialLearningRateFunction : LearningRateFunctionBase
    {
        private readonly double learningRate;

        public ExponentialLearningRateFunction(int iterations, double initialRate, double finalRate)
            : base(iterations, initialRate, finalRate)
        {
            learningRate = Math.Pow(finalRate / initialRate, 1 / (double)iterations);
        }

        public ExponentialLearningRateFunction(int iterations)
            : base(iterations, MinLearningRate, MaxLearningRate)
        {
        }

        public override double Evaluate(int iteration)
            => InitialRate * Math.Pow(learningRate, iteration);
    }
}
