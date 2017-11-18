using System;
using Mozog.Utils.Math;

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
            var net = NeuralNetwork.Hopfield.HopfieldNetwork.Build1DNetwork(neuronCount,
                sparse: false,
                activation: (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            net.Initialize((p, _net) => 1.0, (p, sourceP, _net) => -2.0);

            // Step 4: Test the network.

            var recalled = net.Evaluate(new double[neuronCount], iterations: 10);

            Console.WriteLine(Vector.ToString(recalled));
        }
    }
}
