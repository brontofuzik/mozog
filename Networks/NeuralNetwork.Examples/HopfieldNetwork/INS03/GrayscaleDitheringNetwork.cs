using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp;

namespace NeuralNetwork.Examples.HopfieldNetwork.INS03
{
    class GrayscaleDitheringNetwork
    {
        /// <summary>
        /// Dithers a (grayscale) image.
        /// </summary>
        /// <param name="originalImage">The original (grayscale) image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns>The dithered image.</returns>
        public static Bitmap DitherImage(Bitmap originalImage, int radius, double alpha)
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            Trace.Write("Step 1: Creating the training set... ");

            // Do nothing.

            Trace.WriteLine("Done");

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            Trace.Write("Step 2: Creating the network... ");

            // Create the dithering network.
            int imageWidth = originalImage.Width;
            int imageHeight = originalImage.Height;
            GrayscaleDitheringNetwork grayscaleDitheringNetwork = new GrayscaleDitheringNetwork(imageWidth, imageHeight);

            Trace.WriteLine("Done");

            // --------------------------
            // Step 3: train the network.
            // --------------------------

            Trace.Write("Step 3: Training the network... ");

            grayscaleDitheringNetwork.train(originalImage, radius, alpha);

            Trace.WriteLine("Done");

            // ------------------------
            // Step 4: Use the network.
            // ------------------------

            Trace.Write("Step 4: Using the network... ");

            Bitmap ditheredImage = grayscaleDitheringNetwork.evalaute(originalImage, _evaluationIterationCount);

            Trace.WriteLine("Done");

            return ditheredImage;
        }

        /// <summary>
        /// Initializes a new instance of the GrayscaleDitheringNetwork class.
        /// </summary>
        /// <param name="width">The width of the dithering network.</param>
        /// <param name="height">The height of the dithering network.</param>
        private GrayscaleDitheringNetwork(int width, int height)
        {
            if (width <= 0)
            {
                throw new ArgumentException("The width must be positive.", nameof(width));
            }

            if (height <= 0)
            {
                throw new ArgumentException("The height must be positive.", nameof(height));
            }

            _underlyingHopfieldNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(width * height, grayscaleDitheringNetworkActivationFunction, new SparseHopfieldNetworkImpFactory());
            _width = width;
            _height = height;
        }

        /// <summary>
        /// The activation function of the grayscale dithering network.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double grayscaleDitheringNetworkActivationFunction(double input, double evaluationProgressRatio)
        {
            // The initial and final lambda values.
            double initialLambda = 1.0;
            double finalLambda = 100;

            double lambda = initialLambda + (finalLambda - initialLambda) * evaluationProgressRatio; 
            return 1 / (1 + Math.Exp(-lambda * input));
        }

        /// <summary>
        /// Trains the grayscale dithering network.
        /// </summary>
        /// <param name="trainingImage">The (grayscale) training image.</param>
        /// <param name="radius">The radius (i.e. the "extent of globality").</param>
        /// <param name="alpha">The alpha (i.e. the "degree of locality").</param>
        public void train(Bitmap trainingImage, int radius, double alpha)
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

