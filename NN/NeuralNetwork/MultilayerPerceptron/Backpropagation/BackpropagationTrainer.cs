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
                    ResetWeights(args);

                error = TrainIteration(data, args, iterations + 1);
                iterations++;

                if (OnWeightsUpdated(iterations, error))
                    break;
            }
            while (!args.IsTrainingDone(error, iterations));

            return new TrainingLog(iterations);
        }

        private void ResetWeights(BackpropagationArgs args)
        {
            backpropNetwork.Initialize(args);
            WeightsReset?.Invoke(this, EventArgs.Empty);
        }

        private double TrainIteration(IDataSet data, BackpropagationArgs args, int iteration)
        {
            if (args.Type == BackpropagationType.Batch)
            {
                return TrainBatch(data, iteration);
            }
            else if (args.Type == BackpropagationType.Stochastic)
            {
                return data.Random().Sum(p => TrainPoint(p, iteration));
            }
            else
            {
                return 0.0;
            }
        }

        private double TrainBatch(IEnumerable<ILabeledDataPoint> batch, int iteration)
        {
            backpropNetwork.ResetGradients();
            var error = batch.Sum(p => TrainPoint(p, iteration));
            backpropNetwork.UpdateWeights(iteration);
            return error / batch.Count();
        }

        private double TrainPoint(ILabeledDataPoint point, int iteration)
        {
            var result = backpropNetwork.EvaluateLabeled(point.Input, point.Output);
            backpropNetwork.Backpropagate(point.Output);
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