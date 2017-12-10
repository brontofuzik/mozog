using System.Linq;
using System.Diagnostics;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Hopfield;

namespace NeuralNetwork.Examples.Hopfield
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

            var net = new HopfieldNetwork(new[] {dataSet.InputSize});

            // Step 3: Train the network.

            net.Train(dataSet);

            // Step 4: Test the network.

            var corrupted = (double[])dataSet[0].Input.Clone();
            3.Times(() => Corrupt(corrupted));

            double[] restored = net.Evaluate(corrupted, iterations: 10);

            Debug.Assert(restored.SequenceEqual(dataSet[0].Input));
        }

        private static void Corrupt(double[] array)
        {
            var index = StaticRandom.Int(array.Length);
            array[index] = array[index] == 1.0 ? -1.0 : 1.0;
        }
    }
}
