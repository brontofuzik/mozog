using System;
using System.Drawing;
using NeuralNetwork.Examples.Hopfield.INS04;

namespace NeuralNetwork.Examples.Hopfield
{
    class ColourDithering
    {
        const string ImageDir = @"..\..\..\images\";

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

        static void DitherImage(string imageName, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            Console.Write($"DitherImage(paletteSize: {paletteSize}, radius: {radius}, alpha: {alpha:F3}, beta: {beta:F3}, gamma: {gamma:F3})...");

            var originalImage = new Bitmap($@"{ImageDir}\{imageName}.png");
            var ditheredImage = ColourDitheringNetwork.DitherImage(originalImage, paletteSize, radius, alpha, beta, gamma);

            ditheredImage.Save($"{imageName}_{paletteSize}_{radius}_{alpha:F3}_{beta:F3}_{gamma:F3}.png");

            Console.WriteLine("Done");
        }
    }
}
