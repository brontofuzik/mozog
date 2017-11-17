using System;
using System.Drawing;

namespace NeuralNetwork.Examples.HopfieldNet.INS04
{
    /*
    class Example
    {
        public static void Run()
        {
            string imageName = "monika";

            int[] paletteSizes = new int[] { 4, 8, 12, 16 };
            int[] radii = { 1, 2 };
            double alpha = 0.3;
            double beta = 0.695;
            double gamma = 0.005;

            foreach (int paletteSize in paletteSizes)
            {
                TestPaletteImage(imageName, paletteSize);
                Console.WriteLine();

                foreach (int radius in radii)
                {
                    TestDitherImage(imageName, paletteSize, radius, alpha, beta, gamma);
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Tests the PalettingNetwork.PaletteImage method.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="paletteSize"></param>
        static void TestPaletteImage(string imageName, int paletteSize)
        {
            Console.WriteLine("TestPaletteImage\n* imageName = {0}\n* paletteSize = {1}\n=== BEGIN ===", imageName, paletteSize);

            // Load the original image.
            string originalImageFileName = String.Format("{0}.source.{1}", imageName, imageFileNameExtension);
            Bitmap originalImage = new Bitmap(originalImageFileName);

            // Palette the original image to get the paletted image.
            Bitmap palettedImage = PalettingNetwork.PaletteImage(originalImage, paletteSize);

            // Save the paletted image.
            string palettedImageFileName = String.Format("{0}.target[paletteSize={1}].{2}", imageName, paletteSize, imageFileNameExtension);
            palettedImage.Save(palettedImageFileName);

            Console.WriteLine("=== END ===");
        }

        /// <summary>
        /// Tests the PalettingNetwork.ExtractPalette method.
        /// </summary>
        /// <param name="imageName"></param>
        /// <param name="paletteSize"></param>
        static void TestExtractPalette(string imageName, int paletteSize)
        {
            Console.WriteLine("TestExtractPalette\n(\n\timageName = {0},\n\tpaletteSize = {1}\n)", imageName, paletteSize);

            // Load the image.
            string imageFileName = String.Format("{0}.source.{1}", imageName, imageFileNameExtension);
            Bitmap image = new Bitmap(imageFileName);

            // Extract the palette from the image.
            Color[] palette = PalettingNetwork.ExtractPalette(image, paletteSize);

            // Save the palette.
            // TODO
        }

        /// <summary>
        /// Tests the ColourDitheringNetwork.DitherImage method.
        /// </summary>
        /// <param name="imageFileName"></param>
        /// <param name="paletteSize"></param>
        /// <param name="radius"></param>
        /// <param name="alpha"></param>
        /// <param name="beta"></param>
        /// <param name="gamma"></param>
        static void TestDitherImage(string imageFileName, int paletteSize, int radius, double alpha, double beta, double gamma)
        {
            Console.WriteLine("TestDitherImage\n* imageFileName = {0}\n* paletteSize = {1}\n* radius = {2}\n* alpha = {3}\n* beta = {4}\n* gamma = {5}\n=== BEGIN ===", imageFileName, paletteSize, radius, alpha, beta, gamma);

            // Load the original image.
            string originalImageFileName = String.Format("{0}.source.{1}", imageFileName, imageFileNameExtension);
            Bitmap originalImage = new Bitmap(originalImageFileName);

            // Dither the original image to get the dithered image.
            Bitmap ditheredImage = ColourDitheringNetwork.DitherImage(originalImage, paletteSize, radius, alpha, beta, gamma);

            // Save the paletted image.
            string ditheredImageFileName = String.Format("{0}.target[paletteSize={1},radius={2},alpha={3},beta={4},gamma={5}].{6}", imageFileName, paletteSize, radius, alpha, beta, gamma, imageFileNameExtension);
            ditheredImage.Save(ditheredImageFileName);

            Console.WriteLine("=== END ===");
        }

        /// <summary>
        /// The extension of the image file name.
        /// </summary>
        static string imageFileNameExtension = "png";
    }
    */
}
