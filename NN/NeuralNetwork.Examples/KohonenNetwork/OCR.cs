using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.KohonenNetwork
{
    static class OCR
    {
        public static void Run()
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            var trainingSetSerializer = new TrainingSetSerializer();
            TrainingSet trainingSet = trainingSetSerializer.Deserialize("training.txt");
            TrainingSet validationSet = trainingSetSerializer.Deserialize("validation.txt");
            trainingSet.Add(validationSet);

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int inputLayerNeuronCount = trainingSet.InputVectorLength;
            int[] outputLayerDimensions = new int[] { 26 };
            NeuralNetwork.KohonenNetwork.KohonenNetwork network = new NeuralNetwork.KohonenNetwork.KohonenNetwork(inputLayerNeuronCount, outputLayerDimensions);

            // --------------------------
            // Step 2: Train the network.
            // --------------------------

            network.Train(trainingSet, trainingIterationCount: 10000);

            // -------------------------
            // Step 2: Test the network.
            // -------------------------

            TrainingSet testSet = trainingSetSerializer.Deserialize("test.txt");

            for (int letterIndex = 0; letterIndex < 26; letterIndex++)
            {
                Console.Write((char)(letterIndex + (int)'a') + " : ");
                foreach (SupervisedTrainingPattern trainingPattern in testSet)
                {
                    if (trainingPattern.OutputVector[letterIndex] == 1.0)
                    {
                        int[] winnerOutputNeuronCoordinates = network.Evaluate(trainingPattern.InputVector);
                        Console.Write(winnerOutputNeuronCoordinates[0] + ", ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
