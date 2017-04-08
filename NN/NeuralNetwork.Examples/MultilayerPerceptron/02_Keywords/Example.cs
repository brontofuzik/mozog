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
                maxError: 0.1));

            Console.WriteLine(log);

            // Step 4: Test the network.

            int correctlyClassified = 0;
            for (int i = 0; i < testData.Size; i += 5)
            {
                // Original keyword
                string originalKeyword = (string)testData[i].Tag;
                var index = network.EvaluateEncoded(originalKeyword, Data.Encoder);
                if (Data.Keywords[index] == originalKeyword) correctlyClassified++;

                Console.Write($"{originalKeyword}: {index}");                

                // Mutated keywords
                for (int j = i + 1; j < i + 5; j++)
                {
                    string mutatedKeyword = (string)testData[j].Tag;
                    index = network.EvaluateEncoded(mutatedKeyword, Data.Encoder);
                    if (Data.Keywords[index] == originalKeyword) correctlyClassified++;

                    Console.Write($", {mutatedKeyword}: {index}");
                }
                Console.WriteLine();
            }

            var testStats = trainer.Test(network, testData);
            var percentage = correctlyClassified / (double)testData.Size * 100;
            Console.WriteLine($"Test stats: {testStats} ({correctlyClassified}/{testData.Size} = {percentage:F2}%)");
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