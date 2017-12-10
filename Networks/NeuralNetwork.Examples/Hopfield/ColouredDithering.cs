using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Examples.Kohonen;
using NeuralNetwork.Hopfield;
using NeuralNetwork.Kohonen;
using ShellProgressBar;
using static Mozog.Utils.Math.Math;
using static NeuralNetwork.Hopfield.HopfieldNetwork;
using Math = System.Math;

namespace NeuralNetwork.Examples.Hopfield
{
    class ColourDithering
    {
        const string ImageDir = @"..\..\..\images\";

        // Params
        private static Bitmap image;
        private static int paletteSize;
        private static int radius;
        private static double alpha;
        private static double beta;
        private static double gamma;

        private static Color[] palette;
        private static KohonenNetwork kohonenNet;
        private static HopfieldNetwork hopfieldNet;

        // TODO
        //private static int NeuronCount => _height * _width * _depth;

        public static void Run()
        {
            string imageName = "lenna-col";

            //int[] paletteSizes = { 2, 4, 8 };
            int[] paletteSizes = { 12 };
            //int[] radii = { 1, 2, 3, 4 };
            int[] radii = { 1, 2 };

            double alpha = 0.3;   // Pixel energy coefficient
            double beta  = 0.695; // Local energy coefficient
            double gamma = 0.005; // Global energy coefficient

            foreach (int paletteSize in paletteSizes)
            foreach (int radius in radii)
                DitherImage(imageName, paletteSize, radius, alpha, beta, gamma);
        }

