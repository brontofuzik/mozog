using System;

namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    public class ExponentialDecayFunction : ILearningRateFunction
    {
        private const double SampleRate = 1e6;

        private readonly double initialRate;
        private readonly double decayRate;

        public ExponentialDecayFunction(double initialRate, double finalRate)
        {
            this.initialRate = initialRate;
            decayRate = Math.Pow(finalRate / initialRate, 1 / SampleRate);
        }

        public double Evaluate(double progress)
            => initialRate * Math.Pow(decayRate, progress * SampleRate);
    }
}
