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
            // Parameters

            const int hiddenNeurons = 5;
            const double learningRate = 0.1;
            const double maxError = 0.1;
            const int restartInterval = 1_000;

            // Step 1: Create the training set.

            var trainingData = Data.Create();
            var testData = Data.Create();

            // Step 2: Create the network.

            // Softmax & CEE
            var architecture = NetworkArchitecture.Feedforward(new(int, IActivationFunction)[]
            {
                (trainingData.InputSize, null),
                (hiddenNeurons, Activation.Sigmoid),
                (trainingData.OutputSize, Activation.Softmax)
            }, Error.CEE);

            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new RestartingBackpropTrainer(restartInterval);
            trainer.WeightsUpdated += LogTrainingProgress;

            var args = BackpropagationArgs.Batch(Optimizer.RmsProp(learningRate), maxError);
            var log = trainer.Train(network, trainingData, args);
            Console.WriteLine(log);

            // Step 4: Test the network.

            var testingLog = trainer.Test(network, testData);
            Console.WriteLine(testingLog);

            for (int i = 0; i < testData.Size; i += 5)
            {
                // Original keyword
                string originalKeyword = (string)testData[i].Tag;
                var index = network.EvaluateUnlabeled(originalKeyword, Data.Encoder);
                Console.Write($"{originalKeyword}: {index}");                

                // Mutated keywords
                for (int j = i + 1; j < i + 5; j++)
                {
                    string mutatedKeyword = (string)testData[j].Tag;
                    index = network.EvaluateUnlabeled(mutatedKeyword, Data.Encoder);
                    Console.Write($", {mutatedKeyword}: {index}");
                }
                Console.WriteLine();
            }
        }

        private static void LogTrainingProgress(object sender, TrainingStatus e)
        {
            if (e.Iterations % 10 == 0)
            {
                Console.WriteLine($"{e.Iterations:D5}: {e.Error:F2}");
            }
        }
    }
}