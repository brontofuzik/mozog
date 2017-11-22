using System;
using System.Drawing;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.Kohonen;

namespace NeuralNetwork.Examples.Kohonen
{
    class PaletteExtraction
    {
        const string ImageDir = @"..\..\..\images\";
        const int TrainingIterations = 100;

        public static void Run()
        {
            string imageName = "lenna-col";

            int[] paletteSizes = { 4, 8, 12, 16 };

            foreach (int paletteSize in paletteSizes)
                TestProcessImage(imageName, paletteSize);
        }

        private static void TestProcessImage(string imageName, int paletteSize)
        {
            Console.Write($"ProcessImage({paletteSize})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var palettedImage = ProcessImage(originalImage, paletteSize);

            palettedImage.Save($"{imageName}_{paletteSize}.png");

            Console.WriteLine("Done");
        }

        private static void TestExtractPalette(string imageName, int paletteSize)
        {
            Console.Write($"ExtractPalette({paletteSize})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");

            var palette = ExtractPalette(originalImage, paletteSize);

            Console.WriteLine("Done");
        }

        public static Bitmap ProcessImage(Bitmap originalImage, int paletteSize)
        {
            // Step 1: Build the training set.

            var data = BuildTrainingSet(originalImage);

            // Step 2 : Build the network.

            var net = new KohonenNetwork(3, new[] { paletteSize });

            // Step 3: Train the network.

            net.Train(data, TrainingIterations);

            // Step 4 : Use the network.

            var processedImage = ProcessImage(net, originalImage);

            return processedImage;
        }

        // Using a trained net
        public static Bitmap ProcessImage(KohonenNetwork net, Bitmap originalImage)
        {
            var processedImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            for (int x = 0; x < originalImage.Width; x++)
            {
                var originalColor = originalImage.GetPixel(x, y);
                var processedColor = Evaluate(net, originalColor);
                processedImage.SetPixel(x, y, processedColor);
            }

            return processedImage;
        }

        public static Color[] ExtractPalette(Bitmap image, int paletteSize)
        {
            // Step 1: Build the training set.

            var data = BuildTrainingSet(image);

            // Step 2: Build the network.

            var net = new KohonenNetwork(3, new[] { paletteSize });

            // Step 3: Train the network.

            net.Train(data, TrainingIterations);

            // Step 4 : Use the network.

            Color[] palette = GetPalette(net);

            return palette;
        }

        // From a trained net
        public static Color[] GetPalette(KohonenNetwork net)
            => Enumerable.Range(0, net.OutputSize).Select(n => GetOutputNeuronColor(net, n)).ToArray();

        private static DataSet BuildTrainingSet(Bitmap image)
        {
            var trainingSet = new DataSet(3, 0);

            for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
            {
                var color = image.GetPixel(x, y);
                var point = new LabeledDataPoint(ColorToVector(color), new double[0]);
                trainingSet.Add(point);
            }

            return trainingSet;
        }

        private static Color Evaluate(KohonenNetwork net, Color color)
        {
            var input = ColorToVector(color);
            var winner = net.Evaluate(input);
            var output = net.GetOutputNeuronSynapseWeights(winner);
            return VectorToColor(output);
        }

        private static Color GetOutputNeuronColor(KohonenNetwork net, int neuronIndex)
        {
            var weights = net.GetOutputNeuronSynapseWeights(neuronIndex);
            return VectorToColor(weights);
        }

        private static Color GetOutputNeuronColor(KohonenNetwork net, int[] neuronCoordinates)
        {
            var weights = net.GetOutputNeuronSynapseWeights(neuronCoordinates);
            return VectorToColor(weights);
        }

        private static double[] ColorToVector(Color color)
        {
            double red = color.R / (double)byte.MaxValue;
            double green = color.G / (double)byte.MaxValue;
            double blue = color.B / (double)byte.MaxValue;

            return new[] { red, green, blue };
        }

        private static Color VectorToColor(double[] vector)
        {
            byte red = (byte)Math.Round(vector[0] * byte.MaxValue);
            byte green = (byte)Math.Round(vector[1] * byte.MaxValue);
            byte blue = (byte)Math.Round(vector[2] * byte.MaxValue);

            return Color.FromArgb(red, green, blue);
        }
    }
}
