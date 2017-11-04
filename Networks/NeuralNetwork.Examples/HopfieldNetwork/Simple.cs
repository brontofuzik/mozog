using System;
using Mozog.Utils.Math;
using NeuralNetwork.Data;

namespace NeuralNetwork.Examples.HopfieldNetwork
{
    class Simple
    {
        /// <summary>
        /// Tests the Hopfield network.
        /// </summary>
        public static void Run()
        {
            Console.WriteLine("TestHopfieldNetwork");
            Console.WriteLine("===================");

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            var dataSet = new DataSet(10, 0)
            {
                new LabeledDataPoint(new[] { 1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, -1.0 }, new double[0])
            };

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int neuronCount = dataSet.InputSize;
            NeuralNetwork.HopfieldNetwork.HopfieldNetwork defaultNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(neuronCount, sparse: false);

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            defaultNetwork.Train(dataSet);

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            double[] patternToRecall = { -1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, 1.0 };
            int iterationCount = 10;
            double[] recalledPattern = defaultNetwork.Evaluate(patternToRecall, iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine();
        }
    }
}
