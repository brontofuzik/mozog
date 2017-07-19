using System;
using Mozog.Utils;
using Mozog.Utils.Math;
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
        private static INetwork network;

        public static void Run()
        {
            // Parameters

            const double learningRate = 0.1;
            const double maxError = 0.001;
            const int restartInterval = 1_000;

            // Step 1: Create the training set.

            var data = Data.XOR;

            // Step 2: Create the network.

            // Sigmoid & MSE
            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 2, data.OutputSize },
                Activation.Sigmoid,
                Error.MSE);

            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new RestartingBackpropTrainer(restartInterval);
            trainer.WeightsUpdated += LogTrainingProgress;

            var args = BackpropagationArgs.Batch(Optimizer.RmsProp(learningRate), maxError);
            var log = trainer.Train(network, data, args);
            Console.WriteLine(log);

            // Step 4: Test the trained network.

            // Test using the same data.

            foreach (var point in data)
            {
                var output = network.EvaluateUnlabeled(point.Input);
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
