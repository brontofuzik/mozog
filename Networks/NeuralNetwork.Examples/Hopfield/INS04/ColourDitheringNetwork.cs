using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using NeuralNetwork.Examples.Kohonen;
using NeuralNetwork.Hopfield;
using NeuralNetwork.Kohonen;

namespace NeuralNetwork.Examples.Hopfield.INS04
{
    class ColourDitheringNetwork
    {
        private static int _evaluationIterationCount = 20;

        private static KohonenNetwork _kohonenNet;

        private static HopfieldNetwork<Position1D> _hopfieldNet;

        private static Color[] _palette;

        // Dimensions
        private static int _width;
        private static int _height;
        private static int _depth;

        // alpha - pixel energy coefficient
        // beta - local energy coefficient
        // gamma - global energy coefficient
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

            _hopfieldNet = HopfieldNetwork.Build1DNetwork(originalImage.Width * originalImage.Height * paletteSize, sparse: true, activation: Activation);
            _width = originalImage.Width;
            _height = originalImage.Height;
            _depth = paletteSize;

            // Step 3: Train the network.

            Train(originalImage, radius, alpha, beta, gamma);

            // Step 4: Use the network.

            var ditheredImage = colourDitheringNetwork.evaluate(originalImage);

            return ditheredImage;
        }

