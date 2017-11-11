using System;
using Mozog.Utils.Math;
using NeuralNetwork.HopfieldNet;

namespace NeuralNetwork.Examples.HopfieldNet
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
            var net = new HopfieldNetwork(rows, cols, true, (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            net.Initialize(p => 1.0, (p, sourceP) => (p.Row == sourceP.Row || p.Col == sourceP.Col) ? -2.0 : 0.0);

            // Step 4: Test the network.

            double[] solution = net.Evaluate(new double[rows * cols], 10);

            Console.WriteLine(Vector.ToString(solution));
        }
    }
}
