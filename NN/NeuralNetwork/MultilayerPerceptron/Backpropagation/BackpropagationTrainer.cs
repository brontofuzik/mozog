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

        public BackpropagationTrainer(DataSet trainingSet, DataSet validationSet, DataSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override TrainingLog Train(INetwork network, BackpropagationArgs args)
        {
            var architecture = network.Architecture;
            var backpropNetwork = new BackpropagationNetwork(architecture);

            var log = Train(backpropNetwork, args);

            var weights = backpropNetwork.GetWeights();
            network.SetWeights(weights);

            return log;
        }

        // Defaults
        public override TrainingLog Train(INetwork network, int maxIterations, double maxError)
            => Train(network, new BackpropagationArgs(maxIterations, maxError, learningRate: 0.01, momentum: 0.9, batchLearning: false));

        private TrainingLog Train(BackpropagationNetwork network, BackpropagationArgs args)
        {
            network.SetLearningRate(args.LearningRate);
            network.SetMomentum(args.Momentum);

            network.Initialize();

            double networkError = 0.0;

            int iterationCount = 0;
            double cumulativeNetworkError = Double.MaxValue;
            while (!args.IsDone(iterationCount, cumulativeNetworkError))
            {
                TrainWithSet(network, TrainingSet, args.BatchLearning);
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
                    Console.WriteLine($"{iterationCount:D5}: {networkError:F2}, {cumulativeNetworkError:F2}");
                }
            }

            return new TrainingLog(iterationCount, networkError);
        }

        private void TrainWithSet(BackpropagationNetwork network, DataSet data, bool batch)
        {
            if (batch)
            {
                // Synapses
                network.ResetPartialDerivatives();
            }

            data.ForEach(p => TrainWithPoint(network, p, batch));

            if (batch)
            {
                // Synapses
                network.UpdateWeights();
                network.UpdateLearningRates();
            }
        }

        private void TrainWithPoint(BackpropagationNetwork network, LabeledDataPoint point, bool batch)
        {
            if (!batch)
            {
                // Synapses
                network.ResetPartialDerivatives();
            }

            network.Evaluate(point.Input); // Neurons
            network.Backpropagate(point.Output); // Neurons

            network.UpdateError(); // Network
            network.UpdatePartialDerivatives(); // Synapses

            if (!batch)
            {
                // Synapses
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