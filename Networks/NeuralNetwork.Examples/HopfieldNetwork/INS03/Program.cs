using System;
using System.Drawing;

namespace NeuralNetwork.Examples.HopfieldNetwork.INS03
{
    class Program
    {
        static void _Main(string[] args)
        {
            string[] imageFileNames = new string[] { "square" };
            int[] radii = new int[] { 0, 1, 2, 3, 4 };
            double[] alphas = new double[] { 1.0, 0.995, 0.99, 0.985, 0.98 };

            foreach (string imageFileName in imageFileNames)
            {
                foreach (int radius in radii)
                {
                    foreach (double alpha in alphas)
                    {
                        TestDitherImage(imageFileName, radius, alpha);
                        Console.WriteLine();
                    }
                }
            }
        }

        static void TestDitherImage(string imageName, int radius, double alpha)
        {
            Console.WriteLine("TestDitherImage\n* imageName = {0}\n* radius = {1}\n* alpha = {2}\n=== BEGIN ===", imageName, radius, alpha);

            // Load the original image.
            string originalImageFileName = String.Format("{0}.source.{1}", imageName, imageFileNameExtension);
            Bitmap originalImage = new Bitmap(originalImageFileName);

            // Dither the original image to get the dithered image.
            Bitmap ditheredImage = GrayscaleDitheringNetwork.DitherImage(originalImage, radius, alpha);

            // Save the dithered image.
            string ditheredImageFileName = String.Format("{0}.target[radius={1},alpha={2:0.000}].{3}", imageName, radius, alpha, imageFileNameExtension);
            ditheredImage.Save(ditheredImageFileName);

            Console.WriteLine("=== END ===");
        }

        /// <summary>
        /// The extension of the image file name.
        /// </summary>
        static string imageFileNameExtension = "png";
    }
}
