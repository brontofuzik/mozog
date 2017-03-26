using System;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;
using NeuralNetwork.MultilayerPerceptron.Training.Backpropagation;

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

            // 1.1. Create the training set.
            int inputVectorLength = 2;
            int outputVectorLength = 1;
            TrainingSet trainingSet = new TrainingSet(inputVectorLength, outputVectorLength);

            // 1.2. Create the training patterns.
            SupervisedTrainingPattern trainingPattern;
            trainingPattern = new SupervisedTrainingPattern(new double[2] { 0.0, 0.0 }, new double[1] { 0.0 });
            trainingSet.Add(trainingPattern);
            trainingPattern = new SupervisedTrainingPattern(new double[2] { 0.0, 1.0 }, new double[1] { 1.0 });
            trainingSet.Add(trainingPattern);
            trainingPattern = new SupervisedTrainingPattern(new double[2] { 1.0, 0.0 }, new double[1] { 1.0 });
            trainingSet.Add(trainingPattern);
            trainingPattern = new SupervisedTrainingPattern(new double[2] { 1.0, 1.0 }, new double[1] { 0.0 });
            trainingSet.Add(trainingPattern);

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            // 2.1. Create the blueprint of the netowork.

            // 2.1.1. Create the blueprint of the input layer.
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint(inputVectorLength);

            // 2.1.2. Create the blueprints of the hidden layers.
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[1];
            hiddenLayerBlueprints[0] = new ActivationLayerBlueprint(2, new LogisticActivationFunction());

            // 2.1.3. Create the blueprints of the output layer.
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint(outputVectorLength, new LogisticActivationFunction());

            // 2.1.4. Create the blueprint of the network.
            NetworkBlueprint networkBlueprint = new NetworkBlueprint(inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint);

            // 2.2. : Create the network.
            Network network = new Network(networkBlueprint);

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

            // ---------------------------------
            // Step 4: Test the trained network.
            // ---------------------------------

            foreach (SupervisedTrainingPattern tp in trainingSet)
            {
                double[] inputVector = tp.InputVector;
                double[] outputVector = network.Evaluate(inputVector);
                Console.WriteLine(tp + " -> " + UnsupervisedTrainingPattern.VectorToString(outputVector));
            }
        }
    }
}
