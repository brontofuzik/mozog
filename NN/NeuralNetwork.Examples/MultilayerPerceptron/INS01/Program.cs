using System;
using System.Collections.Generic;
using System.Drawing;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.MultilayerPerceptron.ActivationFunctions;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.INS01
{
    class Program
    {
        static int iterationCount = 500;

        static string fileName = "litter_disposal";
        static string fileExtension = "gif";

        // Picture dimensions.
        static int pictureWidth = 200;
        static int pictureHeight = 400;

        // Tile dimensions.
        static int tileWidth = 10;
        static int tileHeight = 10;

        // Mosaic dimensions.
        static readonly int rowCount = pictureHeight / tileHeight;
        static readonly int columnCount = pictureWidth / tileWidth;

        static readonly int[] topology = {100, 4, 9, 100};

        static Network network;

        static void _Main(string[] args)
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            string sourcePictureFileName = fileName + "." + fileExtension;
            Bitmap sourcePicture = new Bitmap(sourcePictureFileName);
            sourcePicture = CropBitmap(sourcePicture, 0, 0, pictureWidth, pictureHeight);
            Bitmap[,] sourceTiles = SplitPictureIntoTiles(sourcePicture);

            TrainingSet trainingSet = new TrainingSet(tileWidth * tileHeight, tileWidth * tileHeight);
            for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
                {
                    Bitmap sourceTile = sourceTiles[rowIndex, columnIndex];
                    double[] outputVector = TileToVector(sourceTile);

                    for (int i = 0; i < 4; ++i)
                    {
                        double[] inputVector = TileToVector(sourceTile);
                        trainingSet.Add(new SupervisedTrainingPattern(inputVector, outputVector));

                        sourceTile.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
            }

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            network = new Network(topology, new LogisticActivationFunction());

            /* TODO Backprop
            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            // 3.1. Create the (backpropagation) trainer.
            BackpropagationTrainer trainer = new BackpropagationTrainer(trainingSet, null, null);

            // 3.2. Create the (backpropagation) training strategy.
            int maxIterationCount = iterationCount;
            double maxNetworkError = 0.0;
            bool batchLearning = false ;

            double synapseLearningRate = 0.005;
            double connectorMomentum = 0.9;
            
            INS01BackpropagationTrainingStrategy backpropagationTrainingStrategy = new INS01BackpropagationTrainingStrategy(maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum);

            // 3.3. Train the network.
            TrainingLog trainingLog = trainer.Train(network, backpropagationTrainingStrategy);

            // 3.4. Inspect the training log.
            Console.WriteLine("Number of iterations used : " + trainingLog.IterationCount);
            Console.WriteLine("Minimum network error achieved : " + trainingLog.NetworkError);
            */

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            // Gets the target tiles.
            Bitmap[,] targetTiles = SourceTilesToTargetTiles(sourceTiles);

            // Count the number of unique tiles.
            int uniqueTileCount = CountUniqueTiles(targetTiles);
            Console.WriteLine("The number of unique tiles : {0}", uniqueTileCount);

            // Merge the (target) tiles into the (target) picture.
            Bitmap targetPicture = MergeTilesIntoPicture(targetTiles);

            // Save the (target) picture.
            string targetPictureFileName = fileName + "#" + iterationCount + "#" + uniqueTileCount + "." + fileExtension;
            targetPicture.Save(targetPictureFileName);
        }

        /// <summary>
        /// Crops a bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap to crop.</param>
        /// <param name="x">The x coordinate of the upper left corner of the cropped bitmap.</param>
        /// <param name="y">The y coordinate of the upper left corner of the cropped bitmap.</param>
        /// <param name="width">The width of the cropped bitmap.</param>
        /// <param name="height">The height of the cropped bitmap.</param>
        /// <returns>The cropped bitmap.</returns>
        static Bitmap CropBitmap(Bitmap bitmap, int x, int y, int width, int height)
        {
            Rectangle rectangle = new Rectangle(x, y, width, height);
            return bitmap.Clone(rectangle, bitmap.PixelFormat);
        }

        /// <summary>
        /// Splits the (source) picture into tiles.
        /// </summary>
        /// <param name="picture">The picture to split.</param>
        /// <param name="tileWidth">The width of a tile.</param>
        /// <param name="rtileHeight">The height of a tile.</param>
        /// <returns>
        /// The tiles.
        /// </returns>
        static Bitmap[,] SplitPictureIntoTiles(Bitmap picture)
        {
            int rowCount = pictureHeight / tileHeight;
            int columnCount = pictureWidth / tileWidth;
            Bitmap[,] tiles = new Bitmap[rowCount, columnCount];

            for (int rowIndex = 0, y = 0; rowIndex < rowCount; rowIndex++, y += tileHeight)
            {
                for (int columnIndex = 0, x = 0; columnIndex < columnCount; columnIndex++, x += tileWidth)
                {
                    tiles[rowIndex, columnIndex] = CropBitmap(picture, x, y, tileWidth, tileHeight);
                }
            }

            return tiles;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceTiles"></param>
        /// <returns></returns>
        static Bitmap[,] SourceTilesToTargetTiles(Bitmap[,] sourceTiles)
        {
            Bitmap[,] targetTiles = new Bitmap[rowCount, columnCount];
            
            for (int rowIndex = 0; rowIndex < rowCount; ++rowIndex)
            {
                for (int columnIndex = 0; columnIndex < columnCount; ++columnIndex)
                {
                    Bitmap sourceTile = sourceTiles[rowIndex, columnIndex];
                    double[] inputVector = TileToVector(sourceTile);

                    double[] outputVector = network.Evaluate(inputVector);

                    Bitmap outputTile = VectorToTile(outputVector);

                    int minTileDistance = Int32.MaxValue;
                    Bitmap targetTile = null;
                    for (int i = 0; i < 4; ++i)
                    {
                        int tileDistance = CalculateTileDistance(outputTile, sourceTile);
                        if (tileDistance < minTileDistance)
                        {
                            minTileDistance = tileDistance;
                            targetTile = VectorToTile(TileToVector(outputTile));
                        }

                        // Rotate the tile.
                        outputTile.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                    targetTiles[rowIndex, columnIndex] = targetTile;
                }
            }

            return targetTiles;
        }

        /// <summary>
        /// Converts a tile to a vector.
        /// </summary>
        /// <param name="tile">A tile to convert.</param>
        /// <returns>
        /// The vector.
        /// </returns>
        static double[] TileToVector(Bitmap tile)
        {
            double[] vector = new double[tileWidth * tileHeight];

            int i = 0;
            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    vector[i++] = tile.GetPixel(x, y).GetBrightness() > 0.5 ? 1.0 : 0.0;
                }
            }

            return vector;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        static Bitmap VectorToTile(double[] vector)
        {
            Bitmap tile = new Bitmap(tileWidth, tileHeight);

            int i = 0;
            for (int y = 0; y < tileHeight; y++)
            {
                for (int x = 0; x < tileWidth; x++)
                {
                    tile.SetPixel(x, y, vector[i++] >= 0.5 ? Color.White : Color.Black);
                }
            }

            return tile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        static Bitmap MergeTilesIntoPicture(Bitmap[,] tiles)
        {
            Bitmap picture = new Bitmap(pictureWidth, pictureHeight);
            Graphics graphics = Graphics.FromImage(picture);
       
            for (int rowIndex = 0, y = 0; rowIndex < rowCount; rowIndex++, y += tileHeight)
            {
                for (int columnIndex = 0, x = 0; columnIndex < columnCount; columnIndex++, x += tileWidth)
                {
                    graphics.DrawImage(tiles[rowIndex, columnIndex], x, y, tileWidth, tileHeight);
                }

            }

            return picture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tiles"></param>
        /// <returns></returns>
        static int CountUniqueTiles(Bitmap[,] tiles)
        {
            // Convert the 2D tile array into a 1D tile array.
            Bitmap[] tiles1D = new Bitmap[rowCount * columnCount];
            
            int k = 0;
            for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
                {
                    tiles1D[k++] = tiles[rowIndex, columnIndex]; 
                }
            }

            // Count the unique tiles.
            int uniqueTileCount = 0;

            for (int i = 0; i < tiles1D.Length; ++i)
            {
                bool tileIsUnique = true;
                for (int j = 0; j < i; ++j)
                {
                    if (TileEquals(tiles1D[j], tiles1D[i]))
                    {
                        tileIsUnique = false;
                        break;
                    }
                }
                if (tileIsUnique)
                {
                    ++uniqueTileCount;
                }
            }

            return uniqueTileCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bitmap1"></param>
        /// <param name="bitmap2"></param>
        /// <returns></returns>
        static bool TileEquals(Bitmap tile1, Bitmap tile2)
        {
            for (int i = 0; i < 4; ++i)
            {
                bool equals = true;

                for (int y = 0; y < tileHeight; ++y)
                {
                    for (int x = 0; x < tileWidth; ++x)
                    {
                        if (tile1.GetPixel(x,y).GetBrightness() != tile2.GetPixel(x,y).GetBrightness())
                        {
                            equals = false;
                            break;
                        }
                    }

                    if (!equals)
                    {
                        break;
                    }
                }

                if (equals)
                {
                    return true;
                }

                // Rotate tehe 1st tile.
                tile1.RotateFlip(RotateFlipType.Rotate90FlipNone);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tile1"></param>
        /// <param name="tile2"></param>
        /// <returns></returns>
        static int CalculateTileDistance(Bitmap tile1, Bitmap tile2)
        {
            double[] tile1Vector = TileToVector(tile1);
            double[] tile2Vector = TileToVector(tile2);

            int tileDistance = 0;

            for (int i = 0; i < tile1Vector.Length; ++i)
            {
                if (tile1Vector[i] != tile2Vector[i])
                {
                    ++tileDistance;
                }
            }

            return tileDistance;
        }
    }
}