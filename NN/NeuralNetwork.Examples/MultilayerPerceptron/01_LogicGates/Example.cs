using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.ErrorFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.LogicGates
{
    static class Example
    {
        private static DataSet data;    
        private static INetwork network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = Data.XOR;

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 2, data.OutputSize },
                Activation.Sigmoid,
                Error.MSE);
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer();
            var args = new BackpropagationArgs(
                maxIterations: 1000,
                maxError: 0.001,
                type: BackpropagationType.Batch,
                learningRate: 0.05,
                momentum: 0.9);      
            var log = trainer.Train(network, data, args);

            Console.WriteLine($"Iterations: {log.Iterations}, Error:{log.TrainingError}");

            // Step 4: Test the trained network.

            foreach (var point in data)
            {
                var output = network.Evaluate(point.Input);
                Console.WriteLine($"{Vector.ToString(point.Input)} -> {Vector.ToString(output)}");
            }
        }
    }
}
