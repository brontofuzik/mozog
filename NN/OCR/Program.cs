using NeuralNetwork.KohonenNetwork;
using NeuralNetwork.MultilayerPerceptron.Training;
using System;

namespace OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            // ------------------------------
            // Step 0: Adjust the parameters.
            // ------------------------------

            // The number of training iterations.
            int trainingIterationCount = 10000;

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            TrainingSet trainingSet = TrainingSet.Load("training.txt");
            TrainingSet validationSet = TrainingSet.Load("validation.txt");
            trainingSet.Add(validationSet);

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int inputLayerNeuronCount = trainingSet.InputVectorLength;
            int[] outputLayerDimensions = new int[] { 26 };
            KohonenNetwork network = new KohonenNetwork(inputLayerNeuronCount, outputLayerDimensions);

            // --------------------------
            // Step 2: Train the network.
            // --------------------------

            network.Train(trainingSet, trainingIterationCount);

            // -------------------------
            // Step 2: Test the network.
            // -------------------------

            TrainingSet testSet = TrainingSet.Load("test.txt");

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
