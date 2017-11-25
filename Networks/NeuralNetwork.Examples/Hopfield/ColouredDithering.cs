using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using NeuralNetwork.Examples.Kohonen;
using NeuralNetwork.Hopfield;
using NeuralNetwork.Kohonen;
using static Mozog.Utils.Math.Math;

namespace NeuralNetwork.Examples.Hopfield
{
    class ColourDithering
    {
        const string ImageDir = @"..\..\..\images\";

        private static KohonenNetwork _kohonenNet;
        private static HopfieldNetwork _hopfieldNet;
        private static Color[] _palette;

        // Dimensions
        private static int _width;
        private static int _height;
        private static int _depth;

        private static int NeuronCount => _width * _height * _depth;

        public static void Run()
        {
            string imageName = "lenna-col";

            int[] paletteSizes = { 4, 8, 12, 16 };
            int[] radii = { 1, 2 };
            double alpha = 0.3;
            double beta = 0.695;
            double gamma = 0.005;

            foreach (int paletteSize in paletteSizes)
            foreach (int radius in radii)
                DitherImage(imageName, paletteSize, radius, alpha, beta, gamma);
        }

        // alpha - pixel energy coefficient
        // beta - local energy coefficient
        // gamma - global energy coefficient
        static void DitherImage(string imageName, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            Console.Write($"DitherImage(paletteSize: {paletteSize}, radius: {radius}, alpha: {alpha:F3}, beta: {beta:F3}, gamma: {gamma:F3})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var ditheredImage = DitherImage(originalImage, paletteSize, radius, alpha, beta, gamma);

            ditheredImage.Save($"{imageName}_{paletteSize}_{radius}_{alpha:F3}_{beta:F3}_{gamma:F3}.png");

            Console.WriteLine("Done");
        }

        public static Bitmap DitherImage(Bitmap originalImage, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            if (radius < 0)
                throw new ArgumentOutOfRangeException(nameof(radius), "The radius must be non-negative.");

            if (alpha < 0.0 || 1.0 < alpha)
                throw new ArgumentOutOfRangeException(nameof(alpha), "The alpha must be within the range [0, 1] (inclusive).");

            if (beta < 0.0 || 1.0 < beta)
                throw new ArgumentOutOfRangeException(nameof(beta), "The betamust be within the range [0, 1] (inclusive).");

            if (gamma < 0.0 || 1.0 < gamma)
                throw new ArgumentOutOfRangeException(nameof(gamma), "The gamma must be within the range [0, 1] (inclusive).");

            // Step 1: Build the training set.

            // Do nothing.

            // Step 2: Build the network.

            _hopfieldNet = HopfieldNetwork.Build3DNetwork(originalImage.Height, originalImage.Width, paletteSize, sparse: true, activation: Activation);
            _width = originalImage.Width;
            _height = originalImage.Height;
            _depth = paletteSize;

            // Step 3: Train the network.

            Train(originalImage, radius, alpha, beta, gamma);

            // Step 4: Use the network.

            var ditheredImage = EvaluateHopfield(_hopfieldNet, originalImage);

            return ditheredImage;
        }

        private static double Activation(double input, double progress)
        {
            const double initialLambda = 1.0;
            const double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * progress;
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        #region Initialization

        private static void Train(Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            _palette = PaletteExtraction.ExtractPalette(image, _depth);

            // Train the neurons.
            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                    for (int z = 0; z < _depth; z++)
                        TrainNeuron(new[] { y, x, z }, image, radius, alpha, beta, gamma);
        }

        private static void TrainNeuron(int[] neuron, Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            //
            // Train bias
            //

            double localNeuronBias = -ColorComponents.Sum(c => Square(C_ijb(image, X(neuron), Y(neuron), c) - P_kb(Z(neuron), c)));

            var neighbourhoodSourceNeurons = GetNeighbourhoodSourceNeurons(neuron, radius);
            double globalNeuronBias = ColorComponents.Sum(c =>
            {
                var innerSum = neighbourhoodSourceNeurons.Sum(source =>
                    D_ijkl(radius, X(neuron), Y(neuron), X(source), Y(source)) * C_ijb(image, X(source), Y(source), c));
                var x = P_kb(Z(neuron), c);

                return 2 * x * innerSum - Square(radius) * Square(x * x);
            });

            const double pixelNeuronBias = 1.0;
            double neuronBias = alpha * pixelNeuronBias + beta * localNeuronBias + gamma * globalNeuronBias;

            SetNeuronBias(neuron, neuronBias);

            //
            // Train synapses
            //

            // Train the "chain" synapses.
            var sourceNeurons = GetChainSourceNeurons(neuron);
            foreach (var source in sourceNeurons)
                TrainChainSynapse(neuron, source, image, radius, alpha, beta, gamma);

            // Train the "neighbourhood" synapes.
            foreach (var source in neighbourhoodSourceNeurons)
                TrainNeighbourhoodSynapse(neuron, source, image, radius, alpha, beta, gamma);
        }

        // Gets the coordinates of a neuron's "chain" source neurons.
        // Along the z-axis.
        private static IEnumerable<int[]> GetChainSourceNeurons(int[] position)
        {
            var neurons = new List<int[]>();

            for (int sourceZ = 0; sourceZ < _depth; sourceZ++)
            {
                if (sourceZ == Z(position)) continue;
                neurons.Add(P(Y(position), X(position), sourceZ));
            }

            return neurons;
        }

        // Gets the coordinates of a neuron's "neighbourhood" source neurons.
        // Along the x and y-axes.
        private static IEnumerable<int[]> GetNeighbourhoodSourceNeurons(int[] position, int radius)
        {
            var neurons = new List<int[]>();

            // Calculate the limits.
            int ymin = Math.Max(Y(position) - radius, 0);
            int ymax = Math.Min(Y(position) + radius, _height - 1);
            int xmin = Math.Max(X(position) - radius, 0);
            int xmax = Math.Min(X(position) + radius, _width - 1);

            for (int sourceY = ymin; sourceY <= ymax; sourceY++)
                for (int sourceX = xmin; sourceX <= xmax; sourceX++)
                {
                    if (sourceX != X(position) || sourceY != Y(position))
                        neurons.Add(P(sourceY, sourceX, Z(position)));
                }

            return neurons;
        }

        private static void TrainChainSynapse(int[] neuron, int[] source, Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            double pixelSynapseWeight = -2.0;
            double localSynapseWeight = 0.0;
            double globalSynapseWeight = 0.0;
            double synapseWeight = alpha * pixelSynapseWeight + beta * localSynapseWeight + gamma * globalSynapseWeight;
            SetSynapseWeight(neuron, source, synapseWeight);
        }

        private static void TrainNeighbourhoodSynapse(int[] neuron, int[] source, Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            double pixelSynapseWeight = 0.0;
            double localSynapseWeight = 0.0;

            double sum = 0.0;
            foreach (ColorComponent colorComponent in Enum.GetValues(typeof(ColorComponent)))
            {
                double x = P_kb(neuron[2], colorComponent);
                sum += D_ijkl(radius, neuron[1], neuron[0], source[1], source[0]) * x * x;
            }
            double globalSynapseWeight = -2 * sum;

            double synapseWeight = alpha * pixelSynapseWeight + beta * localSynapseWeight + gamma * globalSynapseWeight;

            SetSynapseWeight(neuron, source, synapseWeight);
        }

        private static void SetNeuronBias(int[] neuron, double bias)
        {
            int index = NeuronPositionToIndex(neuron);
            _hopfieldNet.SetNeuronBias(index, bias);
        }

        private static void SetSynapseWeight(int[] neuron, int[] source, double weight)
        {
            int index = NeuronPositionToIndex(neuron);
            int sourceIndex = NeuronPositionToIndex(source);
            _hopfieldNet.SetSynapseWeight(index, sourceIndex, weight);
        }

        #endregion // Initialization

        #region Evaluation

        private static Bitmap EvaluateHopfield(HopfieldNetwork net, Bitmap originalImage)
        {
            var input = ImageToVector(originalImage);
            var output = net.Evaluate(input, iterations: 20);
            var ditheredImage = VectorToImage(output);

            return ditheredImage;
        }

        private static double[] ImageToVector(Bitmap image)
        {
            var vector = new double[NeuronCount];

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    Color color = image.GetPixel(x, y);
                    int[] winner = null; // TODO _kohonenNet.Evaluate(color); 
                    int colorIndex = winner[0];

                    for (int z = 0; z < _depth; z++)
                    {
                        int index = NeuronPositionToIndex(x, y, z);
                        vector[index] = z == colorIndex ? 1.0 : 0.0;
                    }
                }
            return vector;
        }

        private static Bitmap VectorToImage(double[] vector)
        {
            var image = new Bitmap(_width, _height);

            for (int y = 0; y < _height; y++)
                for (int x = 0; x < _width; x++)
                {
                    Color color = Color.White;
                    for (int z = 0; z < _depth; z++)
                    {
                        int neuronIndex = NeuronPositionToIndex(x, y, z);
                        if (vector[neuronIndex] >= 0.99)
                        {
                            color = _palette[z];
                            break;
                        }
                    }
                    image.SetPixel(x, y, color);
                }

            return image;
        }

        #endregion // Evaluation

        private static int NeuronPositionToIndex(int row, int col, int depth)
            => row * (_width * _depth) + col * _depth + depth;

        private static int NeuronPositionToIndex(int[] position)
            => NeuronPositionToIndex(row: position[0], col: position[1], depth: position[2]);

        // DEBUG
        private static Bitmap PaletteToImage(Color[] palette)
        {
            var paletteImage = new Bitmap(palette.Length * 10, 10);

            for (int c = 0; c < palette.Length; c++)
                for (int y = 0; y < 10; y++)
                    for (int x = c * 10; x < (c + 1) * 10; x++)
                        paletteImage.SetPixel(x, y, palette[c]);

            return paletteImage;
        }

        private static double C_ijb(Bitmap image, int i, int j, ColorComponent colorComponent)
        {
            var color = image.GetPixel(i, j);
            return GetColorComponent(color, colorComponent) / (double)Byte.MaxValue;
        }

        private static double P_kb(int k, ColorComponent colorComponent)
        {
            var color = _palette[k];
            return GetColorComponent(color, colorComponent) / (double)Byte.MaxValue;
        }

        private static int D_ijkl(int radius, int i, int j, int k, int l)
        {
            return Math.Abs(i - k) <= radius && Math.Abs(j - l) <= radius
                ? (radius - Math.Abs(i - k) + 1) * (radius - Math.Abs(j - l) + 1) : 0;
        }

        private static byte GetColorComponent(Color color, ColorComponent colorComponent)
        {
            switch (colorComponent)
            {
                case ColorComponent.Red: return color.R;
                case ColorComponent.Green: return color.G;
                case ColorComponent.Blue: return color.B;
                default: throw new ArgumentException("colorComponent");
            }
        }

        private static IEnumerable<ColorComponent> ColorComponents
            => Enum.GetValues(typeof(ColorComponent)).Cast<ColorComponent>();

        #region Utils

        private static int[] P(int y, int x, int z) => new[] { x, y, z };

        // Row
        private static int Y(int[] neuron) => neuron[0];

        // Column
        private static int X(int[] neuron) => neuron[1];

        // Depth
        private static int Z(int[] neuron) => neuron[2];

        #endregion // Utils
    }

    enum ColorComponent
    {
        Red,
        Green,
        Blue
    }
}
