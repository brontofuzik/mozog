using System;
using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Training;
using NeuralNetwork.MultilayerPerceptron.Training.Backpropagation;

namespace NeuralNetwork.Examples.MultilayerPerceptron.INS01
{
    /// <summary>
    /// A INS01 backpropagation training strategy.
    /// </summary>
    class INS01BackpropagationTrainingStrategy
        : BackpropagationTrainingStrategy
    {
        /// <summary>
        /// Creates a new INS01 backpropagation learning strategy.
        /// </summary>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxNetworkError">The maximum error of the network.</param>
        /// <param name="batchLearning">Batch vs. incremental learning.</param>
        /// <param name="synapseLearningRate">The learning rate of the synapses.</param>
        /// <param name="connectorMomentum">The momentum of the connectors.</param>
        public INS01BackpropagationTrainingStrategy(int maxIterationCount, double maxNetworkError, bool batchLearning, double synapseLearningRate, double connectorMomentum)
            : base(maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override IEnumerable< SupervisedTrainingPattern > TrainingPatterns
        {
            get
            {
                // For each tile quadruple in the training set ...
                for (int i = 0; i < TrainingSet.Size; i += 4)
                {
                    double minNetworkError = Double.MaxValue;
                    int trainingPatternIndex = -1;

                    // For each tile orientation in the quadruple ...
                    for (int j = i; j < i + 4; j++)
                    {
                        SupervisedTrainingPattern trainingPattern = TrainingSet[j];
                        double networkError = BackpropagationNetwork.CalculateError(trainingPattern);
                        if (networkError < minNetworkError)
                        {
                            minNetworkError = networkError;
                            trainingPatternIndex = j;
                        }
                    }

                    // Pick the one yielding the least network error.
                    yield return TrainingSet[trainingPatternIndex];
                }
            }
        }
    }
}
