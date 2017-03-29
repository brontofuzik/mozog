using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron
{
    static class Xor
    {
        public static void Run()
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            const int inputLength = 2;
            const int outputLength = 1;
            var trainingSet = new TrainingSet(inputLength, outputLength)
            {
                (new[] {0.0, 0.0}, new[] {0.0}),
                (new[] {0.0, 1.0}, new[] {1.0}),
                (new[] {1.0, 0.0}, new[] {1.0}),
                (new[] {1.0, 1.0}, new[] {0.0})
            };

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            var architecture = NetworkArchitecture.Feedforward(new[] {inputLength, 2, outputLength}, new LinearActivationFunction());
            Network network = new Network(architecture);

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            BackpropagationTrainer trainer = new BackpropagationTrainer(trainingSet, null, null);

            var args = new BackpropagationArgs(
                maxIterations: 10000,
                maxError: 0.001,
                learningRate: 0.01,
                momentum: 0.9,
                batchLearning: true);      
            var trainingLog = trainer.Train(network, args);

            Console.WriteLine($"Number of iterations used: {trainingLog.IterationCount}");
            Console.WriteLine($"Minimum network error achieved: {trainingLog.NetworkError}");

            // ---------------------------------
            // Step 4: Test the trained network.
            // ---------------------------------

            foreach (var pattern in trainingSet)
            {
                double[] inputVector = pattern.InputVector;
                double[] outputVector = network.Evaluate(inputVector);
                Console.WriteLine(pattern + " -> " + Vector.ToString(outputVector));
            }
        }
    }
}
