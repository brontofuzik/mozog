using System;
using System.Diagnostics;
using System.Drawing;

using NeuralNetwork.KohonenNetwork;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace INS04
{
    class PalettingNetwork
    {
        /// <summary>
        /// Initializes a new instance of the PalettingNetwork class.
        /// </summary>
        /// <param name="paletteSize">The size of the palette (i.e. the number of colours in the palette).</param>
        public PalettingNetwork(int paletteSize)
        {
            _underlyingKohonenNetwork = new KohonenNetwork(3, new int[] { paletteSize });
        }

        /// <summary>
        /// Palettes an image.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <param name="paletteSize">The size of the palette (i.e. the number of colours in the palette).</param>
        /// <returns>The paletted image.</returns>
        public static Bitmap PaletteImage(Bitmap originalImage, int paletteSize)
        {
            // -------------------------------
            // Step 1: Build the training set.
            // -------------------------------

            Trace.Write("Step 1: Building the training set... ");

            // Do nothing.

            Trace.WriteLine("Done");

            // ---------------------------
            // Step 2 : Build the network.
            // ---------------------------

            Trace.Write("Step 2: Building the network... ");

            PalettingNetwork palettingNetwork = new PalettingNetwork(paletteSize);

            Trace.WriteLine("Done");

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            Trace.Write("Step 3: Training the netwotk... ");

            palettingNetwork.Train(originalImage);

            Trace.WriteLine("Done");

            // -------------------------
            // Step 4 : Use the network.
            // -------------------------

            Trace.Write("Step 4: Using the network... ");

            Bitmap palettedImage = palettingNetwork.Use(originalImage);

            Trace.WriteLine("Done");

            return palettedImage;
        }

        /// <summary>
        /// Extracts a palette from an image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="paletteSize">he size of the palette (i.e. the number of colours in the palette).</param>
        /// <returns>The palette.</returns>
        public static Color[] ExtractPalette(Bitmap image, int paletteSize)
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

            Trace.Write("Step 2: Building the network... ");

            PalettingNetwork palettingNetwork = new PalettingNetwork(paletteSize);

            Trace.WriteLine("Done");

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            Trace.Write("Step 3: Training the netwotk... ");

            palettingNetwork.Train(image);

            Trace.WriteLine("Done");

            // -------------------------
            // Step 4 : Use the network.
            // -------------------------

            Trace.Write("Step 4: Using the network... ");

            Color[] palette = palettingNetwork.GetPalette();

            Trace.WriteLine("Done");

            return palette;
        }

        /// <summary>
        /// Trains the paletting network.
        /// </summary>
        /// <param name="image">The training image.</param>
        public void Train(Bitmap trainingImage)
        {
            if (trainingImage == null)
            {
                throw new ArgumentNullException(nameof(trainingImage));
            }

            // Build the training set.
            TrainingSet trainingSet = buildTrainingSet(trainingImage);

            // Train the underlying Kohonen network.
            _underlyingKohonenNetwork.Train(trainingSet, _trainingIterationCount);
        }

        /// <summary>
        /// Evaluates the paletting network.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The paletted color.</returns>
        public int[] Evaluate(Color color)
        {
            if (color == null)
            {
                throw new ArgumentNullException(nameof(color));
            }

            // Convert the color into the input vector.
            double[] inputVector = colorToVector(color);

            // Evaluate the underlying Kohonen network on the input vector to get the coordinates of the winner output neuron.
            int[] winnerOutputNeuronCoordinates = _underlyingKohonenNetwork.Evaluate(inputVector);

            return winnerOutputNeuronCoordinates;
        }

        /// <summary>
        /// Gets the color represented by a given output neuron.
        /// </summary>
        /// <param name="outputNeuronIndex">The index of the output neuron.</param>
        /// <returns>The color represented by the output neuron.</returns>
        public Color GetOutputNeuronColor(int outputNeuronIndex)
        {
            double[] outputNeuronSynapseWeights = _underlyingKohonenNetwork.GetOutputNeuronSynapseWeights(outputNeuronIndex);
            Color color = vectorToColor(outputNeuronSynapseWeights);

            return color;
        }

        /// <summary>
        /// Gets the color represented by a given output neuron.
        /// </summary>
        /// <param name="outputNeuronCoordinates">The coordinates of the output neuron.</param>
        /// <returns>The color represented by the output neuron.</returns>
        public Color GetOutputNeuronColor(int[] outputNeuronCoordinates)
        {
            double[] outputNeuronSynapseWeights = _underlyingKohonenNetwork.GetOutputNeuronSynapseWeights(outputNeuronCoordinates);
            Color color = vectorToColor(outputNeuronSynapseWeights);

            return color;
        }

        /// <summary>
        /// Evaluates the paletting network.
        /// </summary>
        /// <param name="image">The original image.</param>
        /// <returns>The paletted image.</returns>
        public Bitmap Use(Bitmap originalImage)
        {
            Bitmap palettedImage = new Bitmap(originalImage.Width, originalImage.Height);
            for (int y = 0; y < originalImage.Height; ++y)
            {
                for (int x = 0; x < originalImage.Width; ++x)
                {
                    Color originalColor = originalImage.GetPixel(x, y);
                    int[] winnerOutputNeuronCoordinates = Evaluate(originalColor);
                    Color palettedColor = GetOutputNeuronColor(winnerOutputNeuronCoordinates);
                    palettedImage.SetPixel(x, y, palettedColor);
                }
            }
            return palettedImage;
        }

        public Color[] GetPalette()
        {
            Color[] palette = new Color[OutputNeuronCount];
            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                palette[outputNeuronIndex] = GetOutputNeuronColor(outputNeuronIndex);
            }
            return palette;
        }

        public int InputNeuronCount
        {
            get
            {
                return _underlyingKohonenNetwork.InputNeuronCount;
            }
        }

        public int OutputNeuronCount
        {
            get
            {
                return _underlyingKohonenNetwork.OutputNeuronCount;
            }
        }

        /// <summary>
        /// Builds a training set from the given training image.
        /// </summary>
        /// <param name="trainingImage">The training image.</param>
        /// <returns>The training set.</returns>
        private static TrainingSet buildTrainingSet(Bitmap trainingImage)
        {
            // Build an empty training set.
            // TODO: Change the current supervised sed for an unsupervised set.
            int inputVectorLength = 3;
            int outputVectorLength = 0;
            TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength);

            int trainingImageWidth = trainingImage.Width;
            int trainingImageHeight = trainingImage.Height;
            for (int y = 0; y < trainingImageHeight; ++y)
            {
                for (int x = 0; x < trainingImageWidth; ++x)
                {
                    Color color = trainingImage.GetPixel(x, y);

                    // Build a new training pattern and add it to the training set.
                    double[] inputVector = colorToVector(color);
                    double[] outputVector = new double[0];
                    SupervisedTrainingPattern trainingPattern = new SupervisedTrainingPattern(inputVector, outputVector);
                    trainingSet.Add(trainingPattern);
                }
            }

            return trainingSet;
        }

        /// <summary>
        /// Converts a color into its vector representation.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The vector representing the color.</returns>
        private static double[] colorToVector(Color color)
        {
            double red = color.R / (double)Byte.MaxValue;
            double green = color.G / (double)Byte.MaxValue;
            double blue = color.B / (double)Byte.MaxValue;
            double[] vector = new double[] { red, green, blue };

            return vector;
        }

        /// <summary>
        /// Converts a vector into its color representation.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The color representing the vector.</returns>
        private static Color vectorToColor(double[] vector)
        {
            byte red = (byte)Math.Round(vector[0] * Byte.MaxValue);
            byte green = (byte)Math.Round(vector[1] * Byte.MaxValue);
            byte blue = (byte)Math.Round(vector[2] * Byte.MaxValue);
            Color color = Color.FromArgb(red, green, blue);

            return color;
        }

        /// <summary>
        /// The number of training iterations.
        /// </summary>
        private static int _trainingIterationCount = 100;

        /// <summary>
        /// The underlying Kohonen network.
        /// </summary>
        private KohonenNetwork _underlyingKohonenNetwork;
    }
}
