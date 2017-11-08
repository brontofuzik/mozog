using System;
using System.Diagnostics;
using System.Drawing;

namespace NeuralNetwork.Examples.HopfieldNet.INS03
{
    class Example
    {
        const string imageDir = @"..\..\..\images\";

        public static void Run()
        {
            string imageName = "lenna";
            var radii = new int[] { 0, 1, 2, 3, 4 };
            //var radii = new int[] { 1 };
            var alphas = new double[] { 1.0, 0.995, 0.99, 0.985, 0.98 };
            //var alphas = new double[] { 0.995 };

            foreach (int radius in radii)
                foreach (double alpha in alphas)
                    DitherImage(imageName, radius, alpha);
            }

        private static void DitherImage(string imageName, int radius, double alpha)
        {
            Console.Write($"DitherImage({radius}, {alpha})...");

            var originalImage = new Bitmap($@"{imageDir}\{imageName}.png");
            var ditheredImage = GrayscaleDitheringNetwork.DitherImage(originalImage, radius, alpha);
            ditheredImage.Save($"{imageName}_{radius}_{alpha:0.000}.png");

            Console.WriteLine("Done");
        }
    }
}
