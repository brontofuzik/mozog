using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
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

            log.TrainingStatistics = TestBasic(network, data);

            return log;
        }

        protected virtual TrainingLog TrainBackprop()
        {
            backpropNetwork.Initialize(args);
            WeightsReset?.Invoke(this, EventArgs.Empty);

            double error = Double.MaxValue;
            int iterations = 0;
            bool interrupt;
            do
            {
                iterations++;
                error = TrainIteration(iterations);          
                interrupt = OnWeightsUpdated(iterations, error);
            }
            while (!IsTrainingDone(error, iterations) && !interrupt);

            return new TrainingLog(iterations, error);
        }

        protected virtual bool IsTrainingDone(double error, int iteration)
            => error <= args.MaxError || iteration >= args.MaxIterations;

        private double TrainIteration(int iteration)
        {
            switch (args.Type)
            {
                case BackpropagationType.Batch:
                    return TrainBatch(data, iteration);

                // TODO 
                case BackpropagationType.MiniBatch:           
                    return 0.0;

                // TODO 
                case BackpropagationType.Stochastic:              
                    return 0.0;

                default:
                    throw new ArgumentException($"Backprop type '{args.Type}' not supported", nameof(args.Type));
            }
        }

        private double TrainBatch(IEnumerable<ILabeledDataPoint> batch, int iteration)
        {
            backpropNetwork.ResetGradients();
            var error = batch.Sum(p => TrainPoint(p, iteration)) / batch.Count();

            backpropNetwork.UpdateWeights(iteration);

            return error;
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