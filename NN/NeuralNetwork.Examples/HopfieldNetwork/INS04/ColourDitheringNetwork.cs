using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp;

namespace NeuralNetwork.Examples.HopfieldNetwork.INS04
{
    class ColourDitheringNetwork
    {
        static ColourDitheringNetwork()
        {
            _random = new Random();
        }

        /// <summary>
        /// Dithers a (colour) image.
        /// </summary>
        /// <param name="image">The original (colour) image.</param>
        /// <param name="paletteSize">The size of the palette (i.e. the number of colours in the palette).</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The alpha coefficient.</param>
        /// <param name="beta">The beta coefficient.</param>
        /// <param name="gamma">The gamma coefficient.</param>
        /// <returns>The dithered image.</returns>
        public static Bitmap DitherImage(Bitmap originalImage, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            // -------------------------------
            // Step 1: Build the training set.
            // -------------------------------

            Trace.Write("Step 1: Building the training set... ");

            // Do nothing.

            Trace.WriteLine("Done");

            // --------------------------
            // Step 2: Build the network.
            // --------------------------

            Trace.Write("Step 2: Build the network... ");

            int width = originalImage.Width;
            int height = originalImage.Height;
            int depth = paletteSize;
            ColourDitheringNetwork colourDitheringNetwork = new ColourDitheringNetwork(width, height, depth);

            Trace.WriteLine("Done");

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            Trace.Write("Step 3: Training the network... ");

            colourDitheringNetwork.train(originalImage, radius, alpha, beta, gamma);

            Trace.WriteLine("Done");

            // ------------------------
            // Step 4: Use the network.
            // ------------------------

            Trace.Write("Step 4: Using the network... ");

            Bitmap ditheredImage = colourDitheringNetwork.evaluate(originalImage);

            Trace.WriteLine("Done");

            return ditheredImage;
        }

        /// <summary>
        /// Initializes a new instance of the ColourDitheringNetwork class.
        /// </summary>
        /// <param name="width">The width of the network.</param>
        /// <param name="height">The height of the network.</param>
        /// <param name="depth">The depth of the network.</param>
        private ColourDitheringNetwork(int width, int height, int depth)
        {
            if (width <= 0)
            {
                throw new ArgumentException("The width must be positive.", nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentException("The height must be positive.", nameof(height));
            }

            if (depth <= 0)
            {
                throw new ArgumentException("The depth must be positive", nameof(depth));
            }

            _underlyingHopfieldNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(width * height * depth, colourDitheringNetworkActivationFunction, new SparseHopfieldNetworkImpFactory());
            _width = width;
            _height = height;
            _depth = depth;
        }

        /// <summary>
        /// The activation function of the colour dithering network.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double colourDitheringNetworkActivationFunction(double input, double evaluationProgressRatio)
        {
            // The initial and final lambda values.
            double initialLambda = 1.0;
            double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * evaluationProgressRatio;
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        // DEBUG
        private static Bitmap paletteToImage(Color[] palette)
        {
            Bitmap paletteImage = new Bitmap(palette.Length * 10, 10);

            for (int colorIndex = 0; colorIndex < palette.Length; ++colorIndex)
            {
                for (int y = 0; y < 10; ++y)
                {
                    for (int x = colorIndex * 10; x < (colorIndex + 1) * 10; ++x)
                    {
                        paletteImage.SetPixel(x, y, palette[colorIndex]);
                    }
                }
            }

            return paletteImage;
        }

        /// <summary>
        /// Trains the colour dithering network.
        /// </summary>
        /// <param name="trainingImage">The (colour) training image.</param>
        /// <pa
        /// <param name="radius">The radius (i.e. the "extent of globality").</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="beta">The beta.</param>
        /// <param name="gamma">The gamma.</param>
        private void train(Bitmap trainingImage, int radius, double alpha, double beta, double gamma)
        {
            if (trainingImage == null)
            {
                throw new ArgumentNullException(nameof(trainingImage));
            }

            if (radius < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(radius), "The radius must be non-negative.");
            }

            if (alpha < 0.0 || 1.0 < alpha)
            {
                throw new ArgumentOutOfRangeException(nameof(alpha), "The alpha must be within the range [0, 1] (inclusive).");
            }

            if (beta < 0.0 || 1.0 < beta)
            {
                throw new ArgumentOutOfRangeException(nameof(beta), "The betamust be within the range [0, 1] (inclusive).");
            }

            if (gamma < 0.0 || 1.0 < gamma)
            {
                throw new ArgumentOutOfRangeException(nameof(gamma), "The gamma must be within the range [0, 1] (inclusive).");
            }

            #region Paletting network & palette

            // Build the paletting network.
            _palettingNetwork = new PalettingNetwork(_depth);

            // Train the paletting network.
            _palettingNetwork.Train(trainingImage);

            // Get the palette.
            _palette = _palettingNetwork.GetPalette();

            #endregion // Paletting network & palette

            // Train the neurons.
            for (int neuronYCoordinate = 0; neuronYCoordinate < _height; ++neuronYCoordinate)
            {
                for (int neuronXCoordinate = 0; neuronXCoordinate < _width; ++neuronXCoordinate)
                {
                    for (int neuronZCoordinate = 0; neuronZCoordinate < _depth; ++neuronZCoordinate)
                    {
                        NeuronCoordinates neuronCoordinates = new NeuronCoordinates(neuronXCoordinate, neuronYCoordinate, neuronZCoordinate);
                        trainNeuron(neuronCoordinates, trainingImage, radius, alpha, beta, gamma);
                    } 
                }
            }
        }

        /// <summary>
        /// Trains a neuron.
        /// </summary>
        /// <param name="neuronCoordinates">The coordinates of the neuron.</param>
        /// <param name="trainingImage">The training image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The pixel energy coefficient.</param>
        /// <param name="beta">The local energy coefficient.</param>
        /// <param name="gamma">The global energy coefficient.</param>
        private void trainNeuron(NeuronCoordinates neuronCoordinates, Bitmap trainingImage, int radius, double alpha, double beta, double gamma)
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
                double x = c_ijb(trainingImage, neuronCoordinates.X, neuronCoordinates.Y, colorComponent) - p_kb(neuronCoordinates.Z, colorComponent);
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
                    innerSum += d_ijkl(radius, neuronCoordinates.X, neuronCoordinates.Y, sourceNeuronCoordinates.X, sourceNeuronCoordinates.Y) * c_ijb(trainingImage, sourceNeuronCoordinates.X, sourceNeuronCoordinates.Y, colorComponent);
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
                trainChainSynapse(neuronCoordinates, sourceNeuronCoordinates, trainingImage, radius, alpha, beta, gamma);
            }

