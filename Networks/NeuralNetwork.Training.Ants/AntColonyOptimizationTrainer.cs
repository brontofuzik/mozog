using System;
using AntColonyOptimization;
using NeuralNetwork.Data;
using NeuralNetwork.MLP;

namespace NeuralNetwork.Training.Ants
{
    public class AntColonyOptimizationTrainer : TrainerBase<TrainingArgs>
    {
        public override TrainingLog Train(INetwork network, IDataSet data, TrainingArgs args)
        {
            var networkOptimization = new AntColonyOptimization.AntColonyOptimization(network.SynapseCount)
            {
                ObjectiveFunc = ObjectiveFunction.Minimize(weights =>
                {
                    network.SetWeights(weights);
                    return network.CalculateError((DataSet)data);
                })
            };

            var result = networkOptimization.Run(antCount: 100, gaussianCount: 10, maxIterations: args.MaxIterations, targetEvaluation: args.MaxError);

            network.SetWeights(result.Solution);

            return new TrainingLog(result.Iterations);
        }

        // TODO
        public override event EventHandler<TrainingStatus> WeightsUpdated;

        // TODO
        public override event EventHandler WeightsReset;
    }
}
