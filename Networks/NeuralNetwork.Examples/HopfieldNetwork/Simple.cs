using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.HopfieldNet;
using System.Diagnostics;

namespace NeuralNetwork.Examples.HopfieldNet
{
    class Simple
    {
        public static void Run()
        {
            // Parameters

            int iterations = 10;

            // Step 1: Create the training set.

            var dataSet = new DataSet(10)
            {
                new LabeledDataPoint(new[] { 1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, -1.0 }, new double[0])
            };

            // Step 2: Create the network.

            var net = new HopfieldNetwork(dataSet.InputSize, sparse: false);

            // Step 3: Train the network.

            net.Train(dataSet);

            // Step 4: Test the network.

            double[] pattern = { -1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, 1.0 };
            double[] recalled = net.Evaluate(pattern, iterations);

            Debug.Assert(recalled.SequenceEqual(dataSet[0].Input));
        }
    }
}
