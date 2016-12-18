using System;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.Examples.HopfieldNetwork
{
    class Examples
    {
        public static void Run()
        {
            TestHopfieldNetwork();
            TestMultiflopNetwork();
            TestEightRooksNetwork();
            TestEightQueensNetwork();
        }

        /// <summary>
        /// Tests the Hopfield network.
        /// </summary>
        static void TestHopfieldNetwork()
        {
            Console.WriteLine("TestHopfieldNetwork");
            Console.WriteLine("===================");

            #region Step 1 : Create the training set.

            // Create the training set.

            TrainingSet trainingSet = new TrainingSet(10, 0);

            // Create the training patterns.

            SupervisedTrainingPattern trainingPattern = new SupervisedTrainingPattern(new double[] { 1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, -1.0 }, new double[0]);
            trainingSet.Add(trainingPattern);

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            int neuronCount = trainingSet.InputVectorLength;
            IHopfieldNetworkImpFactory networkImpFactory = new FullHopfieldNetworkImpFactory();
            NeuralNetwork.HopfieldNetwork.HopfieldNetwork defaultNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(neuronCount, networkImpFactory);

            #endregion Step 2 : Create the network.

            #region Step 3 : Train the network.

            defaultNetwork.Train(trainingSet);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            double[] patternToRecall = new double[] { -1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, 1.0 };
            int iterationCount = 10;
            double[] recalledPattern = defaultNetwork.Evaluate(patternToRecall, iterationCount);

            Console.WriteLine(UnsupervisedTrainingPattern.VectorToString(recalledPattern));

            #endregion // Step 4 : Test the network.

            Console.WriteLine();
        }

        /// <summary>
        /// Tests the multiflop Hopfield network.
        /// </summary>
        static void TestMultiflopNetwork()
        {
            Console.WriteLine("TestMultiflopNetwork");
            Console.WriteLine("====================");

            #region Step 1 : Create the training set.

            // Empty

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            int neuronCount = 4;
            MultiflopHopfieldNetwork multiflopNetwork = new MultiflopHopfieldNetwork(neuronCount);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            multiflopNetwork.Train();

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int iterationCount = 10;
            double[] recalledPattern = multiflopNetwork.Evaluate(iterationCount);

            Console.WriteLine(UnsupervisedTrainingPattern.VectorToString(recalledPattern));

            #endregion // Step 4 : Test the network.

            Console.WriteLine();
        }

        /// <summary>
        /// Tests the eight-rooks Hopfield network.
        /// </summary>
        static void TestEightRooksNetwork()
        {
            Console.WriteLine("TestEightRooksNetwork");
            Console.WriteLine("=====================");

            #region Step 1 : Create the training set.

            // Empty step.

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            EightRooksHopfieldNetwork eightRooksNetwork = new EightRooksHopfieldNetwork();

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            eightRooksNetwork.Train();

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int iterationCount = 10;
            double[] recalledPattern = eightRooksNetwork.Evaluate(iterationCount);

            Console.WriteLine(UnsupervisedTrainingPattern.VectorToString(recalledPattern));

            #endregion // Step 4 : Test the network.

            Console.WriteLine();
        }

        /// <summary>
        /// Tets the eight-queens Hopfield network.
        /// </summary>
        static void TestEightQueensNetwork()
        {
            Console.WriteLine("TestEightQueensNetwork started");
            Console.WriteLine("==============================");

            #region Step 1 : Create the training set.

            // Empty step.

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            EightQueensHopfieldNetwork eightQueensNetwork = new EightQueensHopfieldNetwork();

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            eightQueensNetwork.Train();

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            int iterationCount = 10;
            double[] recalledPattern = eightQueensNetwork.Evaluate(iterationCount);

            Console.WriteLine(UnsupervisedTrainingPattern.VectorToString(recalledPattern));

            #endregion // Step 4 : Test the network.

            Console.WriteLine("TestEightQueensNetwork finished");
            Console.WriteLine();
        }
    }
}
