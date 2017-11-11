using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NeuralNetwork.HopfieldNet;

namespace NeuralNetwork.Examples.HopfieldNet.INS03
{
    class GrayscaleDitheringNetwork
    {
        public static Bitmap DitherImage(Bitmap image, int radius, double alpha)
        {
            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            var net = new GrayscaleDitheringNetwork(image);

            // Step 3: train the network.

            net.InitializeNet(image, radius, alpha);

            // Step 4: Use the network.

            return net.Evaluate(image);
        }

        private int width;
        private int height;
        private HopfieldNetwork net;

        private GrayscaleDitheringNetwork(Bitmap image)
        {
            width = image.Width;
            height = image.Height;
            net = new HopfieldNetwork(width * height, true, Activation);
        }

        private static double Activation(double input, double progress)
        {
            const double initialLambda = 1.0;
            const double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * progress; 
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        private int NeuronCount => net.Neurons;

        #region Training

        public void InitializeNet(Bitmap image, int radius, double alpha)
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    InitializeNeuron(new Point(x, y), image, radius, alpha);
        }

        private void InitializeNeuron(Point p, Bitmap image, int radius, double alpha)
        {
            var sourceNeurons = GetSourceNeurons(p, radius);

            // Initialize bias.
            double localBias = 2 * C(image, p) - 1;
            var sum = sourceNeurons.Select(s => C(image, s) * D(radius, p, s)).Sum();
            double globalBias = 2 * sum - radius * radius;
            double bias = alpha * localBias + (1 - alpha) * globalBias;
            SetNeuronBias(p, bias);

            // Initialize synapses.
            foreach (Point sourceNeuron in sourceNeurons)
                InitializeSynapse(p, sourceNeuron, radius, alpha);
        }

        private IEnumerable<Point> GetSourceNeurons(Point p, int radius)
        {
            var sourceNeurons = new List<Point>();

            // Limits
            int sourceXmin = Math.Max(p.X - radius, 0);
            int sourceXmax = Math.Min(p.X + radius, width - 1);
            int sourceYmin = Math.Max(p.Y - radius, 0);
            int sourceYmax = Math.Min(p.Y + radius, height - 1);

            for (int sourceY = sourceYmin; sourceY <= sourceYmax; sourceY++)
                for (int sourceX = sourceXmin; sourceX <= sourceXmax; sourceX++)
                {
                    if (sourceX == p.X && sourceY == p.Y) continue;
                    sourceNeurons.Add(new Point(sourceX, sourceY));
                }

            return sourceNeurons;
        }

        private void InitializeSynapse(Point p, Point sourceP, int radius, double alpha)
        {
            double localW = 0.0;
            double globalW = -2 * D(radius, p, sourceP);
            double weight = alpha * localW + (1 - alpha) * globalW;

            SetSynapseWeight(p, sourceP, weight);
        }

        private void SetNeuronBias(Point p, double bias)
        {
            int n = CoordinatesToIndex(p);
            net.SetNeuronBias(n, bias);
        }

        private void SetSynapseWeight(Point p, Point sourceP, double weight)
        {
            int n = CoordinatesToIndex(p);
            int sourceN = CoordinatesToIndex(sourceP);
            net.SetSynapseWeight(n, sourceN, weight);
        }

        private int CoordinatesToIndex(Point c) => c.Y * width + c.X;

        // Unused
        private Point IndexToCoordinates(int index) => new Point(index % width, index / width);

        private double C(Bitmap image, Point p) => image.GetPixel(p.X, p.Y).GetBrightness();

        private int D(int radius, Point p1, Point p2)
            => Math.Max(radius - Math.Abs(p1.X - p2.X) + 1, 0) * Math.Max(radius - Math.Abs(p1.Y - p2.Y) + 1, 0);

        #endregion // Training

        #region Evaluation

        public Bitmap Evaluate(Bitmap image)
        {
            var originalPixels = ImageToPixels(image);
            var ditheredPixels = net.Evaluate(originalPixels, 20);
            return PixelsToImage(ditheredPixels);
        }

        private double[] ImageToPixels(Bitmap image)
        {
            double[] pixels = new double[NeuronCount];
            foreach (Point p in GetPoints())
            {
                int neuron = CoordinatesToIndex(p);
                pixels[neuron] = image.GetPixel(p.X, p.Y).GetBrightness();
            }
            return pixels;
        }

        private Bitmap PixelsToImage(double[] pixels)
        {
            var image = new Bitmap(width, height);
            foreach (Point p in GetPoints())
            {
                int neuron = CoordinatesToIndex(p);
                var color = pixels[neuron] >= 0.5 ? Color.White : Color.Black;
                image.SetPixel(p.X, p.Y, color);
            }
            return image;
        }

        private IEnumerable<Point> GetPoints()
        {
            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    yield return new Point(x, y);
        }

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
