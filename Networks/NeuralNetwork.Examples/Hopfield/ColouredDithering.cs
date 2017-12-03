using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Examples.Kohonen;
using NeuralNetwork.Hopfield;
using NeuralNetwork.Kohonen;
using static Mozog.Utils.Math.Math;
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

            //int[] paletteSizes = { 4, 8, 12, 16 };
            int[] paletteSizes = { 4 };
            //int[] radii = { 1, 2 };
            int[] radii = { 1 };
            double alpha = 0.3; // Pixel energy coefficient
            double beta = 0.695; // Local energy coefficient
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

            (palette, kohonenNet) = PaletteExtraction.ExtractPalette(image, paletteSize);

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

            double localNeuronBias = -ColorComponents.Sum(c => Square(C(Col(neuron), Row(neuron), c) - P(Dep(neuron), c)));

            var neighbourhoodSourceNeurons = GetNeighbourhoodSourceNeurons(neuron);
            double globalNeuronBias = ColorComponents.Sum(c =>
            {
                var innerSum = neighbourhoodSourceNeurons.Sum(source =>
                    D(Col(neuron), Row(neuron), Col(source), Row(source)) * C(Col(source), Row(source), c));
                var x = P(Dep(neuron), c);

                return 2 * x * innerSum - Square(radius) * Square(x * x);
            });

            const double pixelNeuronBias = 1.0;
            double neuronBias = alpha * pixelNeuronBias + beta * localNeuronBias + gamma * globalNeuronBias;

            hopfieldNet.SetNeuronBias(neuron, neuronBias);

            //
            // Train synapses
            //

            // Train the "chain" synapses.
            var sourceNeurons = GetChainSourceNeurons(neuron);
            foreach (var source in sourceNeurons)
                InitializeChainSynapse(neuron, source);

            // Train the "neighbourhood" synapes.
            foreach (var source in neighbourhoodSourceNeurons)
                InitializeNeighbourhoodSynapse(neuron, source);
        }

        // Gets the coordinates of a neuron's "chain" source neurons.
        // Along the z-axis.
        private static IEnumerable<int[]> GetChainSourceNeurons(int[] neuron)
            => Enumerable.Range(0, Depth).Where(z => z != Dep(neuron)).Select(z => Pos(Row(neuron), Col(neuron), z)).ToList();

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
                .SelectMany(row => EnumerableExtensions.Range(cmin, cmax, inclusive: true), (row, col) => new { row, col })
                .Where(s => s.row != Row(neuron) || s.col != Col(neuron))
                .Select(s => Pos(s.row, s.col, Dep(neuron)));
        }

        private static void InitializeChainSynapse(int[] neuron, int[] source)
        {
            const double pixelW = -2.0;
            const double localW = 0.0;
            const double globalW = 0.0;

            double synapseWeight = alpha * pixelW + beta * localW + gamma * globalW;

            hopfieldNet.SetSynapseWeight(neuron, source, synapseWeight);
        }

        private static void InitializeNeighbourhoodSynapse(int[] neuron, int[] source)
        {
            const double pixelW = 0.0;
            const double localW = 0.0;
            double globalW = -2 * ColorComponents.Sum(c => D(neuron[1], neuron[0], source[1], source[0]) * Square(P(neuron[2], c)));

            double synapseWeight = alpha * pixelW + beta * localW + gamma * globalW;

            hopfieldNet.SetSynapseWeight(neuron, source, synapseWeight);
        }

        // Color
        private static double C(int x, int y, ColorComponent component)
            => GetColorComponent(image.GetPixel(x, y), component) / (double)byte.MaxValue;

        // Palette
        private static double P(int k, ColorComponent colorComponent)
            => GetColorComponent(palette[k], colorComponent) / (double)byte.MaxValue;

        // Distance
        private static int D(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) <= radius && Math.Abs(y1 - y2) <= radius
                ? (radius - Math.Abs(x1 - x2) + 1) * (radius - Math.Abs(y1 - y2) + 1) : 0;
        }

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
                int colorIndex = PaletteExtraction.EvaluateIndex(kohonenNet, color);
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

        private static byte GetColorComponent(Color color, ColorComponent component)
        {
            switch (component)
            {
                case ColorComponent.Red: return color.R;
                case ColorComponent.Green: return color.G;
                case ColorComponent.Blue: return color.B;
                default: throw new ArgumentException(nameof(component));
            }
        }

        private static IEnumerable<ColorComponent> ColorComponents
            => Enum.GetValues(typeof(ColorComponent)).Cast<ColorComponent>();

        #region Utils

        private static int[] Pos(int r, int c, int d) => new[] { r, c, d };

        // Row
        private static int Row(int[] neuron) => neuron[0];

        // Column
        private static int Col(int[] neuron) => neuron[1];

        // Depth
        private static int Dep(int[] neuron) => neuron[2];

        #endregion // Utils
    }

    enum ColorComponent
    {
        Red,
        Green,
        Blue
    }
}
