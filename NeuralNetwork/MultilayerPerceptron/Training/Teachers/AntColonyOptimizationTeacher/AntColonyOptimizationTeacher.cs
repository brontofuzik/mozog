using System;

using NeuralNetwork.MultilayerPerceptron.Networks;

using AntColonyOptimization;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.AntColonyOptimizationTeacher
{
    /// <summary>
    /// An ant colony optimization teacher.
    /// </summary>
    public class AntColonyOptimizationTeacher
        : TeacherBase
    {
        #region Private instance fields

        /// <summary>
        /// The ant colony optimization;
        /// </summary>
        private NetworkAntColonyOptimization networkAntColonyOptimization;

        #endregion // private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the name of the teacher.
        /// </summary>
        /// <value>
        /// The name of the teacher.
        /// </value>
        public override string Name
        {
            get
            {
                return "AntColonyOptimizationTeacher";
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new ant colony teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        public AntColonyOptimizationTeacher( TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet )
            : base( trainingSet, validationSet, testSet )
        {
            networkAntColonyOptimization = new NetworkAntColonyOptimization();
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        public override TrainingLog Train( INetwork network, int maxIterationCount, double maxTolerableNetworkError )
        {
            // The network ant colony optimiaztion parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction( network, trainingSet );
            int antCount = 100;
            int normalPDFCount = 10;
            double requiredAccuracy = 0.001;

            // Train the network.
            int iterationCount;
            double networkError;
            double[] weights = networkAntColonyOptimization.Run( networkObjectiveFunction,
                maxIterationCount, out iterationCount, maxTolerableNetworkError, out networkError,
                antCount, normalPDFCount, requiredAccuracy
            );
            network.SetWeights( weights );

            // LOGGING
            // -------

            // Create the training log and log the training data.
            TrainingLog trainingLog = new TrainingLog( iterationCount, networkError );

            // Log the network statistics.
            LogNetworkStatistics( trainingLog, network );

            return trainingLog;
        }

        #endregion // Public instance methods
    }
}
