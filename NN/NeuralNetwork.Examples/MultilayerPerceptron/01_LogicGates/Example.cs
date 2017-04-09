﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        private static INetwork network;

        public static void Run()
        {
            // Parameters

            const int hiddenNeurons = 2;
            const double maxError = 0.001;
            const int resetInterval = 5_000;

            // Step 1: Create the training set.

            var data = Data.XOR;

            // Step 2: Create the network.

            // Sigmoid & MSE
            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, hiddenNeurons, data.OutputSize },
                Activation.Sigmoid,
                Error.MSE);

            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer();
            trainer.WeightsUpdated += LogTrainingProgress;

            var log = trainer.Train(network, data, BackpropagationArgs.Batch(
                learningRate: 0.1,
                momentum: 0.9,
                maxError: maxError,
                resetInterval: resetInterval));

            Console.WriteLine(log);

            // Step 4: Test the trained network.

            // Test using the same data.

            //var trainingStats = trainer.Test(network, data);
            //Console.WriteLine($"Training stats: {trainingStats}");

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
