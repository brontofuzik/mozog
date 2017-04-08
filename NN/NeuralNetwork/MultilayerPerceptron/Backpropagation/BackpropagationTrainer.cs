using System;
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

            int iterations = 0;
            double error;
            do
            {
                error = TrainIteration(network, data, args);
                iterations++;

                // DEBUG
                if (iterations % 100 == 0)
                {
                    Console.WriteLine($"{iterations:D5}: {error:F2}");
                }
            }
            while (!args.IsDone(iterations, error));

            return new TrainingLog(iterations);
        }

        private double TrainIteration(BackpropagationNetwork network, DataSet data, BackpropagationArgs args)
        {
            if (args.Type == BackpropagationType.Batch)
            {
                return TrainWithBatch(network, data);
            }
            else if (args.Type == BackpropagationType.Stochastic)
            {
                return data.Random().Sum(p => TrainWithPoint(network, p));
            }
            else
            {
                return 0.0;
            }
        }

        private double TrainWithBatch(BackpropagationNetwork network, IEnumerable<LabeledDataPoint> batch)
        {
            network.ResetPartialDerivatives();

            var error = batch.Sum(p => TrainWithPoint(network, p));

            network.UpdateWeights();
            network.UpdateLearningRates();

            return error;
        }

        private double TrainWithPoint(BackpropagationNetwork network, LabeledDataPoint point)
        {
            var result = network.Evaluate(point.Input, point.Output);
            network.Backpropagate(point.Output);

            // TODO Training
            network.UpdateError(point.Output); // Network
            network.UpdatePartialDerivatives(); // Synapses

            return result.error;
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