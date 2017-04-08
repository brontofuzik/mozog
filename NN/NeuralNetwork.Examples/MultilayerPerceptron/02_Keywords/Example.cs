﻿using System;
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

            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 20, data.OutputSize },
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

            // Step 4: Test the network.

            var trainingStats = trainer.Test(network, data);
            Console.WriteLine($"Training stats: {trainingStats}");

            var testData = Data.Create();
            var testStats = trainer.Test(network, testData);
            Console.WriteLine($"Test stats: {testStats}");

            //for (int i = 0; i < Data.KeywordCount; i++)
            //{
            //    // Original keyword
            //    string originalKeyword = Data.Keywords[i];
            //    var index = network.EvaluateEncoded(originalKeyword, Data.Encoder);
            //    Console.Write($"{originalKeyword}: {index}");

            //    // Mutated keywords
            //    4.Times(() =>
            //    {
            //        string mutatedKeyword = Data.MutateKeyword(originalKeyword);
            //        index = network.EvaluateEncoded(mutatedKeyword, Data.Encoder);
            //        Console.Write($", {mutatedKeyword}: {index}");
            //    });
            //    Console.WriteLine();
            //}
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