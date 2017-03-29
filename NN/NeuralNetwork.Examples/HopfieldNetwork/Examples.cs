using System;
using Mozog.Utils;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp;
using NeuralNetwork.Training;

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

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            // Create the training set.

            TrainingSet trainingSet = new TrainingSet(10, 0);

            // Create the training patterns.

            SupervisedTrainingPattern trainingPattern = new SupervisedTrainingPattern(new[] { 1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, -1.0 }, new double[0]);
            trainingSet.Add(trainingPattern);

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int neuronCount = trainingSet.InputVectorLength;
            IHopfieldNetworkImpFactory networkImpFactory = new FullHopfieldNetworkImpFactory();
            NeuralNetwork.HopfieldNetwork.HopfieldNetwork defaultNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(neuronCount, networkImpFactory);

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            defaultNetwork.Train(trainingSet);

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            double[] patternToRecall = { -1.0, 1.0, 1.0, -1.0, -1.0, -1.0, 1.0, -1.0, 1.0, 1.0 };
            int iterationCount = 10;
            double[] recalledPattern = defaultNetwork.Evaluate(patternToRecall, iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine();
        }

        /// <summary>
        /// Tests the multiflop Hopfield network.
        /// </summary>
        static void TestMultiflopNetwork()
        {
            Console.WriteLine("TestMultiflopNetwork");
            Console.WriteLine("====================");

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            // Do nothing.

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int neuronCount = 4;
            MultiflopHopfieldNetwork multiflopNetwork = new MultiflopHopfieldNetwork(neuronCount);

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            multiflopNetwork.Train();

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            int iterationCount = 10;
            double[] recalledPattern = multiflopNetwork.Evaluate(iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine();
        }

        /// <summary>
        /// Tests the eight-rooks Hopfield network.
        /// </summary>
        static void TestEightRooksNetwork()
        {
            Console.WriteLine("TestEightRooksNetwork");
            Console.WriteLine("=====================");

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            // Do nothing.

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            EightRooksHopfieldNetwork eightRooksNetwork = new EightRooksHopfieldNetwork();

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            eightRooksNetwork.Train();

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            int iterationCount = 10;
            double[] recalledPattern = eightRooksNetwork.Evaluate(iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine();
        }

        /// <summary>
        /// Tets the eight-queens Hopfield network.
        /// </summary>
        static void TestEightQueensNetwork()
        {
            Console.WriteLine("TestEightQueensNetwork started");
            Console.WriteLine("==============================");

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            // Do nothing.

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            EightQueensHopfieldNetwork eightQueensNetwork = new EightQueensHopfieldNetwork();

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            eightQueensNetwork.Train();

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            int iterationCount = 10;
            double[] recalledPattern = eightQueensNetwork.Evaluate(iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine("TestEightQueensNetwork finished");
            Console.WriteLine();
        }
    }
}
