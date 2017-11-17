using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Kohonen.LearningRateFunctions;
using NeuralNetwork.Kohonen.NeighbourhoodFunctions;

namespace NeuralNetwork.Kohonen
{
    public class KohonenNetwork : IKohonenNetwork
    {
        private readonly double[] inputNeuronOutputs;
        private readonly int outputLayerDimension;
        private readonly int[] outputLayerDimensions;
        private readonly double[][] outputNeuronWeights;
        private readonly double[] outputNeuronOutputs;
        private readonly double outputLayerDiameter;

        private ILearningRateFunction learningRateFunction;
        private ILearningRateFunction neighbourhoodRadiusFunction;
        private INeighbourhoodFunction neighbourhoodFunction;

        public KohonenNetwork(int inputLayerNeurons, int[] outputLayerDimensions)
        {
            Require.IsPositive(inputLayerNeurons, nameof(inputLayerNeurons));
            Require.IsPositive(outputLayerDimensions.Length, nameof(outputLayerDimensions));

            foreach (int dim in outputLayerDimensions)
                Require.IsPositive(dim, "outputLayerDimension");

            // ---------------------------
            // Initialize the input layer.
            // ---------------------------

            // Initialize the number of neurons in the input layer (i.e. the input neurons).
            InputNeuronCount = inputLayerNeurons;

            // Initialize the outputs of the input neurons.
            inputNeuronOutputs = new double[inputLayerNeurons];

            // ----------------------------
            // Initialize the output layer.
            // ----------------------------

            // INitialize the number of neurons in the output layer (i.e. the output nerouns).
            outputLayerDimension = outputLayerDimensions.Length;

            // Initialize the dimensions of the output layer.
            this.outputLayerDimensions = outputLayerDimensions;

            // Calculate the number of output neurons.
            OutputNeuronCount = 1;
            foreach (int outputLayerDimension in outputLayerDimensions)
            {
                OutputNeuronCount *= outputLayerDimension;
            }

            // Initialize the weights of the output neurons.
            outputNeuronWeights = new double[OutputNeuronCount][];
            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                outputNeuronWeights[outputNeuronIndex] = new double[InputNeuronCount];
            }

            // Initialize the outputs of the output neurons.
            outputNeuronOutputs = new double[OutputNeuronCount];

            // Calculate the diameter of the output layer (using Pythagoras theorem).
            double tmp = 0.0;
            for (int i = 0; i < outputLayerDimension; ++i)
            {
                tmp += this.outputLayerDimensions[i] * this.outputLayerDimensions[i];
            }
            outputLayerDiameter = System.Math.Sqrt(tmp);
        }

        #region Events

        public event EventHandler<DataSetEventArgs> BeforeTrainingSet;

        public event EventHandler<DataSetEventArgs> AfterTrainingSet;

        public event EventHandler<DataPointEventArgs> BeforeTrainingPoint;

        public event EventHandler<DataPointEventArgs> AfterTrainingPoint;

        #endregion // Events

        public int InputNeuronCount { get; }

        public int OutputNeuronCount { get; }

