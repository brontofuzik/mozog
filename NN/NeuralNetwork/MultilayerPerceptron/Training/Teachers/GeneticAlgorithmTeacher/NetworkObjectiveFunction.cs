﻿using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    internal class NetworkObjectiveFunction
        : ObjectiveFunction<double>
    {
        private INetwork network;

        private TrainingSet trainingSet;

        public NetworkObjectiveFunction(INetwork network, TrainingSet trainingSet)
            : base(network.SynapseCount, Objective.Minimize)
        {
            this.network = network;
            this.trainingSet = trainingSet;
        }

        public override double Evaluate(double[] weights)
        {
            network.SetWeights(weights);
            return network.CalculateError(trainingSet);
        }
    }
}
