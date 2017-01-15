using System;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers
{
    /// <summary>
    /// A teacher.
    /// </summary>
    public abstract class TeacherBase
    {
        /// <summary>
        /// Gets the name of the teacher.
        /// </summary>
        /// <value>
        /// The name of the teacher.
        /// </value>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the training set.
        /// </summary>
        /// <value>
        /// The training set.
        /// </value>
        public TrainingSet TrainingSet
        {
            get
            {
                return trainingSet;
            }
        }

        /// <summary>
        /// Gets the validation set.
        /// </summary>
        /// <value>
        /// The validation set.
        /// </value>
        public TrainingSet ValidationSet
        {
            get
            {
                return validationSet;
            }
        }

        /// <summary>
        /// Gets the test set.
        /// </summary>
        /// <value>
        /// The test set.
        /// </value>
        public TrainingSet TestSet
        {
            get
            {
                return testSet;
            }
        }

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximal number of iterations.</param>
        /// <param name="maxNetworkError">The maximal network error.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        public abstract TrainingLog Train(INetwork network, int maxIterationCount, double maxNetworkError);

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximal number of iterations.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        public TrainingLog Train(INetwork network, int maxIterationCount)
        {
            return Train(network, maxIterationCount, 0);
        }

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxTolerableNetworkError">The maximal network error.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        public TrainingLog Train(INetwork network, double maxNetworkError)
        {
            return Train(network, Int32.MaxValue, maxNetworkError);
        }

        /// <summary>
        /// Logs the network statistics (the measures of fit and the forecast accuracy).
        /// </summary>
        /// <param name="trainingLog">The training log.</param>
        /// <param name="network">The network whose statistics are to be logged.</param>
        public void LogNetworkStatistics(TrainingLog trainingLog, INetwork network)
        {
            // Calculate and log the measures of fit.
            trainingLog.CalculateMeasuresOfFit(network, trainingSet);

            // Calculate and log the forecast accuracy.
            if (testSet != null)
            {
                trainingLog.CalculateForecastAccuracy(network, testSet);
            }
        }

        /// <summary>
        /// Creates a new teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        protected TeacherBase(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
        {
            // Vdalite the training set.
            Require.IsNotNull(trainingSet, "trainingSet");
            this.trainingSet = trainingSet;

            // The validation and test sets need not be provided.
            this.validationSet = validationSet;
            this.testSet = testSet;
        }

        /// <summary>
        /// The training set.
        /// </summary>
        protected TrainingSet trainingSet;

        /// <summary>
        /// The validation set.
        /// </summary>
        protected TrainingSet validationSet;

        /// <summary>
        /// The test set.
        /// </summary>
        protected TrainingSet testSet;
    }
}
