using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Common;
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

        //private readonly int[] optimization;

        private readonly ILearningRateFunction learningRateFunction = new ExponentialDecayFunction(initialRate: 0.99, finalRate: 0.01);
        private readonly ILearningRateFunction neighbourhoodSizeFunction;
        private readonly INeighbourhoodFunction neighbourhoodFunction = new GaussianNeighbourhoodFunction();

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

            outputNeuronWeights = OutputSize.Times(() => new double[InputSize]).ToArray();
            outputNeuronOutputs = new double[OutputSize];

            // Pythagoras theorem
            var neighbourhoodSize = System.Math.Sqrt(outputSizes.Sum(s => s * s));
            neighbourhoodSizeFunction = new ExponentialDecayFunction(initialRate: neighbourhoodSize, finalRate: 0.01);
        }

        #region Events

        public event EventHandler<TrainingEventArgs> TrainingIteration;

        public event EventHandler<TrainingEventArgs> IterationTrained;

        #endregion // Events

        public int InputSize { get; }

        public int OutputSize { get; }

        private IEnumerable<int> InputNeurons => Enumerable.Range(0, InputSize);

        private IEnumerable<int> OutputNeurons => Enumerable.Range(0, OutputSize);

        #region Training

        public void Train(DataSet data, int iterations)
        {
            Require.IsNotNull(data, nameof(data));

            if (data.InputSize != InputSize)
                throw new ArgumentException("The training set is not compatible with the network (i.e. the length of the input vector does not match the number of neurons in the input layer).", nameof(data));

            Require.IsPositive(iterations, nameof(iterations));

            Initialize();
            iterations.Times(i => TrainSet(data, i));
        }

        // Initialize before training
        private void Initialize(double minWeight = 0.0, double maxWeight = 1.0)
        {
            if (maxWeight < minWeight)
                throw new ArgumentException("The maximum weight must be greater than or equal to the minimum weight.", nameof(maxWeight));

            foreach (int n in OutputNeurons)
            {
                outputNeuronWeights[n] = StaticRandom.DoubleArray(InputSize, minWeight, maxWeight);
                outputNeuronOutputs[n] = 0.0;
            }
        }

        private void TrainSet(IDataSet data, int iteration)
        {
            TrainingIteration?.Invoke(this, new TrainingEventArgs(iteration));

            double learningRate = learningRateFunction.Evaluate(iteration);
            double neighbourhoodRadius = neighbourhoodSizeFunction.Evaluate(iteration);

            Trace.WriteLine($"{iteration}: LR = {learningRate:F2}, NR = {neighbourhoodRadius:F2}");

            foreach (var point in data.Random())
                TrainPoint(point, iteration, learningRate, neighbourhoodRadius);

            IterationTrained?.Invoke(this, new TrainingEventArgs(iteration));
        }

        private void TrainPoint(IDataPoint point, int iteration, double learningRate, double neighbourhoodRadius)
        {
            int[] winnerNeuron = Evaluate(point.Input);
            AdaptNeuronWeights(point, winnerNeuron, learningRate, neighbourhoodRadius);
        }

        private void AdaptNeuronWeights(IDataPoint point, int[] winnerNeuron, double learningRate, double neighbourhoodRadius)
        {
            int winnerNeuronIndex = PositionToIndex(winnerNeuron);

            foreach (int neuronIndex in OutputNeurons)
            {
                if (neuronIndex == winnerNeuronIndex) continue;

                var neuron = IndexToPosition(neuronIndex);

                double rawDistance = Vector.Distance(neuron, winnerNeuron);
                double distance = neighbourhoodFunction.Evaluate(rawDistance, neighbourhoodRadius);

                for (int weight = 0; weight < InputSize; weight++)
                {
                    double weightDelta = learningRate * distance * (point[weight] - outputNeuronWeights[neuronIndex][weight]);
                    outputNeuronWeights[neuronIndex][weight] += weightDelta;
                }
            }
        }

        #endregion // Training

        #region Evaluation

        public int[] Evaluate(double[] input)
        {
            Require.IsNotNull(input, nameof(input));

            if (input.Length != InputSize)
                throw new ArgumentException("The input vector is not compatible with the network (i.e. the length of the input vector does not match the number of input neurons).", nameof(input));

            SetInput(input);
            Evaluate();
            var winnerNeuron = GetWinnerNeuron();
            return IndexToPosition(winnerNeuron);
        }

        private void SetInput(double[] input)
            => Array.Copy(input, inputNeuronOutputs, InputSize);

        // Evaluates all neurons
        private void Evaluate()
        {
            OutputNeurons.ForEach(n => outputNeuronOutputs[n] = Vector.Distance(outputNeuronWeights[n], inputNeuronOutputs));
        }

        private int GetWinnerNeuron() => OutputNeurons.MinBy(n => outputNeuronOutputs[n]);

        #endregion // Evaluation

        public double[] GetOutputNeuronSynapseWeights(int neuron)
            => outputNeuronWeights[neuron];

        public double[] GetOutputNeuronSynapseWeights(int[] neuron)
            => GetOutputNeuronSynapseWeights(PositionToIndex(neuron));

        private int[] IndexToPosition(int index)
        {
            var position = new int[outputDimension];
            for (int i = outputDimension - 1; i >= 0; i--)
            {
                position[i] = index % outputSizes[i];
                index /= outputSizes[i];
            }
            return position;
        }

        private int PositionToIndex(int[] position)
        {
            var index = 0;
            for (int i = 0; i < outputDimension; i++)
            {
                index = index * outputSizes[i] + position[i];
            }
            return index;
        }

        #region Bitmap

        public Bitmap ToBitmap(int width, int height)
        {
            const int diameter = 10;

            (double x, double y) GetOutputNeuronWeights(int neuron)
                => (outputNeuronWeights[neuron][0], outputNeuronWeights[neuron].Length > 1 ? outputNeuronWeights[neuron][1] : 0.5);

            var bitmap = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bitmap);

            // White background
            using (var brush = new SolidBrush(Color.White))
                graphics.FillRectangle(brush, 0, 0, width, height);

            var pen = new Pen(Color.Black);

            foreach (int neuron in OutputNeurons)
            {
                // Draw the neuron.
                var (xw, yw) = GetOutputNeuronWeights(neuron);
                int x = (int)System.Math.Round(xw * width);
                int y = (int)System.Math.Round(yw * height);

                graphics.DrawEllipse(pen, x - diameter/2, y - diameter/2, diameter, diameter);

                // Draw the grid.
                var neighbours = GetNeighbourhoodNeurons(neuron);
                foreach (int neighbour in neighbours)
                {
                    (xw, yw) = GetOutputNeuronWeights(neighbour);
                    int xn = (int)System.Math.Round(xw * width);
                    int yn = (int)System.Math.Round(yw * height);

                    graphics.DrawLine(pen, x, y, xn, yn);
                }
            }

            pen.Dispose();
            graphics.Dispose();

            return bitmap;
        }

        private IEnumerable<int> GetNeighbourhoodNeurons(int neuronIndex)
        {
            int[] neuronCoordinates = IndexToPosition(neuronIndex);

            var neighbours = new List<int>();
            for (int d = 0; d < outputDimension; d++)
            {
                int neuronCoordinate = neuronCoordinates[d];

                if (neuronCoordinate > 0)
                {
                    var neighbourCoordinates = (int[])neuronCoordinates.Clone();
                    neighbourCoordinates[d] = neuronCoordinate - 1;
                    neighbours.Add(PositionToIndex(neighbourCoordinates));
                }

                if (neuronCoordinate < outputSizes[d] - 1)
                {
                    var neighbourCoordinates = (int[])neuronCoordinates.Clone();
                    neighbourCoordinates[d] = neuronCoordinate + 1;
                    neighbours.Add(PositionToIndex(neighbourCoordinates));
                }
            }

            return neighbours;
        }

        #endregion Bitmap
    }
}
