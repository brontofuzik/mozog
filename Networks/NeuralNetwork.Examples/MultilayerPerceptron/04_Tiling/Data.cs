using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Tiling
{
    static class Data
    {
        public const string BaseName = "litter_disposal";
        public const string Extension = "gif";
        public const string Filename = BaseName + "." + Extension;

        // Image size
        private const int imageWidth = 200;
        private const int imageHeight = 400;

        // Tile size
        public const int TileSize = 10;
        public const int TilePixels = TileSize * TileSize;

        // Mosaic size
        public const int Rows = imageHeight / TileSize;
        public const int Columns = imageWidth / TileSize;

        public static readonly IEncoder<Bitmap, Bitmap> Encoder = new TilingEncoder();

        public static IDataSet Create()
        {          
            var originalImage = new Bitmap(Filename);
            originalImage = CropBitmap(originalImage, 0, 0, imageWidth, imageHeight);

            OriginalTiles = SplitImage(originalImage);

            var data = EncodedData.New(Encoder, TilePixels, TilePixels);
            foreach (var originalTile in OriginalTileList)
            {
                var rotatedTile = CloneTile(originalTile);
                4.Times(() =>
                {
                    data.Add(rotatedTile, originalTile);
                    rotatedTile.RotateFlip(RotateFlipType.Rotate90FlipNone);
                });
            }
            return data;
        }

        public static Bitmap[,] OriginalTiles { get; private set; }

        public static IEnumerable<Bitmap> OriginalTileList
            => TileCoordinates.Select(c => OriginalTiles[c.Item1, c.Item2]);

        public static IEnumerable<(int row, int column, int index)> TileCoordinates
            => Coordinates(Rows, Columns);

        private static Bitmap CropBitmap(Bitmap bitmap, int x, int y, int width, int height)
            => bitmap.Clone(new Rectangle(x, y, width, height), bitmap.PixelFormat);

        public static Bitmap[,] SplitImage(Bitmap picture)
        {
            var tiles = new Bitmap[Rows, Columns];
            foreach (var c in TileCoordinates)
            {
                tiles[c.row, c.column] = CropBitmap(picture, c.column * TileSize, c.row * TileSize, TileSize, TileSize);
            }
            return tiles;
        }

        public static Bitmap MergeTiles(Bitmap[,] tiles)
        {
            var image = new Bitmap(imageWidth, imageHeight);
            var graphics = Graphics.FromImage(image);
            foreach (var c in TileCoordinates)
            {
                graphics.DrawImage(tiles[c.row, c.column], c.column * TileSize, c.row * TileSize, TileSize, TileSize);
            }
            return image;
        }

        public static int CalculateTileDifference(Bitmap tile1, Bitmap tile2)
        {
            var t1 = Data.Encoder.EncodeInput(tile1);
            var t2 = Data.Encoder.EncodeInput(tile2);
            return Vector.HammingDistance(t1, t2);
        }

        public static Bitmap CloneTile(Bitmap tile)
            => tile.Clone(new Rectangle(0, 0, tile.Width, tile.Height), tile.PixelFormat);

        private static IEnumerable<(int row, int column, int index)> Coordinates(int rows, int columns)
        {
            int index = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    yield return (row, column, index);
                }
            }
        }

        private class TilingEncoder : IEncoder<Bitmap, Bitmap>
        {
            public double[] EncodeInput(Bitmap tile)
            {
                var input = new double[TilePixels];
                foreach (var c in Coordinates(TileSize, TileSize))
                {
                    input[c.index] = tile.GetPixel(c.column, c.row).GetBrightness() > 0.5 ? 1.0 : 0.0;
                }
                return input;
            }

            public Bitmap DecodeInput(double[] input)
            {
                // Not needed
                throw new System.NotImplementedException();
            }

            public double[] EncodeOutput(Bitmap tile)
                => EncodeInput(tile);

            // Tile
            public Bitmap DecodeOutput(double[] output)
            {
                var tile = new Bitmap(TileSize, TileSize);
                foreach (var c in Coordinates(TileSize, TileSize))
                {
                    tile.SetPixel(c.column, c.row, output[c.index] >= 0.5 ? Color.White : Color.Black);
                }
                return tile;
            }
        }
    }

    /* TODO
    class INS01BackpropagationTrainingStrategy : BackpropagationTrainingStrategy
    {
        public override IEnumerable<SupervisedTrainingPattern> TrainingPatterns
        {
            get
            {
                // For each tile quadruple in the training set ...
                for (int i = 0; i < TrainingSet.Size; i += 4)
                {
                    double minNetworkError = Double.MaxValue;
                    int trainingPatternIndex = -1;

                    // For each tile orientation in the quadruple ...
                    for (int j = i; j < i + 4; j++)
                    {
                        SupervisedTrainingPattern trainingPattern = TrainingSet[j];
                        double networkError = BackpropagationNetwork.CalculateError(trainingPattern);
                        if (networkError < minNetworkError)
                        {
                            minNetworkError = networkError;
                            trainingPatternIndex = j;
                        }
                    }

                    // Pick the one yielding the least network error.
                    yield return TrainingSet[trainingPatternIndex];
                }
            }
        }
    }
    */

    class TileEqualityComparer : IEqualityComparer<Bitmap>
    {
        public bool Equals(Bitmap tile1, Bitmap tile2)
        {
            for (int i = 0; i < 4; ++i)
            {
                bool equals = true;

                for (int y = 0; y < Data.TileSize; ++y)
                {
                    for (int x = 0; x < Data.TileSize; ++x)
                    {
                        if (tile1.GetPixel(x, y).GetBrightness() != tile2.GetPixel(x, y).GetBrightness())
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

        public int GetHashCode(Bitmap tile) => tile.GetHashCode();
    }
}
