using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase<BackpropagationArgs>
    {
        private BackpropagationNetwork backpropNetwork;

        public override event EventHandler<TrainingStatus> WeightsUpdated;

        public override event EventHandler WeightsReset;

        public override TrainingLog Train(INetwork network, IDataSet data, BackpropagationArgs args)
        {
            // Convert to backprop network
            var architecture = network.Architecture;
            backpropNetwork = new BackpropagationNetwork(architecture);

            var log = TrainBackprop( data, args);

            // Convert back to normal network
            var weights = backpropNetwork.GetWeights();
            network.SetWeights(weights);

            log.TrainingSetStats = TestBasic(network, data);

            return log;
        }

        private TrainingLog TrainBackprop(IDataSet data, BackpropagationArgs args)
        {
            backpropNetwork.Initialize(args);

            double error = Double.MaxValue;
            int iterations = 0;      
            do
            {
                if (iterations % args.ResetInterval == 0)
                    ResetWeights();

                error = TrainIteration(data, args);
                iterations++;

                if (OnWeightsUpdated(iterations, error))
                    break;
            }
            while (!args.IsDone(error, iterations));

            return new TrainingLog(iterations);
        }

        private void ResetWeights()
        {
            backpropNetwork.ResetWeights();
            WeightsReset?.Invoke(this, EventArgs.Empty);
        }

        private double TrainIteration(IDataSet data, BackpropagationArgs args)
        {
            if (args.Type == BackpropagationType.Batch)
            {
                return TrainBatch(data);
            }
            else if (args.Type == BackpropagationType.Stochastic)
            {
                return data.Random().Sum(p => TrainPoint(p));
            }
            else
            {
                return 0.0;
            }
        }

        private double TrainBatch(IEnumerable<ILabeledDataPoint> batch)
        {
            backpropNetwork.ResetGradients(); // Synapses

            var error = batch.Sum(p => TrainPoint(p));

            backpropNetwork.UpdateWeights();

            // TODO
            backpropNetwork.UpdateLearningRates(); // Synapses

            return error;
        }

        private double TrainPoint(ILabeledDataPoint point)
        {
            var result = backpropNetwork.EvaluateLabeled(point.Input, point.Output);
            backpropNetwork.Backpropagate(point.Output); // Neurons
            backpropNetwork.UpdateGradient(); // Synapses

            return result.error;
        }

        private bool OnWeightsUpdated(int iterations, double error)
        {
            var status = new TrainingStatus(backpropNetwork, iterations, error);
            WeightsUpdated?.Invoke(this, status);
            return status.StopTraining;
        }
    }
}