        public void Train(DataSet trainingSet, int iterations)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));

            if (trainingSet.InputSize != InputNeuronCount)
            {
                throw new ArgumentException("The training set is not compatible with the network (i.e. the length of the input vector does not match the number of neurons in the input layer).", nameof(trainingSet));
            }

            Require.IsPositive(iterations, nameof(iterations));

            // The learning rate function.
            double initialLearningRate = 0.999;
            double finalLearningRate = 0.001;
            learningRateFunction = new ExponentialLearningRateFunction(iterations, initialLearningRate, finalLearningRate);

            // The neighbourhood neighbourhoodRadius function.
            double initialNeighbourhoodRadius = outputLayerDiameter;
            double finalneighbourhoodRadius = 0.001;
            neighbourhoodRadiusFunction = new ExponentialLearningRateFunction(iterations, initialNeighbourhoodRadius, finalneighbourhoodRadius);

            // The neighbourhood function.
            neighbourhoodFunction = new GaussianNeighbourhoodFunction();

            // Initialize the network.
            double minWeight = 0.0;
            double maxWeight = 1.0;
            Initialize(minWeight, maxWeight);

            for (int trainingIterationIndex = 0; trainingIterationIndex < iterations; ++trainingIterationIndex)
            {
                // Calculate the learning rate.
                double learningRate = learningRateFunction.Evaluate(trainingIterationIndex);
                
                // Calculate the neighbourhood radius.
                double neighbourhoodRadius = neighbourhoodRadiusFunction.Evaluate(trainingIterationIndex);
                
                Trace.WriteLine($"{trainingIterationIndex}: learning rate = {learningRate:0.000}, neighbourhood radius = {neighbourhoodRadius:0.000}");

                TrainSet(trainingSet, trainingIterationIndex, learningRate, neighbourhoodRadius);
            }
        }

        private void Initialize(double minWeight, double maxWeight)
        {
            if (maxWeight < minWeight)
                throw new ArgumentException("The maximum weight must be greater than or equal to the minimum weight.", nameof(maxWeight));

            // Initialize the input layer.
            for (int inputNeuronIndex = 0; inputNeuronIndex < InputNeuronCount; ++inputNeuronIndex)
                inputNeuronOutputs[inputNeuronIndex] = 0.0;

            // Initialize the output layer.
            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                for (int weightIndex = 0; weightIndex < InputNeuronCount; ++weightIndex)
                    outputNeuronWeights[outputNeuronIndex][weightIndex] = StaticRandom.Double(minWeight, maxWeight);

                outputNeuronOutputs[outputNeuronIndex] = 0.0;
            }
        }

        private void TrainSet(IDataSet trainingSet, int iteration, double learningRate, double neighbourhoodRadius)
        {
            BeforeTrainingSet?.Invoke(this, new DataSetEventArgs(trainingSet, iteration));

            foreach (var point in trainingSet.Random())
                TrainPattern(point, iteration, learningRate, neighbourhoodRadius);

            AfterTrainingSet?.Invoke(this, new DataSetEventArgs(trainingSet, iteration));
        }

        private void TrainPattern(ILabeledDataPoint trainingPattern, int trainingIterationIndex, double learningRate, double neighbourhoodRadius)
        {
            BeforeTrainingPoint?.Invoke(this, new DataPointEventArgs(trainingPattern, trainingIterationIndex));

            int[] winnerOutputNeuronCoordinates = Evaluate(trainingPattern.Input);
            AdaptOutputLayerNeuronWeights(trainingPattern, winnerOutputNeuronCoordinates, learningRate, neighbourhoodRadius);

            AfterTrainingPoint?.Invoke(this, new DataPointEventArgs(trainingPattern, trainingIterationIndex));
        }

        private void AdaptOutputLayerNeuronWeights(ILabeledDataPoint trainingPattern, int[] winnerOutputNeuronCoordinates, double learningRate, double neighbourhoodRadius)
        {
            // (OPTIMIZATION) No need to adapt if the learning rate is zero.
            if (learningRate == 0.0)
            {
                return;
            }

            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                // Get the coordinates of the output neuron.
                int[] outputNeuronCoordinates = OutputNeuronIndexToCoordinates(outputNeuronIndex);

                // Calculate the distance between the output neuron and the winner output neuron.
                double distanceBetweenOutputNeurons = Vector.Distance(outputNeuronCoordinates, winnerOutputNeuronCoordinates);

                // Calculate the neighbourhood.
                double neighbourhood = neighbourhoodFunction.Evaluate(distanceBetweenOutputNeurons, neighbourhoodRadius);

                // (OPTIMIZATION) No need to adapt if the neighbourhood is zero.
                if (neighbourhood == 0.0)
                {
                    continue;
                }

                for (int weightIndex = 0; weightIndex < InputNeuronCount; ++weightIndex)
                {
                    double weightDelta = learningRate * neighbourhood * (trainingPattern.Input[weightIndex] - outputNeuronWeights[outputNeuronIndex][weightIndex]);
                    outputNeuronWeights[outputNeuronIndex][weightIndex] += weightDelta;
                }
            }
        }

        public int[] Evaluate(double[] input)
        {
            Require.IsNotNull(input, nameof(input));

            if (input.Length != InputNeuronCount)
            {
                throw new ArgumentException("The input vector is not compatible with the network (i.e. the length of the input vector does not match the number of input neurons).", nameof(input));
            }

            // Set the outputs of the input neurons.
            Array.Copy(input, inputNeuronOutputs, InputNeuronCount);

            // Evaluate the output neurons and determine the winner output neuron.
            int winnerOutputNeuronIndex = 0;
            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                outputNeuronOutputs[outputNeuronIndex] = Vector.Distance(outputNeuronWeights[outputNeuronIndex], inputNeuronOutputs);

                // Update the winner neuron index.
                if (outputNeuronOutputs[outputNeuronIndex] < outputNeuronOutputs[winnerOutputNeuronIndex])
                {
                    winnerOutputNeuronIndex = outputNeuronIndex;
                }
            }

            // Convert the index of the winner output neuron to its coordinates.
            int[] winnerOutputNeuronCoordinates = OutputNeuronIndexToCoordinates(winnerOutputNeuronIndex);

            return winnerOutputNeuronCoordinates;
        }

        public double[] GetOutputNeuronSynapseWeights(int neuronIndex)
            => outputNeuronWeights[neuronIndex];

        public double[] GetOutputNeuronSynapseWeights(int[] neuronCoordinates)
        {
            int outputNeuronIndex = OutputNeuronCoordinatesToIndex(neuronCoordinates);
            return GetOutputNeuronSynapseWeights(outputNeuronIndex);
        }

        public Bitmap ToBitmap(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Black);

            int neuronDiameter = 3;

            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputNeuronCount; ++outputNeuronIndex)
            {
                // Draw the neuron.
                double xWeight = outputNeuronWeights[outputNeuronIndex][0];
                int xCoordinate = (int)System.Math.Round(xWeight * width);
                double yWeight = outputNeuronWeights[outputNeuronIndex].Length > 1 ? outputNeuronWeights[outputNeuronIndex][1] : 0.5;
                int yCoordinate = (int)System.Math.Round(yWeight * height);

                graphics.DrawEllipse(pen, xCoordinate, yCoordinate, neuronDiameter, neuronDiameter);

                // Draw the grid.
                ICollection<int> neighbourOutputNeuronsIndices = GetNeighbourOutputNeuronsIndices(outputNeuronIndex);
                foreach (int neighbourOutputNeuronIndex in neighbourOutputNeuronsIndices)
                {
                    double neighbourXWeight = outputNeuronWeights[neighbourOutputNeuronIndex][0];
                    int neighbourXCoordinate = (int)System.Math.Round(neighbourXWeight * width);
                    double neighbourYWeight = outputNeuronWeights[neighbourOutputNeuronIndex].Length > 1 ? outputNeuronWeights[neighbourOutputNeuronIndex][1] : 0.5;
                    int neighbourYCoordinate = (int)System.Math.Round(neighbourYWeight * height);

                    graphics.DrawLine(pen, xCoordinate, yCoordinate, neighbourXCoordinate, neighbourYCoordinate);
                }
            }

            return bitmap;
        }

        private ICollection<int> GetNeighbourOutputNeuronsIndices(int outputNeuronIndex)
        {
            int[] outputNeuronCoordinates = OutputNeuronIndexToCoordinates(outputNeuronIndex);

            ICollection<int> neighbourOutputNeuronsIndices = new HashSet<int>();
            for (int d = 0; d < outputLayerDimension; ++d)
            {
                int outputNeuronCoordinate = outputNeuronCoordinates[d];

                if (outputNeuronCoordinate > 0)
                {
                    int[] neighbourOutputNeuronCoordinates = new int[outputLayerDimension];
                    Array.Copy(outputNeuronCoordinates, neighbourOutputNeuronCoordinates, outputLayerDimension);
                    neighbourOutputNeuronCoordinates[d] = outputNeuronCoordinate - 1;

                    int neighbourOutputNeuronIndex = OutputNeuronCoordinatesToIndex(neighbourOutputNeuronCoordinates);
                    neighbourOutputNeuronsIndices.Add(neighbourOutputNeuronIndex);
                }

                if (outputNeuronCoordinate < outputLayerDimensions[d] - 1)
                {
                    int[] neighbourOutputNeuronCoordinates = new int[outputLayerDimension];
                    Array.Copy(outputNeuronCoordinates, neighbourOutputNeuronCoordinates, outputLayerDimension);
                    neighbourOutputNeuronCoordinates[d] = outputNeuronCoordinate + 1;

                    int neighbourOutputNeuronIndex = OutputNeuronCoordinatesToIndex(neighbourOutputNeuronCoordinates);
                    neighbourOutputNeuronsIndices.Add(neighbourOutputNeuronIndex);
                }
            }

            return neighbourOutputNeuronsIndices;
        }

        private int[] OutputNeuronIndexToCoordinates(int neuronIndex)
        {
            int[] outputNeuronCoordinates = new int[outputLayerDimension];
            for (int i = outputLayerDimension - 1; i >= 0; --i)
            {
                int lowerDimensionsNeuronCount = 1;
                for (int j = i - 1; j >= 0; --j)
                {
                    lowerDimensionsNeuronCount *= outputLayerDimensions[j];
                }
                outputNeuronCoordinates[i] = neuronIndex / lowerDimensionsNeuronCount;
                neuronIndex = neuronIndex % lowerDimensionsNeuronCount;
            }
            return outputNeuronCoordinates;
        }

        private int OutputNeuronCoordinatesToIndex(int[] neuronCoordinates)
        {
            int outputNeuronIndex = 0;
            for (int i = outputLayerDimension - 1; i >= 0; --i)
            {
                int lowerDimensionsNeuronCount = 1;
                for (int j = i - 1; j >= 0; --j)
                {
                    lowerDimensionsNeuronCount *= outputLayerDimensions[j];
                }
                outputNeuronIndex += neuronCoordinates[i] * lowerDimensionsNeuronCount;
            }
            return outputNeuronIndex;
        }

        ///// <summary>
        ///// Normalizes the weight vectors of the output neurons.
        ///// </summary>
        //private void NormalizeOutputLayerNeuronWeights()
        //{
        //    for (int neuronIndex = 0; neuronIndex < _outputLayerNeuronCount; ++neuronIndex)
        //    {
        //        double[] normalizedWeightVector = SupervisedTrainingPattern.NormalizeVector(outputNeuronWeights[neuronIndex]);
        //        Array.Copy(normalizedWeightVector, outputNeuronWeights[neuronIndex], _inputLayerNeurons);
        //    }
        //}
    }
}
