using System;
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
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            var trainingData = Data.Create();
            var testData = Data.Create();

            // Step 2: Create the network.

            // Sigmoid & MSE
            var architecture = NetworkArchitecture.Feedforward(
                new[] { trainingData.InputSize, 10, trainingData.OutputSize },
                Activation.Sigmoid,
                Error.MSE);

            // Linear-Softmax & CEE
            //var architecture = NetworkArchitecture.Feedforward(new (int, IActivationFunction)[]
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

            for (int i = 0; i < testData.Size; i += 5)
            {
                // Original keyword
                string originalKeyword = testData[i].InputTag;
                var index = network.EvaluateEncoded(originalKeyword, Data.Encoder);
                Console.Write($"{originalKeyword}: {index}");                

                // Mutated keywords
                for (int j = i + 1; j < i + 5; j++)
                {
                    string mutatedKeyword = testData[j].InputTag;
                    index = network.EvaluateEncoded(mutatedKeyword, Data.Encoder);
                    Console.Write($", {mutatedKeyword}: {index}");
                }
                Console.WriteLine();
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