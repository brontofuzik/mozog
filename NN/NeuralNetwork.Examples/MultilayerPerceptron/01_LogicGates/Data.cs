using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.LogicGates
{
    static class Data
    {
        private const bool F = false;
        private const bool T = true;

        public static readonly IEncoder<(bool, bool), bool> Encoder = new LogicGatesEncoder();

        public static IDataSet AND => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), F},
            {(T, F), F},
            {(T, T), T}
        };

        public static IDataSet OR => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), T},
            {(T, F), T},
            {(T, T), T}
        };

        public static IDataSet XOR => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), T},
            {(T, F), T},
            {(T, T), F}
        };

        private class LogicGatesData : EncodedDataSet<(bool, bool), bool>
        {
            public LogicGatesData() : base(2, 1, Encoder)
            {
            }
        }

        private class LogicGatesEncoder : IEncoder<(bool a, bool b), bool>
        {
            public double[] EncodeInput((bool a, bool b) input)
                => new[] { BoolToReal(input.a), BoolToReal(input.b) };

            public double[] EncodeOutput(bool output)
                => new[] { BoolToReal(output) };

            public bool DecodeOutput(double[] output)
                => RealToBool(output[0]);

            private static double BoolToReal(bool b) => b ? 1.0 : 0.0;

            private static bool RealToBool(double d) => d > 0.5;
        }
    }
}
