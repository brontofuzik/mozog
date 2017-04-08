﻿using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.ErrorFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Keywords
{
    static class Example
    {
        private static DataSet data;
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = Data.Create();

            // Step 2: Create the network.

            //var architecture = NetworkArchitecture.Feedforward(
            //    new[] { data.InputSize, 20, data.OutputSize },
            //    Activation.Softmax,
            //    Error.CEE);

            var architecture = NetworkArchitecture.Feedforward(new (int, IActivationFunction)[]
            {
                (data.InputSize, null),
                (20, Activation.Linear),
                (data.OutputSize, Activation.Softmax)
            }, Error.CEE);

            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer();
            trainer.TrainingProgress += LogTrainingProgress;

            var log = trainer.Train(network, data, BackpropagationArgs.Stochastic(
                learningRate: 0.05,
                momentum: 0.9,
                maxError: 0.001));

            Console.WriteLine(log);

            // Step 4: Test the network.

            var trainingStats = trainer.Test(network, data);
            Console.WriteLine($"Training stats: {trainingStats}");

            var testData = Data.Create();
            var testStats = trainer.Test(network, testData);
            Console.WriteLine($"Test stats: {testStats}");

            for (int i = 0; i < testData.Size; i += 5)
            {
                // Original keyword
                string originalKeyword = (string)testData[i].Tag;
                var index = network.EvaluateEncoded(originalKeyword, Data.Encoder);
                Console.Write($"{originalKeyword}: {index}");

                // Mutated keywords
                for (int j = i; j < i + 5; j++)
                {
                    string mutatedKeyword = (string)testData[j].Tag;
                    index = network.EvaluateEncoded(mutatedKeyword, Data.Encoder);
                    Console.Write($", {mutatedKeyword}: {index}");
                }
                Console.WriteLine();
            }
        }

        private static void LogTrainingProgress(object sender, TrainingStatus e)
        {
            if (e.Iterations % 1 == 0)
            {
                Console.WriteLine($"{e.Iterations:D5}: {e.Error:F2}");
            }
        }
    }
}