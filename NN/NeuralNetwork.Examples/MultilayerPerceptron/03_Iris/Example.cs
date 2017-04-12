using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Data;
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
            // Parameters

            const int hiddenNeurons = 2;
            const string activation = "Softmax";

            const double learningRate = 0.1;
            const double maxError = 0.01;
            const int resetInterval = 5_000;

            // Step 1: Create the training set.

            var data = Data.Create();
            var trainingData = ClassificationData.New(Data.Encoder, 4, 3);
            var testData = ClassificationData.New(Data.Encoder, 4, 3);
            data.Random().ForEach((p, i) =>
            {
                if (i < 100)
                    trainingData.Add(p);
                else
                    testData.Add(p);
            });

            // Step 2: Create the network.

            INetworkArchitecture architecture;
            if (activation == "Sigmoid")
            {
                // Sigmoid & MSE
                architecture = NetworkArchitecture.Feedforward(
                    new[] { trainingData.InputSize, 5, trainingData.OutputSize },
                    Activation.Sigmoid,
                    Error.MSE);
            }
            else
            {
                // Softmax & CEE
                architecture = NetworkArchitecture.Feedforward(new(int, IActivationFunction)[]
                {
                    (trainingData.InputSize, null),
                    (hiddenNeurons, Activation.Sigmoid),
                    (trainingData.OutputSize, Activation.Softmax)
                }, Error.CEE);
            }

            network = new Network(architecture);

            // Step 3: Train the network.

            //var trainer = new BackpropagationTrainer();
            var trainer = new ValidatingTrainer<BackpropagationArgs>(new BackpropagationTrainer(), 0.6, 0.2, 0.2);
            trainer.WeightsUpdated += LogTrainingProgress;

            var args = BackpropagationArgs.Batch(Optimizer.Adam(learningRate), maxError);
            var log = trainer.Train(network, trainingData, args);
            Console.WriteLine(log);

            // Step 4: Test the network.

            var testingLog = trainer.Test(network, testData);
            Console.WriteLine(testingLog);
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
