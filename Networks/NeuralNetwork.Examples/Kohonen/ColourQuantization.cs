using System;
using System.Drawing;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.Kohonen;
using ShellProgressBar;

namespace NeuralNetwork.Examples.Kohonen
{
    class ColourQuantization
    {
        const string ImageDir = @"..\..\..\images\";
        const int TrainingIterations = 100;

        public static void Run()
        {
            string imageName = "lenna-col";

            int[] paletteSizes = { 4, 8, 16, 32 };

            foreach (int paletteSize in paletteSizes)
                TestQuantizeColours(imageName, paletteSize);
        }

        private static void TestQuantizeColours(string imageName, int paletteSize)
        {
            Console.Write($"ProcessImage({paletteSize})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var (processedImage, paletteImage) = QuantizeColours(originalImage, paletteSize);

            processedImage.Save($"{imageName}_{paletteSize}.png");
            paletteImage.Save($"{imageName}_palette_{paletteSize}.png");

            Console.WriteLine("Done");
        }

        private static (Bitmap processedImage, Bitmap paletteImage) QuantizeColours(Bitmap originalImage, int paletteSize)
        {
            var (net, palette) = QuantizeColours2(originalImage, paletteSize);

            var processedImage = ProcessImage(net, originalImage);
            var paletteImage = PaletteToImage(palette);

            return (processedImage, paletteImage);
        }

        public static (KohonenNetwork net, Color[] palette) QuantizeColours2(Bitmap image, int paletteSize)
        {
            // Step 1: Build the training set.

            var data = BuildTrainingSet(image);

            // Step 2: Build the network.

            var net = new KohonenNetwork(3, new[] { paletteSize });

            // Step 3: Train the network.

            var pbar = new ProgressBar(TrainingIterations, "Training...");
            net.TrainingIteration += (sender, args) => pbar.Tick($"Iteration {args.Iteration}/{TrainingIterations}");
            net.Train(data, TrainingIterations);

            // Step 4 : Use the network.

            var palette = ExtractPalette(net);

            return (net, palette);
        }

        private static Bitmap ProcessImage(KohonenNetwork net, Bitmap originalImage)
        {
            var processedImage = new Bitmap(originalImage.Width, originalImage.Height);

            for (int y = 0; y < originalImage.Height; y++)
            for (int x = 0; x < originalImage.Width; x++)
            {
                var originalColor = originalImage.GetPixel(x, y);
                var processedColor = EvaluateColorToColor(net, originalColor);
                processedImage.SetPixel(x, y, processedColor);
            }

            return processedImage;
        }

        private static Color[] ExtractPalette(KohonenNetwork net)
            => Enumerable.Range(0, net.OutputSize).Select(n => GetOutputNeuronColor(net, n)).ToArray();

        private static DataSet BuildTrainingSet(Bitmap image)
        {
            var trainingSet = new DataSet(3);

            for (int y = 0; y < image.Height; y++)
            for (int x = 0; x < image.Width; x++)
                trainingSet.Add(new LabeledDataPoint(ColorToVector(image.GetPixel(x, y)), new double[0]));

            return trainingSet;
        }

        private static Color EvaluateColorToColor(KohonenNetwork net, Color color)
        {
            var winner = EvaluateColorToNeuron(net, color);
            return GetOutputNeuronColor(net, winner);
        }

        public static int[] EvaluateColorToNeuron(KohonenNetwork net, Color color)
        {
            var input = ColorToVector(color);
            return net.Evaluate(input);
        }

        private static Color GetOutputNeuronColor(KohonenNetwork net, int[] neuron)
        {
            var weights = net.GetOutputNeuronSynapseWeights(neuron);
            return VectorToColor(weights);
        }

        private static Color GetOutputNeuronColor(KohonenNetwork net, int neuron)
        {
            var weights = net.GetOutputNeuronSynapseWeights(neuron);
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
