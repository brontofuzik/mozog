using System;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase
    {
        public BackpropagationTrainer(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override TrainingLog Train(INetwork network, int maxIterationCount, double maxNetworkError)
        {
            BackpropagationTrainingStrategy trainingStrategy = new BackpropagationTrainingStrategy(maxIterationCount, maxNetworkError, false, 0.01, 0.9);
            return Train(network, trainingStrategy);
        }

        public TrainingLog Train(INetwork network, BackpropagationTrainingStrategy strategy)
        {
            // The interval between two consecutive updates of the cumulative network error (CNR).
            int cumulativeNetworkErrorUpdateInterval = 1;

            // 1. Setup : Decorate the network as a backpropagation network.
            BackpropagationNetwork backpropagationNetwork = new BackpropagationNetwork(network);
            backpropagationNetwork.SetSynapseLearningRates(strategy.SynapseLearningRate);
            backpropagationNetwork.SetConnectorMomenta(strategy.ConnectorMomentum);

            // 2. Update the training strategy.
            strategy.BackpropagationNetwork = backpropagationNetwork;
            strategy.TrainingSet = trainingSet;

            // 3. Initialize the backpropagation network.
            backpropagationNetwork.Initialize();

            // 4. Train the backpropagation network while the stopping criterion is not met.
            int iterationCount = 0;
            double networkError = backpropagationNetwork.CalculateError(trainingSet);
            double cumulativeNetworkError = Double.MaxValue;

            while (!strategy.IsStoppingCriterionMet(iterationCount, cumulativeNetworkError))
            {
                // Train the backpropagation network on a training set.
                TrainWithSet(backpropagationNetwork, strategy);
                iterationCount++;

                // Calculate the network error.
                networkError = backpropagationNetwork.CalculateError(trainingSet);

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
                    Console.WriteLine("{0:D5} : {1:F2}, {2:F2}", iterationCount, networkError, cumulativeNetworkError);
                }
            }

            // 5. Teardown : Undecorate the backpropagation network as a network. 
            network = backpropagationNetwork.GetDecoratedNetwork();

            // 6. Create the trainig log.
            TrainingLog trainingLog = new TrainingLog(iterationCount, networkError);
            return trainingLog;
        }

        private void TrainWithSet(BackpropagationNetwork network, BackpropagationTrainingStrategy strategy)
        {
            // Bacth vs. incremental learning.
            if (strategy.BatchLearning)
            {
                network.ResetSynapsePartialDerivatives();
            }

            foreach (SupervisedTrainingPattern trainingPattern in strategy.TrainingPatterns)
            {
                TrainWithPattern(network, strategy, trainingPattern);
            }

            // Batch vs. incremental learning.
            if (strategy.BatchLearning)
            {
                network.UpdateSynapseWeights();
                network.UpdateSynapseLearningRates();
            }
        }

        private void TrainWithPattern(BackpropagationNetwork network, BackpropagationTrainingStrategy trainingStrategy, SupervisedTrainingPattern pattern)
        {
            // Batch vs. incremental learning.
            if (!trainingStrategy.BatchLearning)
            {
                network.ResetSynapsePartialDerivatives();
            }

            network.Evaluate(pattern.InputVector);
            network.Backpropagate(pattern.OutputVector);

            network.UpdateError();
            network.UpdateSynapsePartialDerivatives();

            // Batch vs. inremental learning.
            if (!trainingStrategy.BatchLearning)
            {
                network.UpdateSynapseWeights();
            }
        }
    }
}