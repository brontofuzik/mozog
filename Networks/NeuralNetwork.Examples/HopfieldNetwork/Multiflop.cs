using Mozog.Utils.Math;
using NeuralNetwork.HopfieldNet;
using System;

namespace NeuralNetwork.Examples.HopfieldNet
{
    class Multiflop
    {
        public static void Run()
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            int neuronCount = 4;
            var net = new HopfieldNetwork(neuronCount, false, (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            InitializeNet(net);

            // Step 4: Test the network.

            double[] recalled = net.Evaluate(new double[neuronCount], iterations: 10);
            Console.WriteLine(Vector.ToString(recalled));
        }

        private static void InitializeNet(HopfieldNetwork net)
        {
            for (int neuron = 0; neuron < net.NeuronCount; neuron++)
            {
                net.SetNeuronBias(neuron, bias: 1.0);
                for (int sourceNeuron = 0; sourceNeuron < net.NeuronCount; sourceNeuron++)
                {
                    if (sourceNeuron == neuron) continue;
                    net.SetSynapseWeight(neuron, sourceNeuron, weight: -2.0);
                }
            }
        }
    }
}
