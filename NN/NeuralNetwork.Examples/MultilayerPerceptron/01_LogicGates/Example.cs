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
            trainer.TrainingProgress += LogTrainingProgress;

            var log = trainer.Train(network, data, BackpropagationArgs.Batch(
                learningRate: 0.05,
                maxError: 0.001));

            Console.WriteLine(log);

            // Step 4: Test the trained network.

            // Test using the same data.

            var trainingStats = trainer.Test(network, data);
            Console.WriteLine($"Training stats: {trainingStats}");

            foreach (var point in data)
            {
                var output = network.Evaluate(point.Input);
                Console.WriteLine($"{Vector.ToString(point.Input)} -> {Vector.ToString(output)}");
            }
        }

        private static void LogTrainingProgress(object sender, TrainingStatus e)
        {
            if (e.Iterations % 100 == 0)
            {
                Console.WriteLine($"{e.Iterations:D5}: {e.Error:F2}");
            }
        }
    }
}
