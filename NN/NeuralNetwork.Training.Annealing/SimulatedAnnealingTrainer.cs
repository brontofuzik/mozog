using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;
using SimulatedAnnealing;
using SimulatedAnnealing.Functions.Cooling;
using SimulatedAnnealing.Functions.Initialization;
using SimulatedAnnealing.Functions.Objective;
using SimulatedAnnealing.Functions.Perturbation;

namespace NeuralNetwork.Training.Annealing
{
    public class SimulatedAnnealingTrainer : TrainerBase<TrainingArgs>
    {
        public override TrainingLog Train(INetwork network, IDataSet data, TrainingArgs args)
        {
            var networkAnnealing = new SimulatedAnnealing<double>(network.SynapseCount)
            {
                Objective = ObjectiveFunction<double>.Minimize(weights =>
                {
                    network.SetWeights(weights);
                    return network.CalculateError((DataSet)data);
                }),
                Initialization = new LambdaInitialization<double>(d => d.Times(() => StaticRandom.Double(-20, +20)).ToArray()),
                Perturbation = new LambdaPerturbation<double>(weights =>
                {
                    double[] newWeights = new double[weights.Length];
                    Array.Copy(weights, newWeights, weights.Length);

                    int index = StaticRandom.Int(0, newWeights.Length);
                    newWeights[index] = (newWeights[index] + StaticRandom.Double(-20, +20)) / 2.0;

                    return newWeights;
                }),
                Cooling = LambdaCooling.Exponential
            };

            var result = networkAnnealing.Run(initialTemperature: 1000.0, finalTemperature: 0.001,
                targetEnergy: args.MaxError, maxIterations: args.MaxIterations);

            network.SetWeights(result.State.S);

            return new TrainingLog(0);
        }

        // TODO
        public override event EventHandler<TrainingStatus> WeightsUpdated;

        // TODO
        public override event EventHandler WeightsReset;
    }
}
