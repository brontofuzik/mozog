using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NeuralNetwork.HopfieldNet;

namespace NeuralNetwork.Examples.HopfieldNet.INS03
{
    class GrayscaleDitheringNetwork
    {
        private static Bitmap image;
        private static int radius;
        private static double alpha;

        private static HopfieldNetwork net;

        public static Bitmap DitherImage(Bitmap image, int radius, double alpha)
        {
            GrayscaleDitheringNetwork.image = image;
            GrayscaleDitheringNetwork.radius = radius;
            GrayscaleDitheringNetwork.alpha = alpha;

            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            net = new HopfieldNetwork(rows: image.Height, cols: image.Width, sparse: true,
                activationFunction: Activation, topology_pos: Topology);

            // Step 3: train the network.

            net.Initialize(InitNeuronBias, InitSynapseWeight);

            // Step 4: Use the network.

            return Evaluate(net, image);
        }

        private static double Activation(double input, double progress)
        {
            const double initialLambda = 1.0;
            const double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * progress;
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        private static IEnumerable<Position> Topology(Position p, HopfieldNetwork net)
        {
            var sourceNeurons = new List<Position>();

            // Limits
            int xmin = Math.Max(p.Col - radius, 0);
            int xmax = Math.Min(p.Col + radius, net.Cols.Value - 1);
            int ymin = Math.Max(p.Row - radius, 0);
            int ymax = Math.Min(p.Row + radius, net.Rows.Value - 1);

            for (int sourceY = ymin; sourceY <= ymax; sourceY++)
                for (int sourceX = xmin; sourceX <= xmax; sourceX++)
                {
                    if (sourceX != p.Col || sourceY != p.Row)
                        sourceNeurons.Add(new Position(sourceX, sourceY));
                }

            return sourceNeurons;
        }

        private static double InitNeuronBias(Position p, HopfieldNetwork net)
        {
            double localBias = 2 * C(p) - 1;
            var sum = net.Topology_Pos(p).Select(s => C(s) * D(p, s)).Sum();
            double globalBias = 2 * sum - radius * radius;
            return alpha * localBias + (1 - alpha) * globalBias;
        }

        private static double InitSynapseWeight(Position p, Position sourceP, HopfieldNetwork net)
        {
            double localW = 0.0;
            double globalW = -2 * D(p, sourceP);
            return alpha * localW + (1 - alpha) * globalW;
        }

        private static double C(Position p) => image.GetPixel(p.Col, p.Row).GetBrightness();

        private static int D(Position p1, Position p2)
            => Math.Max(radius - Math.Abs(p1.Col - p2.Col) + 1, 0) * Math.Max(radius - Math.Abs(p1.Row - p2.Row) + 1, 0);

        #region Evaluation

        private static Bitmap Evaluate(HopfieldNetwork net, Bitmap image)
        {
            var originalPixels = ImageToPixels(image);
            var ditheredPixels = net.Evaluate(originalPixels, 20);
            return PixelsToImage(ditheredPixels);
        }

        private static double[] ImageToPixels(Bitmap image)
        {
            double[] pixels = new double[net.Neurons];
            foreach (Point p in GetPoints())
            {
                int neuron = CoordinatesToIndex(p);
                pixels[neuron] = image.GetPixel(p.X, p.Y).GetBrightness();
            }
            return pixels;
        }

        private static Bitmap PixelsToImage(double[] pixels)
        {
            var image = new Bitmap(Width, Height);
            foreach (var p in GetPoints())
            {
                int neuron = CoordinatesToIndex(p);
                var color = pixels[neuron] >= 0.5 ? Color.White : Color.Black;
                image.SetPixel(p.X, p.Y, color);
            }
            return image;
        }

        private static IEnumerable<Point> GetPoints()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    yield return new Point(x, y);
        }

        private static int CoordinatesToIndex(Point c) => c.Y * Width + c.X;

        private static int Width => image.Width;

        private static int Height => image.Height;

        #endregion // Evaluation
    }

    struct Point
    {
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}
