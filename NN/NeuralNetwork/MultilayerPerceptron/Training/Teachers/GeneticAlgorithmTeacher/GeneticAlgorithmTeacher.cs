using System;

using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;


namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <remarks>
    /// A genetic algorithm teacher.
    /// </remarks>
    public class GeneticAlgorithmTeacher
        : TeacherBase
    {
        #region Private instance fields

        /// <summary>
        /// The network genetic algorithm.
        /// </summary>
        private NetworkGeneticAlgorithm networkGeneticAlgorithm;

        #endregion // Private insatnce fields

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
                return "GeneticAlgorithmTeacher";
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new genetic teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        public GeneticAlgorithmTeacher( TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet )
            : base( trainingSet, validationSet, testSet )
        {
            networkGeneticAlgorithm = new NetworkGeneticAlgorithm();
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
            // The network genetic algorithm parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction( network, trainingSet );
            int populationSize = 500;
            double crossoverRate = 0.8;
            double mutationRate = 0.05;
            bool scaling = false;

            // Train the network.
            int iterationCount;
            double networkError;
            double[] weights = networkGeneticAlgorithm.Run( networkObjectiveFunction,
                maxIterationCount, out iterationCount, maxTolerableNetworkError, out networkError,
                populationSize, crossoverRate, mutationRate, scaling
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
