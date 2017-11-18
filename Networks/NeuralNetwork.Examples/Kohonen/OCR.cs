using System;
using NeuralNetwork.Data;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.Kohonen
{
    static class OCR
    {
        public static void Run()
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            var trainingSetSerializer = new DataSetSerializer();
            DataSet trainingSet = trainingSetSerializer.Deserialize("training.txt");
            DataSet validationSet = trainingSetSerializer.Deserialize("validation.txt");
            trainingSet.Add(validationSet);

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int inputLayerNeuronCount = trainingSet.InputSize;
            int[] outputLayerDimensions = new int[] { 26 };
            NeuralNetwork.Kohonen.KohonenNetwork network = new NeuralNetwork.Kohonen.KohonenNetwork(inputLayerNeuronCount, outputLayerDimensions);

            // --------------------------
            // Step 2: Train the network.
            // --------------------------

            network.Train(trainingSet, iterations: 10000);

            // -------------------------
            // Step 2: Test the network.
            // -------------------------

            DataSet testSet = trainingSetSerializer.Deserialize("test.txt");

            for (int letterIndex = 0; letterIndex < 26; letterIndex++)
            {
                Console.Write((char)(letterIndex + (int)'a') + " : ");
                foreach (var point in testSet)
                {
                    if (point.Output[letterIndex] == 1.0)
                    {
                        int[] winnerOutputNeuronCoordinates = network.Evaluate(point.Input);
                        Console.Write(winnerOutputNeuronCoordinates[0] + ", ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
