using System;
using GeneticAlgorithm;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training.Evolutionary
{
    public class GeneticAlgorithmTrainer : TrainerBase<TrainingArgs>
    {
        public override TrainingLog Train(INetwork network, IDataSet data, TrainingArgs args)
        {
            var geneticAlgo = new GeneticAlgorithm<double>(network.SynapseCount).Configure(cfg => cfg
                .Fitness.Minimize(chromosome =>
                {
                    network.SetWeights(chromosome);
                    return network.CalculateError((DataSet)data);
                })
                .Initialization.Piecewise(() => StaticRandom.Double(-10, +10))
                .Crossover.SinglePoint()
                .Mutation.RandomPoint(gene => (gene + StaticRandom.Double(-10, +10)) / 2.0)
                .Termination.MaxGenerationsOrAcceptableEvaluation(maxGenerations: args.MaxIterations, acceptableEvaluation: args.MaxError));

            var result = geneticAlgo.Run(populationSize: 500, crossoverRate: 0.8, mutationRate: 0.05);
            network.SetWeights(result.Solution);

            return new TrainingLog(result.Generations);
        }

        // TODO
        public override event EventHandler<TrainingStatus> WeightsUpdated;

        // TODO
        public override event EventHandler WeightsReset;
    }
}
