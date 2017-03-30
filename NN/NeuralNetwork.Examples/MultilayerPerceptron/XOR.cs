using System;
using Mozog.Utils;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.Interfaces;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.Backpropagation;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron
{
    static class Xor
    {
        private static readonly IEncoder<(int A, int B), int> encoder = new XorEncoder();
        private static DataSet data;    
        private static INetwork network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = CreateDataSet();

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] {data.InputSize, 2, data.OutputSize},
                new LinearActivationFunction());
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer(data, null, null);

            var args = new BackpropagationArgs(
                maxIterations: 10000,
                maxError: 0.001,
                learningRate: 0.01,
                momentum: 0.9,
                batchLearning: true);      
            var log = trainer.Train(network, args);

            Console.WriteLine($"Iterations: {log.IterationCount}, Error:{log.NetworkError}");

            // Step 4: Test the trained network.

            foreach (var point in data)
            {
                double[] output = network.Evaluate(point.Input);
                Console.WriteLine($"{Vector.ToString(point.Input)} -> {Vector.ToString(output)}");
            }
        }

        private static DataSet CreateDataSet()
        {
            return new DataSet(2, 1)
            {
                (encoder.EncodeInput((0, 0)), encoder.EncodeOutput(0)),
                (encoder.EncodeInput((0, 1)), encoder.EncodeOutput(1)),
                (encoder.EncodeInput((1, 0)), encoder.EncodeOutput(1)),
                (encoder.EncodeInput((1, 1)), encoder.EncodeOutput(0))
            };
        }
    }

    class XorEncoder : IEncoder<(int A, int B), int>
    {
        public double[] EncodeInput((int A, int B) input)
            => new double[] {input.A, input.B};

        public double[] EncodeOutput(int output)
            => new double[] {output};

        public int DecodeOutput(double[] output)
            => (int)Math.Round(output[0]);
    }
}
