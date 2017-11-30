using System;
using System.Drawing;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.Kohonen;
using ShellProgressBar;

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
            //int[] paletteSizes = { 16 };

            foreach (int paletteSize in paletteSizes)
                TestProcessImage(imageName, paletteSize);
        }

        // Tests ProcessImage(originalImage, paletteSize): processedImage
        private static void TestProcessImage(string imageName, int paletteSize)
        {
            Console.Write($"ProcessImage({paletteSize})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var processedImage = ProcessImage(originalImage, paletteSize);

            processedImage.Save($"{imageName}_{paletteSize}.png");

            Console.WriteLine("Done");
        }

        // Test ExtractPalette(image, paletteSize): colours
        private static void TestExtractPalette(string imageName, int paletteSize)
        {
            Console.Write($"ExtractPalette({paletteSize})...");

            var image = new Bitmap($@"{ImageDir}\{imageName}.png");

            var (palette, _) = ExtractPalette(image, paletteSize);
            var paletteImage = PaletteToImage(palette);

            paletteImage.Save($"{imageName}_palette_{paletteSize}.png");

            Console.WriteLine("Done");
        }

        public static Bitmap ProcessImage(Bitmap originalImage, int paletteSize)
        {
            // Step 1: Build the training set.

            var data = BuildTrainingSet(originalImage);

            // Step 2 : Build the network.

            var net = new KohonenNetwork(3, new[] { paletteSize });

            // Step 3: Train the network.

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.BeforeTrainingSet += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4 : Use the network.

            var processedImage = ProcessImage(net, originalImage);

            return processedImage;
        }

        // Uses a trained net
        private static Bitmap ProcessImage(KohonenNetwork net, Bitmap originalImage)
        {
            var processedImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            for (int x = 0; x < originalImage.Width; x++)
            {
                var originalColor = originalImage.GetPixel(x, y);
                var processedColor = EvaluateColor(net, originalColor);
                processedImage.SetPixel(x, y, processedColor);
            }

            return processedImage;
        }

        public static (Color[] palette, KohonenNetwork net) ExtractPalette(Bitmap image, int paletteSize)
        {
            // Step 1: Build the training set.

            var data = BuildTrainingSet(image);

            // Step 2: Build the network.

            var net = new KohonenNetwork(3, new[] { paletteSize });

            // Step 3: Train the network.

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.BeforeTrainingSet += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4 : Use the network.

            var palette = ExtractPalette(net);

            return (palette, net);
        }

        // Uses a trained net
        private static Color[] ExtractPalette(KohonenNetwork net)
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

        public static int EvaluateIndex(KohonenNetwork net, Color color)
        {
            var input = ColorToVector(color);
            var winner = net.Evaluate(input);
            return winner[0];
        }

        private static Color EvaluateColor(KohonenNetwork net, Color color)
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
            double red = color.R / (double)Byte.MaxValue;
            double green = color.G / (double)Byte.MaxValue;
            double blue = color.B / (double)Byte.MaxValue;

            return new[] { red, green, blue };
        }

        private static Color VectorToColor(double[] vector)
        {
            byte red = (byte)Math.Round(vector[0] * Byte.MaxValue);
            byte green = (byte)Math.Round(vector[1] * Byte.MaxValue);
            byte blue = (byte)Math.Round(vector[2] * Byte.MaxValue);

            return Color.FromArgb(red, green, blue);
        }

        private static Bitmap PaletteToImage(Color[] palette)
        {
            var paletteImage = new Bitmap(palette.Length * 100, 100);

            for (int c = 0; c < palette.Length; c++)
            for (int y = 0; y < 100; y++)
            for (int x = c * 100; x < (c + 1) * 100; x++)
                paletteImage.SetPixel(x, y, palette[c]);

            return paletteImage;
        }
    }
}