        static void DitherImage(string imageName, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            Console.Write($"DitherImage(paletteSize: {paletteSize}, radius: {radius}, alpha: {alpha:F3}, beta: {beta:F3}, gamma: {gamma:F3})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var ditheredImage = DitherImage(originalImage, paletteSize, radius, alpha, beta, gamma);

            ditheredImage.Save($"{imageName}_{paletteSize}_{radius}_{alpha:F3}_{beta:F3}_{gamma:F3}.png");

            Console.WriteLine("Done");
        }

        public static Bitmap DitherImage(Bitmap image, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            // Params
            ColourDithering.image = image;
            ColourDithering.paletteSize = paletteSize;
            ColourDithering.radius = radius;
            ColourDithering.alpha = alpha;
            ColourDithering.beta = beta;
            ColourDithering.gamma = gamma;

            (kohonenNet, palette) = ColourQuantization.QuantizeColours2(image, paletteSize);

            // Step 1: Build the training set.

            // Do nothing.

            // Step 2: Build the network.

            hopfieldNet = new HopfieldNetwork(new[] {image.Height, image.Width, paletteSize}, sparse: true, activation: Activation);

            // Step 3: Train (initialize) the network.

            Initialize();

            // Step 4: Use the network.

            var ditheredImage = Evaluate(hopfieldNet, image);

            return ditheredImage;
        }

        private static double Activation(double input, double progress)
        {
            const double initialLambda = 1.0;
            const double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * progress;
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        private static int Height => image.Height;

        private static int Width => image.Width;

        private static int Depth => paletteSize;

        #region Initialization

        private static void Initialize()
        {
            const int tickInterval = 1_000;
            var options = new ProgressBarOptions { DisplayTimeInRealTime = false };
            var pbar = new ProgressBar(hopfieldNet.Neurons / tickInterval, "Initializing...", options);
            hopfieldNet.NeuronInitialized += (sender, args) =>
            {
                if (args.NeuronIndex % tickInterval == 0)
                    pbar.Tick($"Neuron {args.NeuronIndex}/{hopfieldNet.Neurons}");
            };

            for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
            for (int z = 0; z < Depth; z++)
                InitializeNeuron(new[] { y, x, z });
        }

        private static void InitializeNeuron(int[] neuron)
        {
            //
            // Train bias
            //

            const double pixelB = 1.0;

            double localB = -ColorComponents.Sum(k => Square(C(neuron, k) - P(neuron, k)));

            var sources = GetNeighbourhoodSourceNeurons(neuron);
            double globalB = ColorComponents.Sum(k =>
            {
                var p = P(neuron, k);
                var sum = sources.Sum(source => D(neuron, source) * C(source, k));
                return 2 * p * sum - Square(radius) * Square(p);
            });


            double bias = alpha * pixelB + beta * localB + gamma * globalB;

            hopfieldNet.SetNeuronBias(neuron, bias);

            //
            // Train synapses
            //

            // Train the "chain" synapses.
            var sourceNeurons = GetChainSourceNeurons(neuron);
            foreach (var source in sourceNeurons)
                InitializeChainSynapse(neuron, source);

            // Train the "neighbourhood" synapes.
            foreach (var source in sources)
                InitializeNeighbourhoodSynapse(neuron, source);
        }

        // Gets the coordinates of a neuron's "chain" source neurons.
        // Along the z-axis.
        private static IEnumerable<int[]> GetChainSourceNeurons(int[] neuron)
            => Enumerable.Range(0, Depth).Where(z => z != Z(neuron)).Select(z => Position(Row(neuron), Col(neuron), z)).ToList();

        // Gets the coordinates of a neuron's "neighbourhood" source neurons.
        // Along the x1 and y-axes.
        private static IEnumerable<int[]> GetNeighbourhoodSourceNeurons(int[] neuron)
        {
            // Bounds
            int rmin = Math.Max(Row(neuron) - radius, 0);
            int rmax = Math.Min(Row(neuron) + radius, hopfieldNet.Dimensions[0] - 1);
            int cmin = Math.Max(Col(neuron) - radius, 0);
            int cmax = Math.Min(Col(neuron) + radius, hopfieldNet.Dimensions[1] - 1);

            return EnumerableExtensions.Range(rmin, rmax, inclusive: true)
                .SelectMany(row => EnumerableExtensions.Range(cmin, cmax, inclusive: true), (row, col) => new {row, col})
                .Where(s => s.row != Row(neuron) || s.col != Col(neuron))
                .Select(s => Position(s.row, s.col, Z(neuron)))
                .ToList();
        }

        private static void InitializeChainSynapse(int[] neuron, int[] source)
        {
            InitializeSynapse(neuron, source, pixelW: -2.0, localW: 0.0, globalW: 0.0);
        }

        private static void InitializeNeighbourhoodSynapse(int[] neuron, int[] source)
        {
            double globalW = -2 * ColorComponents.Sum(k => D(neuron, source) * Square(P(neuron, k)));
            InitializeSynapse(neuron, source, pixelW: 0.0, localW: 0.0, globalW: globalW);
        }

        private static void InitializeSynapse(int[] neuron, int[] source, double pixelW, double localW, double globalW)
        {
            double weight = alpha * pixelW + beta * localW + gamma * globalW;
            hopfieldNet.SetSynapseWeight(neuron, source, weight);
        }

        // Color
        private static double C(int[] neuron, ColorComponent component)
            => GetIntensity(image.GetPixel(Col(neuron), Row(neuron)), component);

        // Palette
        private static double P(int[] neuron, ColorComponent component)
            => GetIntensity(palette[Z(neuron)], component);

        // Distance
        private static int D(int[] n1, int[] n2)
            => Math.Max(radius - Math.Abs(Col(n1) - Col(n2)) + 1, 0) * Math.Max(radius - Math.Abs(Row(n1) - Row(n2)) + 1, 0);

        #endregion // Initialization

        #region Evaluation

        private static Bitmap Evaluate(HopfieldNetwork net, Bitmap image)
        {
            var original = ImageToVector(image);
            var dithered = net.Evaluate(original, iterations: 20);
            return VectorToImage(dithered);
        }

        private static double[] ImageToVector(Bitmap image)
        {
            var vector = new double[Height * Width * Depth];

            foreach (var p in GrayscaleDithering.Pixels(image))
            {
                var color = image.GetPixel(p.pixel.X, p.pixel.X);
                int colorIndex = ColourQuantization.EvaluateColorToNeuron(kohonenNet, color)[0];
                var colorVector = Vector.IndexToVector(colorIndex, Depth);
                vector.ReplaceSubarray(p.index * Depth, colorVector);
            }

            return vector;
        }

        private static Bitmap VectorToImage(double[] vector)
        {
            var image = new Bitmap(Width, Height);

            foreach (var p in GrayscaleDithering.Pixels(image))
            {
                var colorVector = vector.GetSubarray(p.index * Depth, Depth);
                int colorIndex = Vector.VectorToIndex(colorVector);
                var color = palette[colorIndex];
                image.SetPixel(p.pixel.X, p.pixel.Y, color);
            }

            return image;
        }

        private static int NeuronPositionToIndex(int row, int col, int depth)
            => row * (Width * Depth) + col * Depth + depth;

        #endregion // Evaluation

        private static double GetIntensity(Color color, ColorComponent component)
        {
            switch (component)
            {
                case ColorComponent.Red: return color.R / (double)byte.MaxValue;
                case ColorComponent.Green: return color.G / (double)byte.MaxValue;
                case ColorComponent.Blue: return color.B / (double)byte.MaxValue;
                default: throw new ArgumentException(nameof(component));
            }
        }

        private static IEnumerable<ColorComponent> ColorComponents
            => Enum.GetValues(typeof(ColorComponent)).Cast<ColorComponent>();
    }

    enum ColorComponent
    {
        Red,
        Green,
        Blue
    }
}
