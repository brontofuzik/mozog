using System;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.MLP;
using NeuralNetwork.MLP.ActivationFunctions;
using NeuralNetwork.MLP.Backpropagation;
using NeuralNetwork.MLP.ErrorFunctions;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MLP.Tiling
{
    static class Example
    {
        private static IDataSet data;
        private static Network network;

        public static void Run()
        {
            // Step 1: Create the training set.

            data = Data.Create();

            // Step 2: Create the network.

            var architecture = NetworkArchitecture.Feedforward(
                new[] { 100, 4, 9, 100 },
                Activation.Sigmoid,
                Error.MSE);
            network = new Network(architecture);

            // Step 3: Train the network.

            var trainer = new BackpropagationTrainer();
            trainer.WeightsUpdated += LogTrainingProgress;

            var iterations = 500;
            var args = BackpropagationArgs.Stochastic(Optimizer.Adam(0.005), maxIterations: iterations);
            var log = trainer.Train(network, data, args);
            Console.WriteLine(log);

            // Step 4: Test the network.

            var processedTiles = ProcessTiles(Data.OriginalTiles);

            int uniqueTiles = CountUniqueTiles(processedTiles);
            Console.WriteLine($"Number of unique tiles: {uniqueTiles}");

            var processedImage = Data.MergeTiles(processedTiles);
            string filename = $"{Data.BaseName}#{iterations}#{uniqueTiles}.{Data.Extension}";
            processedImage.Save(filename);
        }

        private static void LogTrainingProgress(object sender, TrainingStatus e)
        {
            if (e.Iterations % 100 == 0)
            {
                Console.WriteLine($"{e.Iterations:D5}: {e.Error:F2}");
            }
        }

        private static Bitmap[,] ProcessTiles(Bitmap[,] originalTiles)
        {
            var processedTiles = new Bitmap[Data.Rows, Data.Columns];

            foreach (var c in Data.TileCoordinates)
            {
                var originalTile = originalTiles[c.row, c.column];
                var processedTile = network.EvaluateUnlabeled(originalTile, Data.Encoder);

                int minDifference = Int32.MaxValue;
                Bitmap resultTile = null;
                4.Times(() =>
                {
                    int difference = Data.CalculateTileDifference(processedTile, originalTile);
                    if (difference < minDifference)
                    {
                        minDifference = difference;
                        resultTile = Data.CloneTile(processedTile);
                    }
                    processedTile.RotateFlip(RotateFlipType.Rotate90FlipNone);
                });
                processedTiles[c.row, c.column] = resultTile;
            }

            return processedTiles;
        }

        private static int CountUniqueTiles(Bitmap[,] tiles)
            => Misc.FlattenArray(tiles).Distinct(new TileEqualityComparer()).Count();
    }
}