            // Train the neurons.
            for (int neuronYCoordinate = 0; neuronYCoordinate < _height; ++neuronYCoordinate)
            {
                for (int neuronXCoordinate = 0; neuronXCoordinate < _width; ++neuronXCoordinate)
                {
                    // DEBUG
                    //Console.WriteLine("({0}, {1})", neuronXCoordinate, neuronYCoordinate);
                    trainNeuron(neuronXCoordinate, neuronYCoordinate, trainingImage, radius, alpha);
                }
            }
        }

        /// <summary>
        /// Trains a neuron.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="trainingImage">The training image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The alpha.</param>
        private void trainNeuron(int neuronXCoordinate, int neuronYCoordinate, Bitmap trainingImage, int radius, double alpha)
        {
            // Get the coordinates of the source neurons.
            ICollection<int[]> sourceNeuronsCoordinates = getSourceNeuronsCoordinates(neuronXCoordinate, neuronYCoordinate, radius);          

            // Calculate the local bias of the neuron.
            double localNeuronBias = 2 * c_ij(trainingImage, neuronXCoordinate, neuronYCoordinate) - 1;

            // Calculate the global bias of the neuron.
            double sum = 0.0;
            foreach (int[] sourceNeuronCoordinates in sourceNeuronsCoordinates)
            {
                int sourceNeuronXCoordinate = sourceNeuronCoordinates[0];
                int sourceNeuronYCoordinate = sourceNeuronCoordinates[1];
                sum += c_ij(trainingImage, sourceNeuronXCoordinate, sourceNeuronYCoordinate) * d_ijkl(radius, neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);

            }
            double globalNeuronBias = 2 * sum - radius * radius;

            // Calculate the (total) bias of the neuron.
            double neuronBias = alpha * localNeuronBias + (1 - alpha) * globalNeuronBias;

            //  Set the bias of the neuron.
            setNeuronBias(neuronXCoordinate, neuronYCoordinate, neuronBias);

            // Train the synapses.
            foreach (int[] sourceNeuronCoordinates in sourceNeuronsCoordinates)
            {
                int sourceNeuronXCoordinate = sourceNeuronCoordinates[0];
                int sourceNeuronYCoordinate = sourceNeuronCoordinates[1];
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate, trainingImage, radius, alpha);
            }
        }

        /// <summary>
        /// Gets the coordinates of the neuron's source neurons.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>The coordinates of the neuron's source neurons.</returns>
        private ICollection<int[]> getSourceNeuronsCoordinates(int neuronXCoordinate, int neuronYCoordinate, int radius)
        {
            // Get the coordinates of the source neurons.
            ICollection<int[]> sourceNeuronsCoordinates = new HashSet<int[]>();

            // Calculate the limits.
            int sourceNeuronXCoordinateMin = Math.Max(neuronXCoordinate - radius, 0);
            int sourceNeuronXCoordinateMax = Math.Min(neuronXCoordinate + radius, _width - 1);
            int sourceNeuronYCoordinateMin = Math.Max(neuronYCoordinate - radius, 0);
            int sourceNeuronYCoordinateMax = Math.Min(neuronYCoordinate + radius, _height - 1);

            for (int sourceNeuronYCoordinate = sourceNeuronYCoordinateMin; sourceNeuronYCoordinate <= sourceNeuronYCoordinateMax; ++sourceNeuronYCoordinate)
            {
                for (int sourceNeuronXCoordinate = sourceNeuronXCoordinateMin; sourceNeuronXCoordinate <= sourceNeuronXCoordinateMax; ++sourceNeuronXCoordinate)
                {
                    if (sourceNeuronXCoordinate == neuronXCoordinate && sourceNeuronYCoordinate == neuronYCoordinate)
                    {
                        continue;
                    }
                    sourceNeuronsCoordinates.Add(new int[] {sourceNeuronXCoordinate, sourceNeuronYCoordinate});
                }
            }

            return sourceNeuronsCoordinates;
        }

        /// <summary>
        /// Trains a synapse.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="sourceNeuronXCoordinate">The x coordiante of the source neuron.</param>
        /// <param name="sourceNeuronYCoordinate">The y coordinate of the source neuron.</param>
        /// <param name="trainingImage">The training image.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="alpha">The alpha.</param>
        private void trainSynapse(int neuronXCoordinate, int neuronYCoordinate, int sourceNeuronXCoordinate, int sourceNeuronYCoordinate, Bitmap trainingImage, int radius, double alpha)
        {
            // Calculate the local weight of the synapse.
            double synapseLocalWeight = 0.0;

            // Calculate the global weight of the synapse.
            double synapseGlobalWeight = -2 * d_ijkl(radius, neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);

            // Calculate the (total) weight of the synapse.
            double synapseWeight = alpha * synapseLocalWeight + (1 - alpha) * synapseGlobalWeight;

            // Set the weight of the synapse.
            setSynapseWeight(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate, synapseWeight);
        }

        /// <summary>
        /// Sets the bias of a neuron.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        private void setNeuronBias(int neuronXCoordinate, int neuronYCoordinate, double neuronBias)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronXCoordinate, neuronYCoordinate);

            // Set the bias of the neuron in the underlying network.
            _underlyingHopfieldNetwork.SetNeuronBias(neuronIndex, neuronBias);
        }

        /// <summary>
        /// Sets the weight of a synapse.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="sourceNeuronXCoordinate">The x coordinate of the source neuron.</param>
        /// <param name="sourceNeuronYCoordinate">The y coordinate of the source neuron.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        private void setSynapseWeight(int neuronXCoordinate, int neuronYCoordinate, int sourceNeuronXCoordinate, int sourceNeuronYCoordinate, double synapseWeight)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronXCoordinate, neuronYCoordinate);

            // Calculate the index of the source neuron.
            int sourceNeuronIndex = neuronCoordinatesToIndex(sourceNeuronXCoordinate, sourceNeuronYCoordinate);

            // Set the weight of the synapse in the underlying network.
            _underlyingHopfieldNetwork.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        /// <summary>
        /// Converts the index of a neuron to its coordinates.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        private void neuronIndexToCoordinates(int neuronIndex, out int neuronXCoordinate, out int neuronYCoordinate)
        {
            // Calculate the x coordinate of the neuron.
            neuronXCoordinate = neuronIndex % _width;

            // Calculate the y coordiante of the neuron.
            neuronIndex /= _width;
            neuronYCoordinate = neuronIndex % _height;
        }

        /// <summary>
        /// Converts the coordinates of the neuron to its index.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <returns>The index of the neuron.</returns>
        private int neuronCoordinatesToIndex(int neuronXCoordinate, int neuronYCoordinate)
        {
            return neuronYCoordinate * _width + neuronXCoordinate * 1;
        }

        /// <summary>
        /// c_ijb
        /// Gets the brightness of the pixel with coordinates "i" and "j".
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="i">The i coordinate.</param>
        /// <param name="j">The j coordinate.</param>
        /// <returns></returns>
        private double c_ij(Bitmap trainingImage, int i, int j)
        {
            return trainingImage.GetPixel(i, j).GetBrightness();
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
        /// Evaluates the grayscale dithering network.
        /// </summary>
        /// <param name="originalImage">The original (greyscale) image.</param>
        /// <param name="evaluationIterationCount">The number of evaluation iterations.</param>
        /// <returns>The dithered image.</returns>
        public Bitmap evalaute(Bitmap originalImage, int iterationCount)
        {
            // Convert the original image into the pattern to recall.
            double[] patternToRecall = imageToPattern(originalImage);

            // Evaluate the underlying Hopfield netowork on the pattern to recall to obtain the recalled pattern.
            double[] recalledPattern = _underlyingHopfieldNetwork.Evaluate(patternToRecall, iterationCount);

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
                    int neuronIndex = neuronCoordinatesToIndex(x, y);
                    pattern[neuronIndex] = image.GetPixel(x, y).GetBrightness();
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
                    int neuronIndex = neuronCoordinatesToIndex(x, y);
                    Color color = pattern[neuronIndex] >= 0.5 ? Color.White : Color.Black;
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
    }
}
