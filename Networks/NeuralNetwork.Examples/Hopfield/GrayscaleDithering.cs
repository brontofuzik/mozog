using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Hopfield;
using static Mozog.Utils.Math.Math;

namespace NeuralNetwork.Examples.Hopfield
{
    class GrayscaleDithering
    {
        const string ImageDir = @"..\..\..\images\";

        // Params
        private static Bitmap image;
        private static int radius;
        private static double alpha;

        private static HopfieldNetwork net;

        public static void Run()
        {
            string imageName = "lenna";

            var radii = new[] { 0, 1, 2, 3, 4 };
            //var radii = new[] { 1 };
            var alphas = new[] { 1.0, 0.995, 0.99, 0.985, 0.98 };
            //var alphas = new[] { 0.995 };

            foreach (int radius in radii)
            foreach (double alpha in alphas)
                DitherImage(imageName, radius, alpha);
        }

        private static void DitherImage(string imageName, int radius, double alpha)
        {
            Console.Write($"DitherImage(radius: {radius}, alpha: {alpha:F3})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var ditheredImage = DitherImage(originalImage, radius, alpha);

            ditheredImage.Save($"{imageName}_{radius}_{alpha:F3}.png");

            Console.WriteLine("Done");
        }

        public static Bitmap DitherImage(Bitmap image, int radius, double alpha)
        {
            // Params
            GrayscaleDithering.image = image;
            GrayscaleDithering.radius = radius;
            GrayscaleDithering.alpha = alpha;

            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            net = new HopfieldNetwork(new[] {image.Height, image.Width},
                sparse: true, activation: Activation, topology: Topology);

            // Step 3: Train (initialize) the network.

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

        private static IEnumerable<int[]> Topology(int[] neuron, HopfieldNetwork net)
        {
            // Bounds
            int rmin = Math.Max(Row(neuron) - radius, 0);
            int rmax = Math.Min(Row(neuron) + radius, net.Dimensions[0] - 1);
            int cmin = Math.Max(Col(neuron) - radius, 0);
            int cmax = Math.Min(Col(neuron) + radius, net.Dimensions[1] - 1);

            return EnumerableExtensions.Range(rmin, rmax, inclusive: true)
                .SelectMany(row => EnumerableExtensions.Range(cmin, cmax, inclusive: true), (row, col) => new {row, col})
                .Where(s => s.row != Row(neuron) || s.col != Col(neuron))
                .Select(s => Pos(s.row, s.col));
        }

        private static int Height => image.Height;

        private static int Width => image.Width;

        #region Initialization

        private static double InitNeuronBias(int[] neuron, HopfieldNetwork net)
        {
            double localB = 2 * C(neuron) - 1;
            var sum = net.Topology(neuron).Select(source => C(source) * D(neuron, source)).Sum();
            double globalB = 2 * sum - Square(radius);
            return alpha * localB + (1 - alpha) * globalB;
        }

        // Colour
        private static double C(int[] n) => image.GetPixel(x: Col(n), y: Row(n)).GetBrightness();

        private static double InitSynapseWeight(int[] neuron, int[] source, HopfieldNetwork net)
        {
            double localW = 0.0;
            double globalW = -2 * D(neuron, source);
            return alpha * localW + (1 - alpha) * globalW;
        }

        // Distance
        private static int D(int[] n1, int[] n2)
            => Math.Max(radius - Math.Abs(Col(n1) - Col(n2)) + 1, 0) * Math.Max(radius - Math.Abs(Row(n1) - Row(n2)) + 1, 0);

        #endregion // Initialization

        #region Evaluation

        private static Bitmap Evaluate(HopfieldNetwork net, Bitmap image)
        {
            var originalPixels = ImageToPixels(image);
            var ditheredPixels = net.Evaluate(originalPixels, iterations: 20);
            return PixelsToImage(ditheredPixels);
        }

        private static double[] ImageToPixels(Bitmap image)
            => Pixels(image).Select(p => (double)image.GetPixel(p.pixel.X, p.pixel.Y).GetBrightness()).ToArray();

        private static Bitmap PixelsToImage(double[] pixels)
        {
            var image = new Bitmap(Width, Height);
            foreach (var p in Pixels(image))
            {
                var color = pixels[p.index] >= 0.5 ? Color.White : Color.Black;
                image.SetPixel(p.pixel.X, p.pixel.Y, color);
            }
            return image;
        }

        public static IEnumerable<(Pixel pixel, int index)> Pixels(Bitmap image)
        {
            int index = 0;
            for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
                yield return (new Pixel(x, y), index++);
        }

        #endregion // Evaluation

        #region Utils

        private static int[] Pos(int r, int c) => new[] { r, c };

        // Row
        private static int Row(int[] neuron) => neuron[0];

        // Column
        private static int Col(int[] neuron) => neuron[1];

        #endregion // Utils
    }

    internal struct Pixel
    {
        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }

        public int Y { get; }
    }
}
