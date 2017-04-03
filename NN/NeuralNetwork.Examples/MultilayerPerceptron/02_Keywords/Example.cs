using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
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
                new SigmoidFunction());
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer(data, null, null);
            
            var args = new BackpropagationArgs(
                maxIterations: Int32.MaxValue,
                maxError: 0.01,
                learningRate: 0.05,
                momentum: 0.9,
                batchLearning: true);
            var log = trainer.Train(network, args);

            Console.WriteLine($"Iterations: {log.IterationCount}, Error:{log.NetworkError}");

            // Step 4: Test the network.

            for (int i = 0; i < Data.KeywordCount; i++)
            {
                // Original keyword
                string originalKeyword = Data.Keywords[i];
                var index = network.EvaluateEncoded(originalKeyword, Data.Encoder);
                Console.Write($"{originalKeyword}: {index}");

                // Mutated keywords
                4.Times(() =>
                {
                    string mutatedKeyword = Data.MutateKeyword(originalKeyword);
                    index = network.EvaluateEncoded(mutatedKeyword, Data.Encoder);
                    Console.Write($", {mutatedKeyword}: {index}");
                });
                Console.WriteLine();
            }
        }
    }
}