            // Train the "neighbourhood" synapes.
            foreach (NeuronCoordinates sourceNeuronCoordinates in neighbourhoodSourceNeuronsCoordinates)
            {
                trainNeighbourhoodSynapse(neuronCoordinates, sourceNeuronCoordinates, trainingImage, radius, alpha, beta, gamma);
            }
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
            _underlyingHopfieldNetwork.SetNeuronBias(neuronIndex, neuronBias);
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
            _underlyingHopfieldNetwork.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
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
                    break;
                case ColorComponent.Green:
                    return color.G;
                    break;
                case ColorComponent.Blue:
                    return color.B;
                    break;
                default:
                    throw new ArgumentException("colorComponent");
                    break;
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
            double[] recalledPattern = _underlyingHopfieldNetwork.Evaluate(patternToRecall, _evaluationIterationCount);

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
                    int[] winnerOutputNeuronCoordinates = _palettingNetwork.Evaluate(pixelColor);
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
                return _underlyingHopfieldNetwork.NeuronCount;
            }
        }

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        private static Random _random;
        
        /// <summary>
        /// The number of evaluation iterations.
        /// </summary>
        private static int _evaluationIterationCount = 20;

        /// <summary>
        /// The underlying Hopfield network.
        /// </summary>
        private NeuralNetwork.HopfieldNetwork.HopfieldNetwork _underlyingHopfieldNetwork;

        /// <summary>
        /// The width of the dithering network.
        /// </summary>
        private int _width;

        /// <summary>
        /// The height of the dithering network.
        /// </summary>
        private int _height;

        /// <summary>
        /// The depth of the network.
        /// </summary>
        private int _depth;

        /// <summary>
        /// The paletting network.
        /// </summary>
        private PalettingNetwork _palettingNetwork;

        /// <summary>
        /// The palette.
        /// </summary>
        private Color[] _palette;
    }

    struct NeuronCoordinates
    {
        public NeuronCoordinates(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
        }

        public int X
        {
            get
            {
                return _x;
            }
        }

        public int Y
        {
            get
            {
                return _y;
            }
        }

        public int Z
        {
            get
            {
                return _z;
            }
        }

        public override string ToString()
        {
            return String.Format("({0}, {1}, {2})", _x, _y, _z);
        }

        private int _x;

        private int _y;

        private int _z;
    }

    enum ColorComponent
    {
        Red,
        Green,
        Blue
    }
}
