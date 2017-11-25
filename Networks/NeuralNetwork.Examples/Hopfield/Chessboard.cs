using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Hopfield;

namespace NeuralNetwork.Examples.Hopfield
{
    class Chessboard
    {
        public static void RunEightRooks()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            var net = BuildNetwork();

            // Step 3: Train the network.

            net.Initialize(
                (n, _) => 1.0,
                (n, s, _) => Row(n) == Row(s) || Col(n) == Col(s) ? -2.0 : 0.0);

            // Step 4: Test the network.

            var solution = net.Evaluate(new double[64], iterations: 10);

            var chessboard = solution.Select(n => n == 1.0 ? 'X' : '_').ToArray().Split(8);
            var chessboardStr = String.Join(Environment.NewLine, chessboard.Select(ArrayExtensions.ToString));

            Console.WriteLine(chessboardStr);
        }

        public static void RunEightQueens()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            var net = BuildNetwork();

            // Step 3: Train the network.

            net.Initialize(
                (n, _) => 1.0,
                (n, s, _) => Row(n) == Row(s) || Col(n) == Col(s) || Math.Abs(Row(n) - Row(s)) == Math.Abs(Col(n) - Col(s)) ? -2.0 : 0.0);

            // Step 4: Test the network.

            var solution = net.Evaluate(new double[64], iterations: 10);

            var chessboard = solution.Select(n => n == 1.0 ? 'X' : '_').ToArray().Split(8);
            var chessboardStr = String.Join(Environment.NewLine, chessboard.Select(ArrayExtensions.ToString));

            Console.WriteLine(chessboardStr);
        }

        private static HopfieldNetwork BuildNetwork()
            => HopfieldNetwork.Build2DNetwork(8, 8, sparse: true, activation: (input, _) => input > 0 ? 1.0 : 0.0);

        private static int Row(int[] neuron) => neuron[0];

        private static int Col(int[] neuron) => neuron[1];
    }
}
