﻿using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase<BackpropagationArgs>
    {
        public override TrainingLog Train(INetwork network, DataSet data, BackpropagationArgs args)
        {
            // Convert to backprop network
            var architecture = network.Architecture;
            var backpropNetwork = new BackpropagationNetwork(architecture);

            var log = TrainBackprop(backpropNetwork, data, args);

            // Convert back to normal network
            var weights = backpropNetwork.GetWeights();
            network.SetWeights(weights);

            log.TrainingSetStats = Test(network, data);

            return log;
        }

        private TrainingLog TrainBackprop(BackpropagationNetwork network, DataSet data, BackpropagationArgs args)
        {
            network.Initialize(args);

            double error = Double.MaxValue;
            int iterations = 0;      
            do
            {
                error = TrainIteration(network, data, args);
                iterations++;

                TrainingProgress?.Invoke(this, new TrainingStatus(iterations, error));
            }
            while (!args.IsDone(error, iterations));

            return new TrainingLog(iterations);
        }

        private double TrainIteration(BackpropagationNetwork network, DataSet data, BackpropagationArgs args)
        {
            if (args.Type == BackpropagationType.Batch)
            {
                return TrainBatch(network, data);
            }
            else if (args.Type == BackpropagationType.Stochastic)
            {
                return data.Random().Sum(p => TrainPoint(network, p));
            }
            else
            {
                return 0.0;
            }
        }

        private double TrainBatch(BackpropagationNetwork network, IEnumerable<LabeledDataPoint> batch)
        {
            network.ResetPartialDerivatives(); // Synapses

            var error = batch.Sum(p => TrainPoint(network, p));

            network.UpdateWeights();
            network.UpdateLearningRates(); // Synapses

            return error;
        }

        private double TrainPoint(BackpropagationNetwork network, LabeledDataPoint point)
        {
            var result = network.Evaluate(point.Input, point.Output);
            network.Backpropagate(point.Output); // Neurons
            network.UpdatePartialDerivatives(); // Synapses

            return result.error;
        }

        public override event EventHandler<TrainingStatus> TrainingProgress;
    }

    public class BackpropagationArgs : TrainingArgs
    {
        public BackpropagationArgs(BackpropagationType type, double learningRate, double momentum, double maxError, int maxIterations)
            : base(maxIterations, maxError)
        {
            Type = type;
            LearningRate = learningRate;
            Momentum = momentum;
        }

        public static BackpropagationArgs Batch(double learningRate, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Batch, learningRate, 0.0, maxError, maxIterations);

        public static BackpropagationArgs Stochastic(double learningRate, double momentum, double maxError = 0.0, int maxIterations = Int32.MaxValue)
            => new BackpropagationArgs(BackpropagationType.Stochastic, learningRate, momentum, maxError, maxIterations);

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