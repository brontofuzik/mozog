using System;
using NeuralNetwork.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron
{
    static class Examples
    {
        public static void Run()
        {
            Example();
        }

        private static void Example()
        {
            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            const int inputVectorLength = 2;
            const int outputVectorLength = 1;
            TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength);
            trainingSet.Add(new SupervisedTrainingPattern(new[] { 0.0, 0.0 }, new[] { 0.0 }));
            trainingSet.Add(new SupervisedTrainingPattern(new[] { 0.0, 1.0 }, new[] { 1.0 }));
            trainingSet.Add(new SupervisedTrainingPattern(new[] { 1.0, 0.0 }, new[] { 1.0 }));
            trainingSet.Add(new SupervisedTrainingPattern(new[] { 1.0, 1.0 }, new[] { 0.0 }));

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            Network network = new Network(new[] {2, 2, 1}, new LinearActivationFunction());

            /* TODO Backprop
            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            // 3.1. Create the trainer.
            BackpropagationTrainer trainer = new BackpropagationTrainer(trainingSet, null, null);

            // 3.2. Create the training strategy.
            int maxIterationCount = 10000;
            double maxNetworkError = 0.001;
            bool batchLearning = true;

            double synapseLearningRate = 0.01;
            double connectorMomentum = 0.9;

            BackpropagationTrainingStrategy backpropagationTrainingStrategy = new BackpropagationTrainingStrategy(maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum);

            // 3.3. Train the network.
            TrainingLog trainingLog = trainer.Train(network, backpropagationTrainingStrategy);

            // 3.4. Inspect the training log.
            Console.WriteLine("Number of iterations used : " + trainingLog.IterationCount);
            Console.WriteLine("Minimum network error achieved : " + trainingLog.NetworkError);
            */

            // ---------------------------------
            // Step 4: Test the trained network.
            // ---------------------------------

            foreach (var pattern in trainingSet)
            {
                double[] inputVector = pattern.InputVector;
                double[] outputVector = network.Evaluate(inputVector);
                Console.WriteLine(pattern + " -> " + UnsupervisedTrainingPattern.VectorToString(outputVector));
            }
        }
    }
}
