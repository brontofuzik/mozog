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

            int neuronCount = 64;
            var net = new HopfieldNetwork(neuronCount, true, (input, _) => input > 0 ? 1.0 : 0.0);

            // Step 3: Train the network.

            InitializeNet(net);

            // Step 4: Test the network.

            double[] solution = net.Evaluate(new double[neuronCount], 10);

            Console.WriteLine(Vector.ToString(solution));
        }

        private static void InitializeNet(HopfieldNetwork net)
        {
            for (int row = 0; row < 8; row++)
                for (int col = 0; col < 8; col++)
                    InitializeNeuron(net, row, col);
        }
    
        private static void InitializeNeuron(HopfieldNetwork net, int row, int col)
        {
            SetNeuronBias(net, row, col, 1.0);

            // The source neurons in the same row.
            for (int sourceCol = col - 1; 0 <= sourceCol; sourceCol--) // ←
                SetSynapseWeight(net, row, col, row, sourceCol, -2.0);
            for (int sourceCol = col + 1; sourceCol < 8; sourceCol++) // →
                SetSynapseWeight(net, row, col, row, sourceCol, -2.0);

            // The source neurons in the same column.
            for (int sourceRow = row - 1; 0 <= sourceRow; sourceRow--) // ↑
                SetSynapseWeight(net, row, col, sourceRow, col, -2.0);
            for (int sourceRow = row + 1; sourceRow < 8; sourceRow++) // ↓
                SetSynapseWeight(net, row, col, sourceRow, col, -2.0);
        }

        private static void SetNeuronBias(HopfieldNetwork net, int row, int col, double bias)
        {
            int neuronIndex = NeuronPositionToIndex(row, col);
            net.SetNeuronBias(neuronIndex, bias);
        }

        private static void SetSynapseWeight(HopfieldNetwork net, int row, int col, int sourceRow, int souceCol, double weight)
        {
            int neuron = NeuronPositionToIndex(row, col);
            int sourceNeuron = NeuronPositionToIndex(sourceRow, souceCol);
            net.SetSynapseWeight(neuron, sourceNeuron, weight);
        }

        private static int NeuronPositionToIndex(int row, int col) => row * 8 + col;

        private static (int, int) NeuronIndexToPosition(int index) => (index / 8, index % 8);
    }
}
