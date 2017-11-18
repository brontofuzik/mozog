using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
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
        private readonly int outputDimension;
        private readonly int[] outputSizes;
        private readonly double[][] outputNeuronWeights;
        private readonly double[] outputNeuronOutputs;
        private readonly double outputDiameter;

        private ILearningRateFunction learningRateFunction;
        private ILearningRateFunction neighbourhoodRadiusFunction;
        private INeighbourhoodFunction neighbourhoodFunction;

        public KohonenNetwork(int inputSize, int[] outputSizes)
        {
            Require.IsPositive(inputSize, nameof(inputSize));
            Require.IsPositive(outputSizes.Length, nameof(outputSizes));

            foreach (int size in outputSizes)
                Require.IsPositive(size, nameof(size));

            // Input layer

            InputSize = inputSize;
            inputNeuronOutputs = new double[inputSize];

            // Output layer

            this.outputSizes = outputSizes;
            outputDimension = outputSizes.Length;
            OutputSize = outputSizes.Product();
       
            outputNeuronWeights = Enumerable.Repeat(new double[InputSize], OutputSize).ToArray();
            outputNeuronOutputs = new double[OutputSize];

            // Pythagoras theorem
            outputDiameter = System.Math.Sqrt(outputSizes.Sum(s => s * s));
        }

        #region Events

        public event EventHandler<DataSetEventArgs> BeforeTrainingSet;

        public event EventHandler<DataSetEventArgs> AfterTrainingSet;

        public event EventHandler<DataPointEventArgs> BeforeTrainingPoint;

        public event EventHandler<DataPointEventArgs> AfterTrainingPoint;

        #endregion // Events

        public int InputSize { get; }

        public int OutputSize { get; }

        private IEnumerable<int> OutputNeurons => Enumerable.Range(0, OutputSize);

        public void Train(DataSet data, int iterations)
        {
            Require.IsNotNull(data, nameof(data));

            if (data.InputSize != InputSize)
                throw new ArgumentException("The training set is not compatible with the network (i.e. the length of the input vector does not match the number of neurons in the input layer).", nameof(data));

            Require.IsPositive(iterations, nameof(iterations));

            learningRateFunction = new ExponentialLearningRateFunction(iterations, initialRate: 0.999, finalRate: 0.001);
            neighbourhoodRadiusFunction = new ExponentialLearningRateFunction(iterations, initialRate: outputDiameter, finalRate: 0.001);
            neighbourhoodFunction = new GaussianNeighbourhoodFunction();

            Initialize();

            for (int i = 0; i < iterations; i++)
            {
                double learningRate = learningRateFunction.Evaluate(i);
                double neighbourhoodRadius = neighbourhoodRadiusFunction.Evaluate(i);
                
                Trace.WriteLine($"{i}: LR = {learningRate:0.000}, NR = {neighbourhoodRadius:0.000}");

                TrainSet(data, i, learningRate, neighbourhoodRadius);
            }
        }

        private void Initialize(double minWeight = 0.0, double maxWeight = 1.0)
        {
            if (maxWeight < minWeight)
                throw new ArgumentException("The maximum weight must be greater than or equal to the minimum weight.", nameof(maxWeight));

            // Input layer
            for (int n = 0; n < InputSize; n++)
                inputNeuronOutputs[n] = 0.0;

            // Output layer
            for (int n = 0; n < OutputSize; n++)
            {
                for (int w = 0; w < InputSize; w++)
                    outputNeuronWeights[n][w] = StaticRandom.Double(minWeight, maxWeight);

                outputNeuronOutputs[n] = 0.0;
            }
        }

        private void TrainSet(IDataSet data, int iteration, double learningRate, double neighbourhoodRadius)
        {
            BeforeTrainingSet?.Invoke(this, new DataSetEventArgs(data, iteration));

            foreach (var point in data.Random())
                TrainPoint(point, iteration, learningRate, neighbourhoodRadius);

            AfterTrainingSet?.Invoke(this, new DataSetEventArgs(data, iteration));
        }

        private void TrainPoint(ILabeledDataPoint point, int iteration, double learningRate, double neighbourhoodRadius)
        {
            BeforeTrainingPoint?.Invoke(this, new DataPointEventArgs(point, iteration));

            int[] winnerNeuronCoordinates = Evaluate(point.Input);
            AdaptNeuronWeights(point, winnerNeuronCoordinates, learningRate, neighbourhoodRadius);

            AfterTrainingPoint?.Invoke(this, new DataPointEventArgs(point, iteration));
        }

        private void AdaptNeuronWeights(ILabeledDataPoint point, int[] winnerNeuronCoordinates, double learningRate, double neighbourhoodRadius)
        {
            // Optimization
            if (learningRate == 0.0) return;

            foreach (int neuron in OutputNeurons)
            {
                var neuronCoordinates = NeuronIndexToCoordinates(neuron);

                double distance = Vector.Distance(neuronCoordinates, winnerNeuronCoordinates);
                double neighbourhood = neighbourhoodFunction.Evaluate(distance, neighbourhoodRadius);

                // Optimization
                if (neighbourhood == 0.0) continue;

                for (int weight = 0; weight < InputSize; weight++)
                {
                    double weightDelta = learningRate * neighbourhood * (point.Input[weight] - outputNeuronWeights[neuron][weight]);
                    outputNeuronWeights[neuron][weight] += weightDelta;
                }
            }
        }

        public int[] Evaluate(double[] input)
        {
            Require.IsNotNull(input, nameof(input));

            if (input.Length != InputSize)
                throw new ArgumentException("The input vector is not compatible with the network (i.e. the length of the input vector does not match the number of input neurons).", nameof(input));

            SetInput(input);
            EvaluateNeurons();
            var winnerNeuron = GetWinnerNeuron();
            return NeuronIndexToCoordinates(winnerNeuron);
        }

        private void SetInput(double[] input)
        {
            Array.Copy(input, inputNeuronOutputs, InputSize);
        }

        private void EvaluateNeurons()
        {
            OutputNeurons.ForEach(n => outputNeuronOutputs[n] = Vector.Distance(outputNeuronWeights[n], inputNeuronOutputs));
        }

        private int GetWinnerNeuron() => OutputNeurons.MaxBy(n => outputNeuronOutputs[n]);

        public double[] GetOutputNeuronSynapseWeights(int neuronIndex)
            => outputNeuronWeights[neuronIndex];

        public double[] GetOutputNeuronSynapseWeights(int[] neuronCoordinates)
            => GetOutputNeuronSynapseWeights(NeuronCoordinatesToIndex(neuronCoordinates));

        public Bitmap ToBitmap(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Black);

            int neuronDiameter = 3;

            for (int outputNeuronIndex = 0; outputNeuronIndex < OutputSize; ++outputNeuronIndex)
            {
                // Draw the neuron.
                double xWeight = outputNeuronWeights[outputNeuronIndex][0];
                int xCoordinate = (int)System.Math.Round(xWeight * width);
                double yWeight = outputNeuronWeights[outputNeuronIndex].Length > 1 ? outputNeuronWeights[outputNeuronIndex][1] : 0.5;
                int yCoordinate = (int)System.Math.Round(yWeight * height);

                graphics.DrawEllipse(pen, xCoordinate, yCoordinate, neuronDiameter, neuronDiameter);

                // Draw the grid.
                ICollection<int> neighbourOutputNeuronsIndices = GetNeighbourhoodNeurons(outputNeuronIndex);
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

        private IEnumerable<int> GetNeighbourhoodNeurons(int neuronIndex)
        {
            int[] neuronCoordinates = NeuronIndexToCoordinates(neuronIndex);

            var neighbours = new List<int>();
            for (int d = 0; d < outputDimension; d++)
            {
                int neuronCoordinate = neuronCoordinates[d];

                if (neuronCoordinate > 0)
                {
                    var neighbourCoordinates = (int[])neuronCoordinates.Clone();
                    neighbourCoordinates[d] = neuronCoordinate - 1;
                    neighbours.Add(NeuronCoordinatesToIndex(neighbourCoordinates));
                }

                if (neuronCoordinate < outputSizes[d] - 1)
                {
                    var neighbourCoordinates = (int[])neuronCoordinates.Clone();
                    neighbourCoordinates[d] = neuronCoordinate + 1;
                    neighbours.Add(NeuronCoordinatesToIndex(neighbourCoordinates));
                }
            }

            return neighbours;
        }

        private int[] NeuronIndexToCoordinates(int neuronIndex)
        {
            int[] coordinates = new int[outputDimension];
            for (int i = outputDimension - 1; i >= 0; i--)
            {
                int lowerNeurons = EnumerableExtensions.Range(0, i).Select(j => outputSizes[j]).Product();

                coordinates[i] = neuronIndex / lowerNeurons;
                neuronIndex = neuronIndex % lowerNeurons;
            }
            return coordinates;
        }

        private int NeuronCoordinatesToIndex(int[] neuronCoordinates)
        {
            int index = 0;
            for (int i = outputDimension - 1; i >= 0; i--)
            {
                int lowerNeurons = EnumerableExtensions.Range(0, i).Select(j => outputSizes[j]).Product();

                index += neuronCoordinates[i] * lowerNeurons;
            }
            return index;
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
