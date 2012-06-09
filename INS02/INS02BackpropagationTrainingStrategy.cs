using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;

using NeuralNetwork.MultilayerPerceptron.Training;
using NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher;

namespace INS02
{
    /// <summary>
    /// A INS02 backpropagation training strategy.
    /// </summary>
    class INS02BackpropagationTrainingStrategy
        : BackpropagationTrainingStrategy
    {
        #region Public members

        #region Public instance constructors

        /// <summary>
        /// Creates a new INS02 backpropagation training strategy.
        /// </summary>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxNetworkError">The maximum error of the network.</param>
        /// <param name="batchLearning">Bacth vs. incremental learning.</param>
        /// <param name="synapseLearningRate">The learning rate of the synapses.</param>
        /// <param name="connectorMomentum">The momentum of the connectors. </param>
        public INS02BackpropagationTrainingStrategy( int maxIterationCount, double maxNetworkError, bool batchLearning, double synapseLearningRate, double connectorMomentum )
            : base( maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum )
        {
            random = new Random();
        }

        #endregion // Public instance constructors

        #region Public instance properties

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable< SupervisedTrainingPattern > TrainingPatterns
        {
            get
            {
                foreach (SupervisedTrainingPattern trainingPattern in TrainingSet)
                {
                    yield return trainingPattern;
                    yield return MutateTrainingPattern( trainingPattern );
                }
            }
        }

        #endregion // Public instance constructors

        #endregion // Public members

        #region Private members

        #region Private instance methods

        /// <summary>
        /// Mutates a training pattern.
        /// </summary>
        /// <param name="trainigPattern">The training pattern to mutate.</param>
        /// <returns>
        /// The mutated training pattern.
        /// </returns>
        private SupervisedTrainingPattern MutateTrainingPattern( SupervisedTrainingPattern trainigPattern )
        {
            // Modify the keyword.
            string keyword = (string)trainigPattern.Tag;
            string mutatedKeyword;
            do
            {
                mutatedKeyword = Program.MutateKeyword( keyword );
            }
            while (((StringCollection)TrainingSet.Tag).Contains( mutatedKeyword ));

            // Create a new training pattern.
            double[] inputVector = Program.KeywordToVector( mutatedKeyword );
            double[] outputVector = Program.KeywordIndexToVector( -1 );
            return new SupervisedTrainingPattern( inputVector, outputVector, mutatedKeyword );
        }
        
        #endregion // Private instance methods

        #region Private instance fields

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        Random random;

        #endregion // Private instance fields

        #endregion // Private members
    }
}
