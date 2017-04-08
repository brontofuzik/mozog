﻿using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase<BackpropagationArgs>
    {
        // The interval between two consecutive updates of the cumulative network error (CNR).
        const int cumulativeNetworkErrorUpdateInterval = 1;

        public override TrainingLog Train(INetwork network, DataSet data, BackpropagationArgs args)
        {
            // Convert to backprop network
            var architecture = network.Architecture;
            var backpropNetwork = new BackpropagationNetwork(architecture);

            var log = Train(backpropNetwork, data, args);

            // Convert back to normal network
            var weights = backpropNetwork.GetWeights();
            network.SetWeights(weights);

            log.TrainingSetStats = Test(network, data);

            return log;
        }

        private TrainingLog Train(BackpropagationNetwork network, DataSet data, BackpropagationArgs args)
        {
            network.LearningRate = args.LearningRate;
            network.Momentum = args.Momentum;

            network.Initialize();

            double networkError = 0.0;

            int iterationCount = 0;
            double cumulativeNetworkError = Double.MaxValue;
            while (!args.IsDone(iterationCount, cumulativeNetworkError))
            {
                TrainWithSet(network, data, args.Type == BackpropagationType.Batch);
                iterationCount++;

                // Calculate the network error.
                networkError = network.CalculateError(data);

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

            return new TrainingLog(iterationCount);
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

            network.UpdateError(point.Output); // Network
            network.UpdatePartialDerivatives(); // Synapses

            if (!batch)
            {
                // Synapses
                network.UpdateWeights();
            }
        }

        public override DataStatistics Test(INetwork network, DataSet data)
        {
            var error = 0.0;
            var rss = 0.0;

            foreach (var point in data)
            {
                var result = network.Evaluate(point.Input, point.Output);
                error += result.error;
                rss += Math.Pow(result.output[0] - point.Output[0], 2);
            }

            return new DataStatistics(n: data.Size, p: network.SynapseCount)
            {
                Error = error,
                RSS = rss
            };
        }
    }

    public class BackpropagationArgs : TrainingArgs
    {
        public BackpropagationArgs(int maxIterations, double maxError, BackpropagationType type, double learningRate, double momentum)
            : base(maxIterations, maxError)
        {
            Type = type;
            LearningRate = learningRate;
            Momentum = momentum;

        }

        public BackpropagationType Type { get; }

        public double LearningRate { get; }

        public double Momentum { get; }
    }

    public enum BackpropagationType
    {
        Batch,
        MiniBatch, // Not supported
        Stochastic
    }
}