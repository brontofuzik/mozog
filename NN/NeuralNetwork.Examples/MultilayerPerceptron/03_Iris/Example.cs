using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.ErrorFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Iris
{
    static class Example
    {
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            var data = Data.Create();
            var trainingData = new EncodedDataSet<double[], int>(4, 3, Data.Encoder);
            var testData = new EncodedDataSet<double[], int>(4, 3, Data.Encoder);
            data.Random().ForEach((p, i) =>
            {
                if (i < 30)
                    trainingData.Add(p);
                else
                    testData.Add(p);
            });

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 3, data.OutputSize },
                Activation.Sigmoid,
                Error.MSE);

            //var architecture = NetworkArchitecture.Feedforward(new(int, IActivationFunction)[]
            //{
            //    (data.InputSize, null),
            //    (20, Activation.Linear),
            //    (data.OutputSize, Activation.Softmax)
            //}, Error.CEE);

            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer();
            trainer.TrainingProgress += LogTrainingProgress;

            var log = trainer.Train(network, trainingData, BackpropagationArgs.Batch(
                learningRate: 0.1,
                momentum: 0.9,
                maxError: 0.1,
                resetInterval: 1_000));

            Console.WriteLine(log);

            // Step 4: Test the network.

            var testStats = trainer.Test(network, testData);
            var classificationStats = trainer.TestClassifier(network, testData, Data.Encoder);
            Console.WriteLine($"Test stats: {testStats} (Acc: {classificationStats.accuracy:P2}, Pre: {classificationStats.precision:P2}, Rec: {classificationStats.recall:P2})");
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
