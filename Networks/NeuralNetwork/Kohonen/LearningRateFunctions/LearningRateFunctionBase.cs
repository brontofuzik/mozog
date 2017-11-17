using System;
using Mozog.Utils;

namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    public abstract class LearningRateFunctionBase : ILearningRateFunction
    {
        protected const double MinLearningRate = 0.0;
        protected const double MaxLearningRate = Double.MaxValue;

        private int iterations;

        protected LearningRateFunctionBase(int iterations, double initialRate, double finalRate)
        {
            Require.IsPositive(iterations, nameof(iterations));
            Require.IsWithinRange(initialRate, nameof(initialRate), MinLearningRate, MaxLearningRate);
            Require.IsWithinRange(finalRate, nameof(finalRate), MinLearningRate, MaxLearningRate);

            if (finalRate > initialRate)
                throw new ArgumentException("The final learning rate must be less than or equal to the initial learning rate.", nameof(finalRate));

            this.iterations = iterations;
            InitialRate = initialRate;
            FinalRate = finalRate;
        }

        protected LearningRateFunctionBase(int iterations)
            : this(iterations, MinLearningRate, MaxLearningRate)
        {
        }

        protected double InitialRate { get; }

        protected double FinalRate { get; }

        public abstract double Evaluate(int iteration);
    }
}
