using System;
using System.Drawing.Imaging;
using Mozog.Utils;
using NeuralNetwork.Data;
using ShellProgressBar;

namespace NeuralNetwork.Examples.Kohonen
{
    static class Mapping
    {
        const int BitmapWidth = 500;
        const int BitmapHeight = 500;

        public static void Map_1D_with_1D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 10 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var data = new DataSet(1);
            datasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(1), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(1, outputSizes);

            // Step 3: Train the network

            var pbar = new ProgressBar(iterations, "Training...");
            net.BeforeTrainingSet += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{iterations}");
            net.Train(data, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_1D_with_1D.png", ImageFormat.Png);
        }

        public static void Map_2D_with_1D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 10 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var data = new DataSet(2);
            datasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(2), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(2, outputSizes);

            // Step 3: Train the network

            var pbar = new ProgressBar(iterations, "Training...");
            net.BeforeTrainingSet += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{iterations}");
            net.Train(data, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_2D_with_1D.png", ImageFormat.Png);
        }

        public static void Map_2D_with_2D()
        {
            // Parameters

            const int datasetSize = 1000;
            int[] outputSizes = { 10, 10 };
            const int iterations = 1000;

            // Step 1: Create the training set

            var data = new DataSet(2);
            datasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(2), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(2, outputSizes);

            // Step 3: Train the network

            var pbar = new ProgressBar(iterations, "Training...");
            net.BeforeTrainingSet += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{iterations}");
            net.Train(data, iterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_2D_with_2D.png", ImageFormat.Png);
        }
    }
}
