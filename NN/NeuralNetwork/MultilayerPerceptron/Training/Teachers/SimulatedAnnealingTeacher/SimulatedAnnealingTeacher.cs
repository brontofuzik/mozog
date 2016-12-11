using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.SimulatedAnnealingTeacher
{
    /// <summary>
    /// A simulated annealing teacher.
    /// </summary>
    public class SimulatedAnnealingTeacher
        : TeacherBase
    {
        #region Private instance fields

        /// <summary>
        /// The network simulated annealing.
        /// </summary>
        private NetworkSimlatedAnnealing networkSimulatedAnnealing;

        #endregion // Private instance methods

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
                return "SimulatedAnnealingTeacher";
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new simulated annealing teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validationSet.</param>
        /// <param name="testSet">The test set.</param>
        public SimulatedAnnealingTeacher( TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet )
            : base( trainingSet, validationSet, testSet )
        {
            networkSimulatedAnnealing = new NetworkSimlatedAnnealing();
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
            // The network simulated annealing parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction( network, trainingSet );
            double initialTemperature = 1000.0;
            double finalTemperature = 0.001;

            // Train the network.
            int iterationCount;
            double networkError;
            double[] weights = networkSimulatedAnnealing.Run( networkObjectiveFunction,
                maxIterationCount, out iterationCount, maxTolerableNetworkError, out networkError,
                initialTemperature, finalTemperature 
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