        private static double Activation(double input, double progress)
        {
            const double initialLambda = 1.0;
            const double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * progress;
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        private static void Train(Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            _palette = PaletteExtraction.ExtractPalette(image, _depth);

            // Train the neurons.
            for (int y = 0; y < _height; y++)
            for (int x = 0; x < _width; x++)
            for (int z = 0; z < _depth; z++)
            {
                var neuronCoordinates = new NeuronCoordinates(x, y, z);
                TrainNeuron(neuronCoordinates, image, radius, alpha, beta, gamma);
            }
        }

        private static void TrainNeuron(NeuronCoordinates neuronCoordinates, Bitmap image, int radius, double alpha, double beta, double gamma)
        {
            // Get the coordinates of the "chain" source neurons.
            ICollection<NeuronCoordinates> chainSourceNeuronsCoordinates = getChainSourceNeuronsCoordinates(neuronCoordinates);

            // Get the coordinates of the "neighbourhood" source neurons.
            ICollection<NeuronCoordinates> neighbourhoodSourceNeuronsCoordinates = getNeighbourhoodSourceNeuronsCoordinates(neuronCoordinates, radius);

            // Calculate the pixel bias of the neuron.
            double pixelNeuronBias = 1.0;

            // Calculate the local bias of the neuron.
            double sum = 0.0;
            foreach (ColorComponent colorComponent in Enum.GetValues(typeof(ColorComponent)))
            {
                double x = c_ijb(image, neuronCoordinates.X, neuronCoordinates.Y, colorComponent) - p_kb(neuronCoordinates.Z, colorComponent);
                sum += x * x;
            }
            double localNeuronBias = -sum;

            // Calculate the global bias of the neuron.
            double outerSum = 0.0;
            foreach (ColorComponent colorComponent in Enum.GetValues(typeof(ColorComponent)))
            {
                double innerSum = 0.0;
                foreach (NeuronCoordinates sourceNeuronCoordinates in neighbourhoodSourceNeuronsCoordinates)
                {
                    innerSum += d_ijkl(radius, neuronCoordinates.X, neuronCoordinates.Y, sourceNeuronCoordinates.X, sourceNeuronCoordinates.Y) * c_ijb(image, sourceNeuronCoordinates.X, sourceNeuronCoordinates.Y, colorComponent);
                }
                double x = p_kb(neuronCoordinates.Z, colorComponent);
                outerSum += 2 * x * innerSum - radius * radius * x * x;
            }
            double globalNeuronBias = outerSum;

            // Calculate the (total) bias of the neuron.
            double neuronBias = alpha * pixelNeuronBias + beta * localNeuronBias + gamma * globalNeuronBias;

            // Set the bias of the neuron.
            setNeuronBias(neuronCoordinates, neuronBias);

            // Train the "chain" synapses.
            foreach (NeuronCoordinates sourceNeuronCoordinates in chainSourceNeuronsCoordinates)
            {
                trainChainSynapse(neuronCoordinates, sourceNeuronCoordinates, image, radius, alpha, beta, gamma);
            }

            // Train the "neighbourhood" synapes.
            foreach (NeuronCoordinates sourceNeuronCoordinates in neighbourhoodSourceNeuronsCoordinates)
            {
                trainNeighbourhoodSynapse(neuronCoordinates, sourceNeuronCoordinates, image, radius, alpha, beta, gamma);
            }
        }

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

        /// <summary>
        /// Gets the coordinates of a neuron's "chain" source neurons.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param>
        /// <returns>The coordinates of the neuron's "chain" source neurons.</returns>
        private ICollection<NeuronCoordinates> getChainSourceNeuronsCoordinates(NeuronCoordinates neuronCoordinates)
        {
            ICollection<NeuronCoordinates> chainSourceNeuronsCoordinates = new HashSet<NeuronCoordinates>();

            int sourceNeuronXCoordinate = neuronCoordinates.X;
            int sourceNeuronYCoordinate = neuronCoordinates.Y;
            for (int sourceNeuronZCoordinate = 0; sourceNeuronZCoordinate < _depth; ++sourceNeuronZCoordinate)
            {
                if (sourceNeuronZCoordinate == neuronCoordinates.Z)
                {
                    continue;
                }
                NeuronCoordinates sourceNeuronCoordinates = new NeuronCoordinates(sourceNeuronXCoordinate, sourceNeuronYCoordinate, sourceNeuronZCoordinate);
                chainSourceNeuronsCoordinates.Add(sourceNeuronCoordinates);
            }

            return chainSourceNeuronsCoordinates;
        }

        /// <summary>
        /// Gets the coordinates of a neuron's "neighbourhood" source neurons.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>The coordinates of the neuron's "neighbourhood" source neurons.</returns>
        private ICollection<NeuronCoordinates> getNeighbourhoodSourceNeuronsCoordinates(NeuronCoordinates neuronCoordinates, int radius)
        {
            ICollection<NeuronCoordinates> neighbourhoodSourceNeuronsCoordinates = new HashSet<NeuronCoordinates>();

            // Calculate the limits.
            int sourceNeuronXCoordinateMin = Math.Max(neuronCoordinates.X - radius, 0);
            int sourceNeuronXCoordinateMax = Math.Min(neuronCoordinates.X + radius, _width - 1);
            int sourceNeuronYCoordinateMin = Math.Max(neuronCoordinates.Y - radius, 0);
            int sourceNeuronYCoordinateMax = Math.Min(neuronCoordinates.Y + radius, _height - 1);

            int sourceNeuronZCoordinate = neuronCoordinates.Z;
            for (int sourceNeuronYCoordinate = sourceNeuronYCoordinateMin; sourceNeuronYCoordinate <= sourceNeuronYCoordinateMax; ++sourceNeuronYCoordinate)
            {
                for (int sourceNeuronXCoordinate = sourceNeuronXCoordinateMin; sourceNeuronXCoordinate <= sourceNeuronXCoordinateMax; ++sourceNeuronXCoordinate)
                {
                    if (sourceNeuronXCoordinate == neuronCoordinates.X && sourceNeuronYCoordinate == neuronCoordinates.Y)
                    {
                        continue;
                    }
                    NeuronCoordinates sourceNeuronCoordinates = new NeuronCoordinates(sourceNeuronXCoordinate, sourceNeuronYCoordinate, sourceNeuronZCoordinate);
                    neighbourhoodSourceNeuronsCoordinates.Add(sourceNeuronCoordinates);
                }
            }

            return neighbourhoodSourceNeuronsCoordinates;
        }

        /// <summary>
        /// Trains a "chain" synapse.
        /// </summary>
        /// <param name="neuronCoordinates">The coordiantes of the neuron.</param>
        /// <param name="sourceNeuronCoordinates">The coordinates of the source neuron.</param>
        /// <param name="trainingImage">The training image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The pixel energy coefficient.</param>
        /// <param name="beta">The local energy coefficient.</param>
        /// <param name="gamma">The global energy coefficient.</param>
        private void trainChainSynapse(NeuronCoordinates neuronCoordinates, NeuronCoordinates sourceNeuronCoordinates, Bitmap trainingImage, int radius, double alpha, double beta, double gamma)
        {
            // Calculate the pixel weight of the syanpse.
            double pixelSynapseWeight = -2.0;

            // Calculate the local weight of the synapse.
            double localSynapseWeight = 0.0;

            // Calculate the global weight of the synapse.
            double globalSynapseWeight = 0.0;

            // Calculate the (total) weight of the synapse.
            double synapseWeight = alpha * pixelSynapseWeight + beta * localSynapseWeight + gamma * globalSynapseWeight;

            // Set the weight of the synapse.
            setSynapseWeight(neuronCoordinates,sourceNeuronCoordinates, synapseWeight);
        }

        /// <summary>
        /// Trains a "neighbourhood" synapse.
        /// </summary>
        /// <param name="neuronCoordinates">The coordiantes of the neuron.</param>
        /// <param name="sourceNeuronCoordinates">The coordinates of the source neuron.</param>
        /// <param name="trainingImage">The training image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The pixel energy coefficient.</param>
        /// <param name="beta">The local energy coefficient.</param>
        /// <param name="gamma">The global energy coefficient.</param>
        private void trainNeighbourhoodSynapse(NeuronCoordinates neuronCoordinates, NeuronCoordinates sourceNeuronCoordinates, Bitmap trainingImage, int radius, double alpha, double beta, double gamma)
        {
            // Calculate the pixel weight of the syanpse.
            double pixelSynapseWeight = 0.0;

            // Calculate the local weight of the synapse.
            double localSynapseWeight = 0.0;

            // Calculate the global weight of the synapse.
            double sum = 0.0;
            foreach (ColorComponent colorComponent in Enum.GetValues(typeof(ColorComponent)))
            {
                double x = p_kb(neuronCoordinates.Z, colorComponent);
                sum += d_ijkl(radius, neuronCoordinates.X, neuronCoordinates.Y, sourceNeuronCoordinates.X, sourceNeuronCoordinates.Y) * x * x;
            }
            double globalSynapseWeight = -2 * sum;

            // Calculate the (total) weight of the synapse.
            double synapseWeight = alpha * pixelSynapseWeight + beta * localSynapseWeight + gamma * globalSynapseWeight;

            // Set the weight of the synapse.
            setSynapseWeight(neuronCoordinates, sourceNeuronCoordinates, synapseWeight);
        }

        /// <summary>
        /// Sets the bias of a neuron.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        private void setNeuronBias(NeuronCoordinates neuronCoordinates, double neuronBias)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronCoordinates);

            // Set the bias of the neuron in the underlying network.
            _hopfieldNet.SetNeuronBias(neuronIndex, neuronBias);
        }

