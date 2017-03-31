using System;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.LogicGates
{
    static class Data
    {
        public static readonly IEncoder<(int, int), int> Encoder = new _Encoder();

        public static DataSet CreateXOR()
        {
            return new EncodedDataSet<(int, int), int>(2, 1, Encoder)
            {
                {(0, 0), 0},
                {(0, 1), 1},
                {(1, 0), 1},
                {(1, 1), 0}
            };
        }

        class _Encoder : IEncoder<(int A, int B), int>
        {
            public double[] EncodeInput((int A, int B) input)
                => new double[] { input.A, input.B };

            public double[] EncodeOutput(int output)
                => new double[] { output };

            public int DecodeOutput(double[] output)
                => (int)Math.Round(output[0]);
        }
    }
}
