using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
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
                new[] {data.InputSize, 2, data.OutputSize},
                new HyperbolicTangent());
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer(data, null, null);

            var args = new BackpropagationArgs(
                maxIterations: 1000,
                maxError: 0.001,
                learningRate: 0.01,
                momentum: 0.9,
                batchLearning: true);      
            var log = trainer.Train(network, args);

            Console.WriteLine($"Iterations: {log.IterationCount}, Error:{log.NetworkError}");

            // Step 4: Test the trained network.

            foreach (var point in data)
            {
                var output = network.Evaluate(point.Input);
                Console.WriteLine($"{Vector.ToString(point.Input)} -> {Vector.ToString(output)}");
            }
        }
    }
}
