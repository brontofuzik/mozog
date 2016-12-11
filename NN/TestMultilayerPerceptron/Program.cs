using System;
using System.Collections;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;
using NeuralNetwork.MultilayerPerceptron.Training.Teachers;
using NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher;

namespace NeuralNetwork.PerceptronTest
{
    class Program
    {
        static void Main( string[] args )
        {
            #region Step 1 : Create the training set.

            // Step 1 : Create the training set.
            // ---------------------------------

            // 1.1. Create the training set.
            int inputVectorLength = 2;
            int outputVectorLength = 1;
            TrainingSet trainingSet = new TrainingSet( inputVectorLength, outputVectorLength );

            // 1.2. Create the training patterns.
            SupervisedTrainingPattern trainingPattern;
            trainingPattern = new SupervisedTrainingPattern( new double[ 2 ] { 0.0,  0.0 }, new double[ 1 ] { 0.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new SupervisedTrainingPattern( new double[ 2 ] { 0.0,  1.0 }, new double[ 1 ] { 1.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new SupervisedTrainingPattern( new double[ 2 ] { 1.0,  0.0 }, new double[ 1 ] { 1.0 } );
            trainingSet.Add( trainingPattern );
            trainingPattern = new SupervisedTrainingPattern( new double[ 2 ] { 1.0,  1.0 }, new double[ 1 ] { 0.0 } );
            trainingSet.Add( trainingPattern );

            #endregion // Step 1 : Create the training set.

            #region Step 2 : Create the network.

            // Step 2 : Create the network.
            // ----------------------------

            // 2.1. Create the blueprint of the netowork.

            // 2.1.1. Create the blueprint of the input layer.
            LayerBlueprint inputLayerBlueprint = new LayerBlueprint( inputVectorLength );

            // 2.1.2. Create the blueprints of the hidden layers.
            ActivationLayerBlueprint[] hiddenLayerBlueprints = new ActivationLayerBlueprint[1];
            hiddenLayerBlueprints[ 0 ] = new ActivationLayerBlueprint( 2, new LogisticActivationFunction() );

            // 2.1.3. Create the blueprints of the output layer.
            ActivationLayerBlueprint outputLayerBlueprint = new ActivationLayerBlueprint( outputVectorLength, new LogisticActivationFunction() );

            // 2.1.4. Create the blueprint of the network.
            NetworkBlueprint networkBlueprint = new NetworkBlueprint( inputLayerBlueprint, hiddenLayerBlueprints, outputLayerBlueprint );

            // 2.2. : Create the network.
            Network network = new Network( networkBlueprint );

            #endregion // Step 2 : Create the network.

            #region Step 3 : Train the network.

            // Step 3 : Train the network.
            // ---------------------------

            // 3.1. Create the teacher.
            BackpropagationTeacher teacher = new BackpropagationTeacher( trainingSet, null, null );

            // 3.2. Create the training strategy.
            int maxIterationCount = 10000;
            double maxNetworkError = 0.001;
            bool batchLearning = true;

            double synapseLearningRate = 0.01;
            double connectorMomentum = 0.9;

            BackpropagationTrainingStrategy backpropagationTrainingStrategy = new BackpropagationTrainingStrategy( maxIterationCount, maxNetworkError, batchLearning, synapseLearningRate, connectorMomentum );

            // 3.3. Train the network.
            TrainingLog trainingLog = teacher.Train( network, backpropagationTrainingStrategy );

            // 3.4. Inspect the training log.
            Console.WriteLine( "Number of iterations used : " + trainingLog.IterationCount );
            Console.WriteLine( "Minimum network error achieved : " + trainingLog.NetworkError );

            #endregion // Step 3 : Train the network.

            #region Step 4 : Test the network.

            // Step 4 : Test the trained network.
            // ----------------------------------

            foreach (SupervisedTrainingPattern tp in trainingSet)
            {
                double[] inputVector = tp.InputVector;
                double[] outputVector = network.Evaluate( inputVector );
                Console.WriteLine( tp.ToString() + " -> " + SupervisedTrainingPattern.VectorToString( outputVector ) );
            }

            #endregion // Step 4 : Test the network.
        }
    }
}