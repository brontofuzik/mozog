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
        private static DataSet data;
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = Data.Create();
            var trainingData = new DataSet(4, 3);
            var testData = new DataSet(4, 3);
            data.Random().ForEach((p, i) =>
            {
                if (i < 120)
                    trainingData.Add(p);
                else
                    testData.Add(p);
            });

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] { data.InputSize, 5, data.OutputSize },
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

            var log = trainer.Train(network, trainingData, new BackpropagationArgs(
                BackpropagationType.Batch,
                learningRate: 0.1,
                momentum: 0.9,
                maxError: 2.0,
                maxIterations: Int32.MaxValue));

            Console.WriteLine(log);

            // Step 4: Test the network.

            var trainingStats = trainer.Test(network, trainingData);
            Console.WriteLine($"Training stats: {trainingStats}");

            var testStats = trainer.Test(network, testData);
            Console.WriteLine($"Test stats: {testStats}");

            foreach (var point in testData)
            {
                var output = network.EvaluateEncoded(point.Input, Data.Encoder);
                Console.WriteLine($"{point.Tag}: {output}");
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
