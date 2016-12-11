using NeuralNetwork.KohonenNetwork;
using NeuralNetwork.MultilayerPerceptron.Training;
using System;

namespace OCR
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Step 0 : Adjust the parameters.

            // The number of training iterations.
            int trainingIterationCount = 10000;

            #endregion // Step 0 : Adjust the parameters.

            #region Step 1 : Create the training set.

            TrainingSet trainingSet = TrainingSet.Load("training.txt");
            TrainingSet validationSet = TrainingSet.Load("validation.txt");
            trainingSet.Add(validationSet);

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            int inputLayerNeuronCount = trainingSet.InputVectorLength;
            int[] outputLayerDimensions = new int[] { 26 };
            KohonenNetwork network = new KohonenNetwork(inputLayerNeuronCount, outputLayerDimensions);

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            network.Train(trainingSet, trainingIterationCount);

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

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

            #endregion // Step 4 : Test the network.
        }
    }
}
