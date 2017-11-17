using System.Linq;
using System.Diagnostics;
using NeuralNetwork.Data;

namespace NeuralNetwork.Examples.HopfieldNet
{
    class Simple
    {
        public static void Run()
        {
            // Step 1: Create the training set.

            var dataSet = new DataSet(10)
            {
                new LabeledDataPoint(new[] { 1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, -1.0 }, new double[0])
            };

            // Step 2: Create the network.

            var net = NeuralNetwork.HopfieldNet.HopfieldNetwork.Build1DNetwork(dataSet.InputSize, sparse: false);

            // Step 3: Train the network.

            net.Train(dataSet);

            // Step 4: Test the network.

            double[] toRecall = { -1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, 1.0 };
            double[] recalled = net.Evaluate(toRecall, iterations: 10);

            Debug.Assert(recalled.SequenceEqual(dataSet[0].Input));
        }
    }
}