        /// <summary>
        /// Sets the weight of a synapse.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param>
        /// <param name="sourceNeuronCoordinates">The coordinates of the source neuron.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        private void setSynapseWeight(NeuronCoordinates neuronCoordinates, NeuronCoordinates sourceNeuronCoordinates, double synapseWeight)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronCoordinates);

            // Calculate the index of the source neuron.
            int sourceNeuronIndex = neuronCoordinatesToIndex(sourceNeuronCoordinates);

            // Set the weight of the synapse in the underlying network.
            _hopfieldNet.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        /// <summary>
        /// Converts the index of a neuron to its coordinates.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <returns>The coordinates of the neuron.</returns>
        private NeuronCoordinates neuronIndexToCoordinates(int neuronIndex)
        {
            // Calculate the neuronXCoordinate coordinate.
            int neuronXCoordinate = neuronIndex % _width;

            // Calculate the neuronYCoordinate coordiante.
            neuronIndex /= _width;
            int neuronYCoordinate = neuronIndex % _height;

            // Calculate the neuronXCoordinate coordinate.
            neuronIndex /= _height;
            int neuronZCoordinate = neuronIndex % _depth;

            return new NeuronCoordinates(neuronXCoordinate, neuronYCoordinate, neuronZCoordinate);
        }

        /// <summary>
        /// Converts the coordinates of the neuron to its index.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param> 
        /// <returns>The index of the neuron.</returns>
        private int neuronCoordinatesToIndex(NeuronCoordinates neuronCoordinates)
        {
            return neuronCoordinates.Z * _height * _width + neuronCoordinates.Y * _width + neuronCoordinates.X * 1;
        }

