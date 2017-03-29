using System;
using System.Collections.Generic;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase<BackpropagationArgs>
    {
        // The interval between two consecutive updates of the cumulative network error (CNR).
        const int cumulativeNetworkErrorUpdateInterval = 1;

        public BackpropagationTrainer(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override TrainingLog Train(INetwork network, BackpropagationArgs args)
        {
            var architecture = network.Architecture;
            BackpropagationNetwork backpropagationNetwork = new BackpropagationNetwork(architecture);

            var log = Train(backpropagationNetwork, args);

            var weights = backpropagationNetwork.GetWeights();
            network.SetWeights(weights);

            return log;
        }

        // Defaults
        public override TrainingLog Train(INetwork network, int maxIterations, double maxError)
            => Train(network, new BackpropagationArgs(maxIterations, maxError, learningRate: 0.01, momentum: 0.9, batchLearning: false));

        private TrainingLog Train(BackpropagationNetwork network, BackpropagationArgs args)
        {
            network.SetLearningRates(args.LearningRate);
            // TODO Momentum
            //backpropagationNetwork.SetConnectorMomenta(strategy.Momentum);

            network.Initialize();

            int iterationCount = 0;
            double networkError = network.CalculateError(TrainingSet);
            double cumulativeNetworkError = Double.MaxValue;

            while (!args.IsDone(iterationCount, cumulativeNetworkError))
            {
                TrainWithSet(network, TrainingSet.RandomPatterns, args.BatchLearning);
                iterationCount++;

                // Calculate the network error.
                networkError = network.CalculateError(TrainingSet);

                // Calculate the cumulative network error.
                int i = iterationCount % cumulativeNetworkErrorUpdateInterval;
                if (i == 0)
                {
                    i = cumulativeNetworkErrorUpdateInterval;
                }
                cumulativeNetworkError = network.Error / (double)i;

                // Reset the cumulative network error.
                if (iterationCount % cumulativeNetworkErrorUpdateInterval == 0)
                {
                    network.ResetError();
                }

                // DEBUG
                if (iterationCount % 10 == 0)
                {
                    Console.WriteLine("{0:D5} : {1:F2}, {2:F2}", iterationCount, networkError, cumulativeNetworkError);
                }
            }

            return new TrainingLog(iterationCount, networkError);
        }

        private void TrainWithSet(BackpropagationNetwork network, IEnumerable<SupervisedTrainingPattern> set, bool batch)
        {
            if (batch)
            {
                network.ResetPartialDerivatives();
            }

            set.ForEach(p => TrainWithPattern(network, p, batch));

            if (batch)
            {
                network.UpdateWeights();
                network.UpdateSynapseLearningRates();
            }
        }

        private void TrainWithPattern(BackpropagationNetwork network, SupervisedTrainingPattern pattern, bool batch)
        {
            if (!batch)
            {
                network.ResetPartialDerivatives();
            }

            network.Evaluate(pattern.InputVector);
            network.Backpropagate(pattern.OutputVector);

            network.UpdateError();
            network.UpdatePartialDerivatives();

            if (!batch)
            {
                network.UpdateWeights();
            }
        }
    }

    public class BackpropagationArgs : TrainingArgs
    {
        public BackpropagationArgs(int maxIterations, double maxError, double learningRate, double momentum, bool batchLearning)
            : base(maxIterations, maxError)
        {
            LearningRate = learningRate;
            Momentum = momentum;
            BatchLearning = batchLearning;
        }

        public double LearningRate { get; }

        public double Momentum { get; }

        public bool BatchLearning { get; }
    }
}