using System;
using System.IO;

using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher
{
    /// <summary>
    /// A backpropagation teacher.
    /// </summary>
    public class BackpropagationTeacher
        : TeacherBase
    {
        #region Public members

        #region Public instance constructors

        /// <summary>
        /// Creates a new backpropagation teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        public BackpropagationTeacher( TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet )
            : base( trainingSet, validationSet, testSet )
        {
        }

        #endregion // Public instance constructors
      
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
                return "BackpropagationTeacher";
            }
        }

        #endregion // Public instance properties

        #region Public instance methods

        /// <summary>
        /// Trains a network using the specified training strategy.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="trainingStrategy">The training strategy to use.</param>
        /// <returns>
        /// The training log.
        /// </returns>
        public TrainingLog Train( INetwork network, BackpropagationTrainingStrategy trainingStrategy )
        {
            // The interval between two consecutive updates of the cumulative network error (CNR).
            int cumulativeNetworkErrorUpdateInterval = 1;

            // 1. Setup : Decorate the network as a backpropagation network.
            BackpropagationNetwork backpropagationNetwork = new BackpropagationNetwork( network );
            backpropagationNetwork.SetSynapseLearningRates( trainingStrategy.SynapseLearningRate );
            backpropagationNetwork.SetConnectorMomenta( trainingStrategy.ConnectorMomentum );

            // 2. Update the training strategy.
            trainingStrategy.BackpropagationNetwork = backpropagationNetwork;
            trainingStrategy.TrainingSet = trainingSet;

            // 3. Initialize the backpropagation network.
            backpropagationNetwork.Initialize();

            // 4. Train the backpropagation network while the stopping criterion is not met.
            int iterationCount = 0;
            double networkError = backpropagationNetwork.CalculateError( trainingSet );
            double cumulativeNetworkError = Double.MaxValue;

            while (!trainingStrategy.IsStoppingCriterionMet( iterationCount, cumulativeNetworkError ))
            {
                // Train the backpropagation network on a training set.
                TrainOnTrainingSet( backpropagationNetwork, trainingStrategy );
                iterationCount++;

                // Calculate the network error.
                networkError = backpropagationNetwork.CalculateError( trainingSet );

                // Calculate the cumulative network error.
                int i = iterationCount % cumulativeNetworkErrorUpdateInterval;
                if (i == 0)
                {
                    i = cumulativeNetworkErrorUpdateInterval;
                }
                cumulativeNetworkError = backpropagationNetwork.Error / (double)i;

                // Reset the cumulative network error.
                if (iterationCount % cumulativeNetworkErrorUpdateInterval == 0)
                {
                    backpropagationNetwork.ResetError();
                }

                // DEBUG
                if (iterationCount % 10 == 0)
                {
                    Console.WriteLine( "{0:D5} : {1:F2}, {2:F2}", iterationCount, networkError, cumulativeNetworkError );
                }
            }

            // 5. Teardown : Undecorate the backpropagation network as a network. 
            network = backpropagationNetwork.GetDecoratedNetwork();

            // 6. Create the trainig log.
            TrainingLog trainingLog = new TrainingLog( iterationCount, networkError );
            return trainingLog;
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
        public override TrainingLog Train( INetwork network, int maxIterationCount, double maxNetworkError )
        {
            BackpropagationTrainingStrategy trainingStrategy = new BackpropagationTrainingStrategy( maxIterationCount, maxNetworkError, false, 0.01, 0.9 );
            return Train( network, trainingStrategy );
        }

        #endregion // Public instance methods

        #endregion // Public members

        #region Private memebers

        #region Private instance methods

        /// <summary>
        /// Trains the backpropagation network on the training set using the specified training strategy.
        /// </summary>
        /// <param name="backpropagationNetwork">The backpropagation network to train.</param>
        /// <param name="trainingStrategy">The training strategy to use.</param>
        private void TrainOnTrainingSet( BackpropagationNetwork backpropagationNetwork, BackpropagationTrainingStrategy trainingStrategy )
        {
            // Bacth vs. incremental learning.
            if (trainingStrategy.BatchLearning)
            {
                backpropagationNetwork.ResetSynapsePartialDerivatives();
            }

            foreach (SupervisedTrainingPattern trainingPattern in trainingStrategy.TrainingPatterns)
            {
                TrainOnTrainingPattern( backpropagationNetwork, trainingStrategy, trainingPattern );
            }

            // Batch vs. incremental learning.
            if (trainingStrategy.BatchLearning)
            {
                backpropagationNetwork.UpdateSynapseWeights();
                backpropagationNetwork.UpdateSynapseLearningRates();
            }
        }

        /// <summary>
        /// Trains the backpropagation network on the training pattern.
        /// </summary>
        /// <param name="backpropagationNetwork"></param>
        /// <param name="trainingPattern"></param>
        private void TrainOnTrainingPattern( BackpropagationNetwork backpropagationNetwork, BackpropagationTrainingStrategy trainingStrategy, SupervisedTrainingPattern trainingPattern )
        {
            // Batch vs. incremental learning.
            if (!trainingStrategy.BatchLearning)
            {
                backpropagationNetwork.ResetSynapsePartialDerivatives();
            }

            backpropagationNetwork.Evaluate( trainingPattern.InputVector );
            backpropagationNetwork.Backpropagate( trainingPattern.OutputVector );

            backpropagationNetwork.UpdateError();
            backpropagationNetwork.UpdateSynapsePartialDerivatives();

            // Batch vs. inremental learning.
            if (!trainingStrategy.BatchLearning)
            {
                backpropagationNetwork.UpdateSynapseWeights();
            }
        }

        #endregion // Private instance methods

        #endregion // Private members
    }
}