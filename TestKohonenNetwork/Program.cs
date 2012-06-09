using System;
using System.Drawing;
using System.Drawing.Imaging;

using NeuralNetwork;
using NeuralNetwork.KohonenNetwork;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.KohonenTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1DTo1D();
            Test1DTo2D();
            Test2DTo1D();
            Test2DTo2D();
        }

        /// <summary>
        /// Tests the 1D input space to 1D output space mapping.
        /// </summary>
        static void Test1DTo1D()
        {
            Console.WriteLine("Test1DTo1D started");
            Console.WriteLine("==================");

            #region Step 0 : Adjust the parameters.

            // The number of training samples.
            int trainingSampleCount = 1000;

            // The dimensions of the output layer.
            int[] outputLayerDimensions = new int[] { 100 };

            // The number of training iterations.
            int trainingIterationCount = 1000;

            #endregion // Step 0 : Adjust the parameters.

            #region Step 1 : Create the training set.

            TrainingSet trainingSet = new TrainingSet(1);
            for (int trainingSampleIndex = 0; trainingSampleIndex < trainingSampleCount; ++trainingSampleIndex)
            {
                double[] vector = new double[1];
                for (int i = 0; i < 1; ++i)
                {
                    vector[i] = _random.NextDouble();
                }
                SupervisedTrainingPattern trainingSample = new SupervisedTrainingPattern(vector, new double[0]);
                trainingSet.Add(trainingSample);
            }

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            NeuralNetwork.KohonenNetwork.KohonenNetwork network = new NeuralNetwork.KohonenNetwork.KohonenNetwork(1, outputLayerDimensions);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            network.Train(trainingSet, trainingIterationCount);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int bitmapWidth = 500;
            int bitmapHeight = 500;
            Bitmap bitmap = network.ToBitmap(bitmapWidth, bitmapHeight);
            bitmap.Save("Test1DTo1D.png", ImageFormat.Png);

            #endregion // Step 4 : Test the network.

            Console.WriteLine("Test1DTo1D finished");
            Console.WriteLine();
        }

        /// <summary>
        /// Tests the 1D input space to 2D output space mapping.
        /// </summary>
        static void Test1DTo2D()
        {
            Console.WriteLine("Test1DTo2D started");
            Console.WriteLine("==================");

            #region Step 0 : Adjust the parameters.

            // The number of training samples.
            int trainingSampleCount = 1000;

            // The dimensions of the output layer.
            int[] outputLayerDimensions = new int[] { 10, 10 };

            // The number of training iterations.
            int trainingIterationCount = 1000;

            #endregion // Step 0 : Adjust the parameters.

            #region Step 1 : Create the training set.

            TrainingSet trainingSet = new TrainingSet(1);
            for (int trainingSampleIndex = 0; trainingSampleIndex < trainingSampleCount; ++trainingSampleIndex)
            {
                double[] vector = new double[1];
                for (int i = 0; i < 1; ++i)
                {
                    vector[i] = _random.NextDouble();
                }
                SupervisedTrainingPattern trainingSample = new SupervisedTrainingPattern(vector, new double[0]);
                trainingSet.Add(trainingSample);
            }

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            NeuralNetwork.KohonenNetwork.KohonenNetwork network = new NeuralNetwork.KohonenNetwork.KohonenNetwork(1, outputLayerDimensions);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            network.Train(trainingSet, trainingIterationCount);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int bitmapWidth = 500;
            int bitmapHeight = 500;
            Bitmap bitmap = network.ToBitmap(bitmapWidth, bitmapHeight);
            bitmap.Save("Test1DTo2D.png", ImageFormat.Png);

            #endregion // Step 4 : Test the network.

            Console.WriteLine("Test1DTo2D finished");
            Console.WriteLine();
        }

        /// <summary>
        /// Tests the 2D input space to 1D output space mapping.
        /// </summary>
        static void Test2DTo1D()
        {
            Console.WriteLine("Test2DTo1D started");
            Console.WriteLine("==================");

            #region Step 0 : Adjust the parameters.

            // The number of training samples.
            int trainingSampleCount = 1000;

            // The dimensions of the output layer.
            int[] outputLayerDimensions = new int[] { 100 };

            // The number of training iterations.
            int trainingIterationCount = 1000;

            #endregion // Step 0 : Adjust the parameters.

            #region Step 1 : Create the training set.

            TrainingSet trainingSet = new TrainingSet(2);
            for (int trainingSampleIndex = 0; trainingSampleIndex < trainingSampleCount; ++trainingSampleIndex)
            {
                double[] vector = new double[2];
                for (int i = 0; i < 2; ++i)
                {
                    vector[i] = _random.NextDouble();
                }
                SupervisedTrainingPattern trainingSample = new SupervisedTrainingPattern(vector, new double[0]);
                trainingSet.Add(trainingSample);
            }

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            NeuralNetwork.KohonenNetwork.KohonenNetwork network = new NeuralNetwork.KohonenNetwork.KohonenNetwork(2, outputLayerDimensions);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            network.Train(trainingSet, trainingIterationCount);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int bitmapWidth = 500;
            int bitmapHeight = 500;
            Bitmap bitmap = network.ToBitmap(bitmapWidth, bitmapHeight);
            bitmap.Save("Test2DTo1D.png", ImageFormat.Png);

            #endregion // Step 4 : Test the network.

            Console.WriteLine("Test2DTo1D finished");
            Console.WriteLine();
        }

        /// <summary>
        /// Test the 2D input space to 2D output space mapping.
        /// </summary>
        static void Test2DTo2D()
        {
            Console.WriteLine("Test2DTo2D started");
            Console.WriteLine("==================");

            #region Step 0 : Adjust the parameters.

            // The number of training samples.
            int trainingSampleCount = 1000;

            // The dimensions of the output layer.
            int[] outputLayerDimensions = new int[] { 10, 10 };

            // The number of training iterations.
            int trainingIterationCount = 1000;

            #endregion // Step 0 : Adjust the parameters.

            #region Step 1 : Create the training set.

            TrainingSet trainingSet = new TrainingSet(2);
            for (int trainingSampleIndex = 0; trainingSampleIndex < trainingSampleCount; ++trainingSampleIndex)
            {
                double[] vector = new double[2];
                for (int i = 0; i < 2; ++i)
                {
                    vector[i] = _random.NextDouble();
                }
                SupervisedTrainingPattern trainingSample = new SupervisedTrainingPattern(vector, new double[0]);
                trainingSet.Add(trainingSample);
            }

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            NeuralNetwork.KohonenNetwork.KohonenNetwork network = new NeuralNetwork.KohonenNetwork.KohonenNetwork(2, outputLayerDimensions);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            network.Train(trainingSet, trainingIterationCount);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int bitmapWidth = 500;
            int bitmapHeight = 500;
            Bitmap bitmap = network.ToBitmap(bitmapWidth, bitmapHeight);
            bitmap.Save("Test2DTo2D.png", ImageFormat.Png);

            #endregion // Step 4 : Test the network.

            Console.WriteLine("Test2DTo2D finished");
            Console.WriteLine();
        }

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        static Random _random = new Random();
    }
}
