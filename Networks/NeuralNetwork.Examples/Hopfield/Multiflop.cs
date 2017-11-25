using System;
using Mozog.Utils.Math;
using NeuralNetwork.Hopfield;

namespace NeuralNetwork.Examples.Hopfield
{
    class Multiflop
    {
        public static void Run()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            int neuronCount = 4;
            var net = HopfieldNetwork.Build1DNetwork(neuronCount, sparse: false,
                activation: (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            net.Initialize((n, _) => 1.0, (n, s, _) => -2.0);

            // Step 4: Test the network.

            var recalled = net.Evaluate(new double[neuronCount], iterations: 10);

            Console.WriteLine(Vector.ToString(recalled));
        }
    }
}
