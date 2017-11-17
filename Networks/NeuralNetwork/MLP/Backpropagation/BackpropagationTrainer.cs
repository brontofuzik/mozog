using System;
using System.Collections.Generic;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MLP.Backpropagation
{
    public class BackpropagationTrainer : TrainerBase<BackpropagationArgs>
    {
        private BackpropagationNetwork backpropNetwork;
        private IDataSet data;

        protected BackpropagationArgs args;
        
        public override event EventHandler<TrainingStatus> WeightsUpdated;

        public override event EventHandler WeightsReset;

        public override TrainingLog Train(INetwork network, IDataSet data, BackpropagationArgs args)
        {
            this.data = data;
            this.args = args;

            // Convert to backprop network
            var architecture = network.Architecture;
            backpropNetwork = new BackpropagationNetwork(architecture);

            var log = TrainBackprop();

            // Convert back to normal network
            var weights = backpropNetwork.GetWeights();
            network.SetWeights(weights);

            return log;
        }

        protected virtual TrainingLog TrainBackprop()
        {
            backpropNetwork.Initialize(args);
            WeightsReset?.Invoke(this, EventArgs.Empty);

            int iterations = 0;
            DataStatistics stats;
            do
            {
                iterations++;
                stats = TrainIteration(iterations);

                if (OnWeightsUpdated(iterations, stats.AverageError))
                    break;
            }
            while (!IsTrainingDone(stats.AverageError, iterations));

            return new TrainingLog(iterations)
            {
                TrainingStatistics = stats
            };
        }

        protected virtual bool IsTrainingDone(double error, int iteration)
            => error <= args.MaxError || iteration >= args.MaxIterations;

        private DataStatistics TrainIteration(int iteration)
        {
            switch (args.Type)
            {
                case BackpropagationType.Batch:
                    TrainBatch(data, iteration);
                    break;
                
                case BackpropagationType.MiniBatch: // TODO
                    break;
                 
                case BackpropagationType.Stochastic: // TODO
                    break;

                default:
                    throw new ArgumentException($"Backprop type '{args.Type}' not supported", nameof(args.Type));
            }

            return CalculateStats(backpropNetwork, data);
        }

        private void TrainBatch(IEnumerable<ILabeledDataPoint> batch, int iteration)
        {
            backpropNetwork.ResetGradients();
            batch.ForEach(p => TrainPoint(p, iteration));
            backpropNetwork.UpdateWeights(iteration);
        }

        private void TrainPoint(ILabeledDataPoint point, int iteration)
        {
            backpropNetwork.EvaluateLabeled(point.Input, point.Output);
            backpropNetwork.Backpropagate(point.Output);
        }

        private bool OnWeightsUpdated(int iterations, double error)
        {
            var status = new TrainingStatus(backpropNetwork, iterations, error);
            WeightsUpdated?.Invoke(this, status);
            return status.StopTraining;
        }
    }
}