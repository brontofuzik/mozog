using System;
using NeuralNetwork.Utils;

namespace NeuralNetwork.KohonenNetwork.LearningRateFunctions
{
    public abstract class AbstractLearningRateFunction
        : ILearningRateFunction
    {
        /// <summary>
        /// Calculates the learning rate for the specified trianing iteration.
        /// </summary>
        /// <param name="trainingIterationIndex">The trianing iteration.</param>
        /// <returns>The learning rate.</returns>
        public abstract double CalculateLearningRate(int trainingIterationIndex);

        /// <summary>
        /// Creates a new instance of the AbstractLearningRateFunction class.
        /// </summary>
        /// <param name="trainingIterationCount">The number of trianing iterations.</param>
        /// <param name="initialLearningRate">The initial learning rate.</param>
        /// <param name="finalLearningRate">The final learning rate.</param>
        protected AbstractLearningRateFunction(int trainingIterationCount, double initialLearningRate, double finalLearningRate)
        {
            Require.IsPositive(trainingIterationCount, nameof(trainingIterationCount));
            Require.IsWithinRange(initialLearningRate, nameof(initialLearningRate), MinLearningRate, MaxLearningRate);
            Require.IsWithinRange(finalLearningRate, nameof(finalLearningRate), MinLearningRate, MaxLearningRate);

            if (finalLearningRate > initialLearningRate)
            {
                throw new ArgumentException("The final learning rate must be less than or equal to the initial learning rate.", nameof(finalLearningRate));
            }

            _trainingIterationCount = trainingIterationCount;
            _initialLearningRate = initialLearningRate;
            _finalLearningRate = finalLearningRate;
        }

        /// <summary>
        /// Creates a new instance of the AbstractLearningRateFunction class.
        /// </summary>
        /// <param name="trainingIterationCount">The number of training iterations.</param>
        protected AbstractLearningRateFunction(int trainingIterationCount)
            : this(trainingIterationCount, MinLearningRate, MaxLearningRate)
        {
        }

        /// <summary>
        /// Gets the initial learning rate.
        /// </summary>
        /// <value>
        /// The initial learning rate.
        /// </value>
        protected double InitialLearningRate
        {
            get
            {
                return _initialLearningRate;
            }
        }

        /// <summary>
        /// Gets the final learning rate.
        /// </summary>
        /// <value>
        /// The final learning rate.
        /// </value>
        protected double FinalLearningRate
        {
            get
            {
                return _finalLearningRate;
            }
        }

        /// <summary>
        /// The minimum learning rate.
        /// </summary>
        protected static double MinLearningRate = 0.0;

        /// <summary>
        /// The maximum learning rate.
        /// </summary>
        protected static double MaxLearningRate = Double.MaxValue;

        /// <summary>
        /// The number of trianing iterations.
        /// </summary>
        private int _trainingIterationCount;

        /// <summary>
        /// The initial learning rate.
        /// </summary>
        private double _initialLearningRate;

        /// <summary>
        /// The final learning rate.
        /// </summary>
        private double _finalLearningRate;
    }
}