        /// <summary>
        /// c_ijb
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="i">The i coordinate.</param>
        /// <param name="j">The j coordinate.</param>
        /// <param name="colorComponent">The component of the color.</param>
        /// <returns></returns>
        private double c_ijb(Bitmap image, int i, int j, ColorComponent colorComponent)
        {
            Color color = image.GetPixel(i, j);
            return getColorComponent(color, colorComponent) / (double)Byte.MaxValue;
        }

        /// <summary>
        /// p_kb
        /// </summary>
        /// <param name="k"></param>
        /// <param name="colorComponent">The component of the color.</param>
        /// <returns></returns>
        private double p_kb(int k, ColorComponent colorComponent)
        {
            Color color = _palette[k];
            return getColorComponent(color, colorComponent) / (double)Byte.MaxValue;
        }

        /// <summary>
        /// Gets a component of the given color.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <param name="colorComponent">The component of the color (Red, Green, or Blue).</param>
        /// <returns></returns>
        private byte getColorComponent(Color color, ColorComponent colorComponent)
        {
            switch (colorComponent)
            {
                case ColorComponent.Red:
                    return color.R;

                case ColorComponent.Green:
                    return color.G;

                case ColorComponent.Blue:
                    return color.B;

                default:
                    throw new ArgumentException("colorComponent");
            }
        }

        /// <summary>
        /// D_ijkl
        /// </summary>
        /// <param name="i">The i coordinate.</param>
        /// <param name="j">The j coordinate.</param>
        /// <param name="k">The k coordinate.</param>
        /// <param name="l">The l coordinate.</param>
        /// <returns></returns>
        private int d_ijkl(int radius, int i, int j, int k, int l)
        {
            if (Math.Abs(i - k) <= radius && Math.Abs(j - l) <= radius)
            {
                return (radius - Math.Abs(i - k) + 1) * (radius - Math.Abs(j - l) + 1);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Evaluates the colour dithering network.
        /// </summary>
        /// <param name="image">The original (colour) image.</param>
        /// <param name="evaluationIterationCount">The number of evaluation iterations.</param>
        /// <returns>The dithered image.</returns>
        private Bitmap evaluate(Bitmap originalImage)
        {
            // Convert the original image into the pattern to recall.
            double[] patternToRecall = imageToPattern(originalImage);

            // Evaluate the underlying Hopfield netowork on the pattern to recall to obtain the recalled pattern.
            double[] recalledPattern = _hopfieldNet.Evaluate(patternToRecall, _evaluationIterationCount);

            // Convert the recalled pattern into the dithered image.
            Bitmap ditheredImage = patternToImage(recalledPattern);

            return ditheredImage;
        }

        /// <summary>
        /// Converts an image to a pattern.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns>The pattern.</returns>
        private double[] imageToPattern(Bitmap image)
        {
            double[] pattern = new double[neuronCount];

            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    int[] winnerOutputNeuronCoordinates = _kohonenNet.Evaluate(pixelColor);
                    int colorIndex = winnerOutputNeuronCoordinates[0];
                    for (int z = 0; z < _depth; ++z)
                    {
                        int neuronIndex = neuronCoordinatesToIndex(new NeuronCoordinates(x, y, z));
                        pattern[neuronIndex] = z == colorIndex ? 1.0 : 0.0; 
                    }
                }
            }

            return pattern;
        }

        /// <summary>
        /// Converts a pattern to an image.
        /// </summary>
        /// <param name="pattern">The pattern.</param>
        /// <returns>The image.</returns>
        private Bitmap patternToImage(double[] pattern)
        {
            Bitmap image = new Bitmap(_width, _height);

            for (int y = 0; y < _height; ++y)
            {
                for (int x = 0; x < _width; ++x)
                {
                    Color color = Color.White;
                    for (int z = 0; z < _depth; ++z)
                    {
                        int neuronIndex = neuronCoordinatesToIndex(new NeuronCoordinates(x, y, z));
                        if (pattern[neuronIndex] >= 0.99)
                        {
                            color = _palette[z];
                            break;
                        }
                    }
                    image.SetPixel(x, y, color);
                }
            }

            return image;
        }

        private int neuronCount
        {
            get
            {
                return _hopfieldNet.Neurons;
            }
        }
    }

    struct NeuronCoordinates
    {
        public NeuronCoordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }

        public override string ToString() => $"({X}, {Y}, {Z})";
    }

    enum ColorComponent
    {
        Red,
        Green,
        Blue
    }
}
