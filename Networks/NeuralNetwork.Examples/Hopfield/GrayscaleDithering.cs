using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NeuralNetwork.Hopfield;

namespace NeuralNetwork.Examples.Hopfield
{
    class GrayscaleDithering
    {
        const string ImageDir = @"..\..\..\images\";

        public static void Run()
        {
            string imageName = "lenna";

            //var radii = new[] { 0, 1, 2, 3, 4 };
            var radii = new[] { 1 };
            //var alphas = new[] { 1.0, 0.995, 0.99, 0.985, 0.98 };
            var alphas = new[] { 0.995 };

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

        private static Bitmap image;
        private static int radius;
        private static double alpha;

        private static HopfieldNetwork net;

        public static Bitmap DitherImage(Bitmap image, int radius, double alpha)
        {
            GrayscaleDithering.image = image;
            GrayscaleDithering.radius = radius;
            GrayscaleDithering.alpha = alpha;

            // Step 1: Create the training set.

            // Do nothing.

            // Step 2: Create the network.

            net = NeuralNetwork.Hopfield.HopfieldNetwork.Build2DNetwork(rows: image.Height, cols: image.Width, sparse: true,
                activation: Activation, topology: Topology);

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

        private static IEnumerable<int[]> Topology(int[] p, HopfieldNetwork net)
        {
            var sourceNeurons = new List<int[]>();

            // Limits
            int xmin = Math.Max(p[1] - radius, 0);
            int xmax = Math.Min(p[1] + radius, net.Dimensions[1] - 1);
            int ymin = Math.Max(p[0] - radius, 0);
            int ymax = Math.Min(p[0] + radius, net.Dimensions[0] - 1);

            for (int sourceY = ymin; sourceY <= ymax; sourceY++)
                for (int sourceX = xmin; sourceX <= xmax; sourceX++)
                {
                    if (sourceX != p[1] || sourceY != p[0])
                        sourceNeurons.Add(new[] { sourceY, sourceX});
                }

            return sourceNeurons;
        }

        #region Initialization

        private static double InitNeuronBias(int[] p, HopfieldNetwork net)
        {
            double localBias = 2 * C(p) - 1;
            var sum = net.Topology(p).Select(s => C(s) * D(p, s)).Sum();
            double globalBias = 2 * sum - radius * radius;
            return alpha * localBias + (1 - alpha) * globalBias;
        }

        private static double InitSynapseWeight(int[] p, int[] sourceP, HopfieldNetwork net)
        {
            double localW = 0.0;
            double globalW = -2 * D(p, sourceP);
            return alpha * localW + (1 - alpha) * globalW;
        }

        private static double C(int[] p) => image.GetPixel(x: p[1], y: p[0]).GetBrightness();

        private static int D(int[] p1, int[] p2)
            => Math.Max(radius - Math.Abs(p1[1] - p2[1]) + 1, 0) * Math.Max(radius - Math.Abs(p1[0] - p2[0]) + 1, 0);

        #endregion // Initialization

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
            foreach (Pixel p in GetPoints())
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

        private static IEnumerable<Pixel> GetPoints()
        {
            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                yield return new Pixel(x, y);
        }

        private static int CoordinatesToIndex(Pixel c) => c.Y * Width + c.X;

        private static int Width => image.Width;

        private static int Height => image.Height;

        #endregion // Evaluation
    }

    struct Pixel
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
