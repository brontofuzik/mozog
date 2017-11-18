using System;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.Examples.Hopfield
{
    class EightQueens
    {
        public static void Run()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            int rows = 8;
            int cols = 8;
            var net = NeuralNetwork.Hopfield.HopfieldNetwork.Build2DNetwork(rows, cols,
                sparse: true,
                activation: (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            net.Initialize(
                (p, _net) => 1.0,
                (p, sourceP, _net) => (p.Row == sourceP.Row || p.Col == sourceP.Col || System.Math.Abs(p.Row - sourceP.Row) == System.Math.Abs(p.Col - sourceP.Col)) ? -2.0 : 0.0);

            // Step 4: Test the network.

            var solution = net.Evaluate(new double[rows * cols], 10);

            var chessboard = solution.Select(n => n == 1.0 ? 'X' : '_').ToArray().Split(8);
            var chessboardStr = String.Join(Environment.NewLine, chessboard.Select(r => ArrayExtensions.ToString(r)));

            Console.WriteLine(chessboardStr);
        }
    }
}
