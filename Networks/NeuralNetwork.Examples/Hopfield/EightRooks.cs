using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Hopfield;

namespace NeuralNetwork.Examples.Hopfield
{
    class EightRooks
    {
        public static void Run()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            int rows = 8;
            int cols = 8;
            var net = HopfieldNetwork.Build2DNetwork(rows, cols, sparse: true,
                activation: (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            net.Initialize(
                (p, _net) => 1.0,
                (p, sourceP, _net) => p[0] == sourceP[0] || p[1] == sourceP[1] ? -2.0 : 0.0);

            // Step 4: Test the network.

            var solution = net.Evaluate(new double[rows * cols], 10);

            var chessboard = solution.Select(n => n == 1.0 ? 'X' : '_').ToArray().Split(8);
            var chessboardStr = String.Join(Environment.NewLine, chessboard.Select(r => ArrayExtensions.ToString(r)));

            Console.WriteLine(chessboardStr);
        }
    }
}
