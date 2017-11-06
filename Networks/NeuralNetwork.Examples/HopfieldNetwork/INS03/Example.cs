using System;
using System.Diagnostics;
using System.Drawing;

namespace NeuralNetwork.Examples.HopfieldNet.INS03
{
    class Example
    {
        public static void Run()
        {
            string imageFilename = "TODO";

            int[] radii = new int[] { 0, 1, 2, 3, 4 };
            double[] alphas = new double[] { 1.0, 0.995, 0.99, 0.985, 0.98 };

            foreach (int radius in radii)
                foreach (double alpha in alphas)
                    DitherImage(imageFilename, radius, alpha);
            }

        private static void DitherImage(string imageName, int radius, double alpha)
        {
            Console.Write($"DitherImage({radius}, {alpha})...");

            var originalImage = new Bitmap($"{0}.pgn");
            Bitmap ditheredImage = GrayscaleDitheringNetwork.DitherImage(originalImage, radius, alpha);
            ditheredImage.Save($"{imageName}_{radius}_{alpha:0.000}.pgn");

            Console.WriteLine("Done");
        }
    }
}
