using System.Drawing.Imaging;
using Mozog.Utils;
using NeuralNetwork.Data;
using ShellProgressBar;

namespace NeuralNetwork.Examples.Kohonen
{
    static class SpaceMapping
    {
        const int DatasetSize = 1_000;
        const int TrainingIterations = 1_000;

        const int BitmapWidth = 500;
        const int BitmapHeight = 500;

        public static void Map_1D_with_1D()
        {
            const int spaceDimension = 1;
            int[] somDimension = { 10 };

            // Step 1: Create the training set

            var data = new DataSet(spaceDimension);
            DatasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(spaceDimension), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(spaceDimension, somDimension);

            // Step 3: Train the network

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.TrainingIteration += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_1D_with_1D.png", ImageFormat.Png);
        }

        public static void Map_2D_with_1D()
        {
            const int spaceDimension = 2;
            int[] somDimension = { 10 };

            // Step 1: Create the training set

            var data = new DataSet(spaceDimension);
            DatasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(spaceDimension), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(spaceDimension, somDimension);

            // Step 3: Train the network

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.TrainingIteration += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_2D_with_1D.png", ImageFormat.Png);
        }

        public static void Map_2D_with_2D()
        {
            const int spaceDimension = 2;
            int[] somDimension = { 10, 10 };

            // Step 1: Create the training set

            var data = new DataSet(spaceDimension);
            DatasetSize.Times(() => data.Add(new LabeledDataPoint(Mozog.Utils.Math.StaticRandom.DoubleArray(spaceDimension), new double[0])));

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(spaceDimension, somDimension);

            // Step 3: Train the network

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.TrainingIteration += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4: Test the network

            var bitmap = net.ToBitmap(BitmapWidth, BitmapHeight);
            bitmap.Save("Map_2D_with_2D.png", ImageFormat.Png);
        }
    }
}
