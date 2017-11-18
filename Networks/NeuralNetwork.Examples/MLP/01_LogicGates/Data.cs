using NeuralNetwork.Data;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MLP.LogicGates
{
    static class Data
    {
        private const bool F = false;
        private const bool T = true;

        public static readonly IEncoder<(bool, bool), bool> Encoder = new LogicGatesEncoder();

        public static ILogicGatesData AND => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), F},
            {(T, F), F},
            {(T, T), T}
        };

        public static ILogicGatesData OR => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), T},
            {(T, F), T},
            {(T, T), T}
        };

        public static ILogicGatesData XOR => new LogicGatesData
        {
            {(F, F), F},
            {(F, T), T},
            {(T, F), T},
            {(T, T), F}
        };

        // Typedef
        private class LogicGatesData : EncodedData<(bool, bool), bool>, ILogicGatesData
        {
            public LogicGatesData()
                : base(Data.Encoder, 2, 1)
            {
            }
        }

        // Typedef
        public interface ILogicGatesData : IEncodedData<(bool, bool), bool>
        {
        }

        private class LogicGatesEncoder : IEncoder<(bool a, bool b), bool>
        {
            public double[] EncodeInput((bool a, bool b) input)
                => new[] { BoolToReal(input.a), BoolToReal(input.b) };

            public (bool a, bool b) DecodeInput(double[] input)
            {
                // Not needed
                throw new System.NotImplementedException();
            }

            public double[] EncodeOutput(bool output)
                => new[] { BoolToReal(output) };

            public bool DecodeOutput(double[] output)
                => RealToBool(output[0]);

            private static double BoolToReal(bool b) => b ? 1.0 : 0.0;

            private static bool RealToBool(double d) => d > 0.5;
        }
    }
}
