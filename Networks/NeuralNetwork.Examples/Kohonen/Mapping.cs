using System.Drawing.Imaging;
using Mozog.Utils;
using NeuralNetwork.Data;

namespace NeuralNetwork.Examples.Kohonen
{
    static class Mapping
    {
        const int BitmapWidth = 500;
        const int BitmapHeight = 500;

        public static void Test1DTo1D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 100 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var dataSet = new DataSet(1);
            datasetSize.Times(() => dataSet.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(1), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(1, outputSizes);

            // Step 3: Train the network

            net.Train(dataSet, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Test1DTo1D.png", ImageFormat.Png);
        }

        public static void Test1DTo2D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 10, 10 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var dataSet = new DataSet(1);
            datasetSize.Times(() => dataSet.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(1), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(1, outputSizes);

            // Step 3: Train the network

            net.Train(dataSet, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Test1DTo2D.png", ImageFormat.Png);
        }

        public static void Test2DTo1D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 100 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var dataSet = new DataSet(2);
            datasetSize.Times(() => dataSet.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(2), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(2, outputSizes);

            // Step 3: Train the network

            net.Train(dataSet, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Test2DTo1D.png", ImageFormat.Png);
        }

        public static void Test2DTo2D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 10, 10 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var dataSet = new DataSet(2);
            datasetSize.Times(() => dataSet.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(2), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(2, outputSizes);

            // Step 3: Train the network

            net.Train(dataSet, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Test2DTo2D.png", ImageFormat.Png);
        }
    }
}
