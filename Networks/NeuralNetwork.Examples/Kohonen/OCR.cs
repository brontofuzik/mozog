using System;
using NeuralNetwork.Data;

namespace NeuralNetwork.Examples.Kohonen
{
    static class OCR
    {
        public static void Run()
        {
            // Step 1: Create the training set

            var serializer = new DataSetSerializer();
            var trainingSet = serializer.Deserialize("training.txt");
            var validationSet = serializer.Deserialize("validation.txt");
            trainingSet.Add(validationSet);

            // Step 2: Create the network

            var net = new NeuralNetwork.Kohonen.KohonenNetwork(trainingSet.InputSize, new[] { 26 });

            // Step 2: Train the network

            net.Train(trainingSet, iterations: 10000);

            // Step 2: Test the network

            var testSet = serializer.Deserialize("test.txt");

            for (int l = 0; l < 26; l++)
            {
                Console.Write($"{(char)(l + 'a')}: ");
                foreach (var point in testSet)
                {
                    if (point.Output[l] == 1.0)
                    {
                        int[] winnerCoordinates = net.Evaluate(point.Input);
                        Console.Write($"{winnerCoordinates[0]}, ");
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
