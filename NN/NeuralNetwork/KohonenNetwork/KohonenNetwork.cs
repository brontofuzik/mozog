using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Mozog.Utils;
using NeuralNetwork.KohonenNetwork.LearningRateFunctions;
using NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions;
using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public class KohonenNetwork
        : IKohonenNetwork
    {
        /// <summary>
        /// Initializes a new instance of the KohonenNetwork class.
        /// </summary>
        /// <param name="inputLayerNeuronCount">The number of input neurons.</param>
        /// <param name="outputLayerDimensions">The dimensions (i.e. the numbers of neurons) of the output layer.</param>
        public KohonenNetwork(int inputLayerNeuronCount, int[] outputLayerDimensions)
        {
            Require.IsPositive(inputLayerNeuronCount, nameof(inputLayerNeuronCount));
            Require.IsPositive(outputLayerDimensions.Length, nameof(outputLayerDimensions));

            foreach (int outputLayerDimension in outputLayerDimensions)
            {
                Require.IsPositive(outputLayerDimension, "outputLayerDimension");
            }

            // ---------------------------
            // Initialize the input layer.
            // ---------------------------

            // Initialize the number of neurons in the input layer (i.e. the input neurons).
            _inputLayerNeuronCount = inputLayerNeuronCount;

            // Initialize the outputs of the input neurons.
            _inputLayerNeuronOutputs = new double[inputLayerNeuronCount];

            // ----------------------------
            // Initialize the output layer.
            // ----------------------------

            // INitialize the number of neurons in the output layer (i.e. the output nerouns).
            _outputLayerDimension = outputLayerDimensions.Length;

            // Initialize the dimensions of the output layer.
            _outputLayerDimensions = outputLayerDimensions;

            // Calculate the number of output neurons.
            _outputLayerNeuronCount = 1;
            foreach (int outputLayerDimension in outputLayerDimensions)
            {
                _outputLayerNeuronCount *= outputLayerDimension;
            }

            // Initialize the weights of the output neurons.
            _outputLayerNeuronWeights = new double[_outputLayerNeuronCount][];
            for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
            {
                _outputLayerNeuronWeights[outputNeuronIndex] = new double[_inputLayerNeuronCount];
            }

            // Initialize the outputs of the output neurons.
            _outputLayerNeuronOutputs = new double[_outputLayerNeuronCount];

            // Calculate the diameter of the output layer (using Pythagoras theorem).
            double tmp = 0.0;
            for (int i = 0; i < _outputLayerDimension; ++i)
            {
                tmp += _outputLayerDimensions[i] * _outputLayerDimensions[i];
            }
            _outputLayerDiameter = Math.Sqrt(tmp);
        }

        /// <summary>
        /// Trains the Kohonen network with a training set for a number of iterations.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="trainingIterationCount">The number of training iterations.</param>
        public void Train(DataSet trainingSet, int trainingIterationCount)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));

            if (trainingSet.InputSize != _inputLayerNeuronCount)
            {
                throw new ArgumentException("The training set is not compatible with the network (i.e. the length of the input vector does not match the number of neurons in the input layer).", nameof(trainingSet));
            }

            Require.IsPositive(trainingIterationCount, nameof(trainingIterationCount));

            // The learning rate function.
            double initialLearningRate = 0.999;
            double finalLearningRate = 0.001;
            _learningRateFunction = new ExponentialLearningRateFunction(trainingIterationCount, initialLearningRate, finalLearningRate);

            // The neighbourhood neighbourhoodRadius function.
            double initialNeighbourhoodRadius = _outputLayerDiameter;
            double finalneighbourhoodRadius = 0.001;
            _neighbourhoodRadiusFunction = new ExponentialLearningRateFunction(trainingIterationCount, initialNeighbourhoodRadius, finalneighbourhoodRadius);

            // The neighbourhood function.
            _neighbourhoodFunction = new GaussianNeighbourhoodFunction();

            // Initialize the network.
            double minWeight = 0.0;
            double maxWeight = 1.0;
            Initialize(minWeight, maxWeight);

            for (int trainingIterationIndex = 0; trainingIterationIndex < trainingIterationCount; ++trainingIterationIndex)
            {
                // Calculate the learning rate.
                double learningRate = _learningRateFunction.CalculateLearningRate(trainingIterationIndex);
                
                // Calculate the neighbourhood radius.
                double neighbourhoodRadius = _neighbourhoodRadiusFunction.CalculateLearningRate(trainingIterationIndex);
                
                Trace.WriteLine($"{trainingIterationIndex}: learning rate = {learningRate:0.000}, neighbourhood radius = {neighbourhoodRadius:0.000}");

                TrainSet(trainingSet, trainingIterationIndex, learningRate, neighbourhoodRadius);
            }
        }

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>The coordinates of the output.</returns>
        public int[] Evaluate(double[] inputVector)
        {
            Require.IsNotNull(inputVector, nameof(inputVector));

            if (inputVector.Length != _inputLayerNeuronCount)
            {
                throw new ArgumentException("The input vector is not compatible with the network (i.e. the length of the input vector does not match the number of input neurons).", nameof(inputVector));
            }

            // Set the outputs of the input neurons.
            Array.Copy(inputVector, _inputLayerNeuronOutputs, _inputLayerNeuronCount);

            // Evaluate the output neurons and determine the winner output neuron.
            int winnerOutputNeuronIndex = 0;
            for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
            {
                _outputLayerNeuronOutputs[outputNeuronIndex] = Vector.Distance(_outputLayerNeuronWeights[outputNeuronIndex], _inputLayerNeuronOutputs);

                // Update the winner neuron index.
                if (_outputLayerNeuronOutputs[outputNeuronIndex] < _outputLayerNeuronOutputs[winnerOutputNeuronIndex])
                {
                    winnerOutputNeuronIndex = outputNeuronIndex;
                }
            }

            // Convert the index of the winner output neuron to its coordinates.
            int[] winnerOutputNeuronCoordinates = OutputNeuronIndexToCoordinates(winnerOutputNeuronIndex);

            return winnerOutputNeuronCoordinates;
        }

        /// <summary>
        /// Gets the weights of an output neuron's synapses.
        /// </summary>
        /// <param name="outputNeuronIndex">The index of the neuron.</param>
        /// <returns>The weights of the output neuron's synapses.</returns>
        public double[] GetOutputNeuronSynapseWeights(int outputNeuronIndex)
        {
            return _outputLayerNeuronWeights[outputNeuronIndex];
        }

        /// <summary>
        /// Gets the weights of an output neuron's synapses.
        /// </summary>
        /// <param name="outputNeuronCoordinates">The coordinates of the </param>
        /// <returns>The weights of the output neuron's synapses.</returns>
        public double[] GetOutputNeuronSynapseWeights(int[] outputNeuronCoordinates)
        {
            int outputNeuronIndex = OutputNeuronCoordinatesToIndex(outputNeuronCoordinates);
            return GetOutputNeuronSynapseWeights(outputNeuronIndex);
        }

        /// <summary>
        /// Converts the network to its bitmap representation.
        /// </summary>
        /// <param name="bitmapWidth">The width of the bitmap.</param>
        /// <param name="bitmapHeight">The height of the bitmap.</param>
        /// <returns>The bitmap representation of the network.</returns>
        public Bitmap ToBitmap(int bitmapWidth, int bitmapHeight)
        {
            Bitmap bitmap = new Bitmap(bitmapWidth, bitmapHeight);
            Graphics graphics = Graphics.FromImage(bitmap);
            Pen pen = new Pen(Color.Black);

            int neuronDiameter = 3;

            for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
            {
                // Draw the neuron.
                double xWeight = _outputLayerNeuronWeights[outputNeuronIndex][0];
                int xCoordinate = (int)Math.Round(xWeight * bitmapWidth);
                double yWeight = _outputLayerNeuronWeights[outputNeuronIndex].Length > 1 ? _outputLayerNeuronWeights[outputNeuronIndex][1] : 0.5;
                int yCoordinate = (int)Math.Round(yWeight * bitmapHeight);

                graphics.DrawEllipse(pen, xCoordinate, yCoordinate, neuronDiameter, neuronDiameter);

                // Draw the grid.
                ICollection<int> neighbourOutputNeuronsIndices = GetNeighbourOutputNeuronsIndices(outputNeuronIndex);
                foreach (int neighbourOutputNeuronIndex in neighbourOutputNeuronsIndices)
                {
                    double neighbourXWeight = _outputLayerNeuronWeights[neighbourOutputNeuronIndex][0];
                    int neighbourXCoordinate = (int)Math.Round(neighbourXWeight * bitmapWidth);
                    double neighbourYWeight = _outputLayerNeuronWeights[neighbourOutputNeuronIndex].Length > 1 ? _outputLayerNeuronWeights[neighbourOutputNeuronIndex][1] : 0.5;
                    int neighbourYCoordinate = (int)Math.Round(neighbourYWeight * bitmapHeight);

                    graphics.DrawLine(pen, xCoordinate, yCoordinate, neighbourXCoordinate, neighbourYCoordinate);
                }
            }

            return bitmap;
        }

        public int InputNeuronCount
        {
            get
            {
                return _inputLayerNeuronCount;
            }
        }

        public int OutputNeuronCount
        {
            get
            {
                return _outputLayerNeuronCount;
            }
        }

        #region Events

        /// <summary>
        /// The event invoked at the beginning of training with a training set.
        /// </summary>
        public event DataSetEventHandler BeginTrainingSetEvent;

        /// <summary>
        /// The event invoked at the end of training with a training set.
        /// </summary>
        public event DataSetEventHandler EndTrainingSetEvent;

        /// <summary>
        /// The event invoked at the beginning of training with a training pattern.
        /// </summary>
        public event TrainingPatternEventhandler BeginTrainingPatternEvent;

        /// <summary>
        /// The event invoked at the end of training with a training pattern.
        /// </summary>
        public event TrainingPatternEventhandler EndTrainingPatternEvent;

        #endregion // Events

        /// <summary>
        /// Initializes the network.
        /// </summary>
        /// <param name="maxWeight">The minimum weight.</param>
        /// <param name="minWeight">The maximum weight.</param>
        private void Initialize(double minWeight, double maxWeight)
        {
            if (maxWeight < minWeight)
            {
                throw new ArgumentException("The maximum weight must be greater than or equal to the minimum weight.", nameof(maxWeight));
            }

            // Initialize the input layer.
            for (int inputNeuronIndex = 0; inputNeuronIndex < _inputLayerNeuronCount; ++inputNeuronIndex)
            {
                _inputLayerNeuronOutputs[inputNeuronIndex] = 0.0;
            }

            // Initialize the output layer.
            for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
            {
                for (int weightIndex = 0; weightIndex < _inputLayerNeuronCount; ++weightIndex)
                {
                    _outputLayerNeuronWeights[outputNeuronIndex][weightIndex] = StaticRandom.Double(minWeight, maxWeight);
                }
                _outputLayerNeuronOutputs[outputNeuronIndex] = 0.0;
            }
        }

        ///// <summary>
        ///// Normalizes the weight vectors of the output neurons.
        ///// </summary>
        //private void NormalizeOutputLayerNeuronWeights()
        //{
        //    for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
        //    {
        //        double[] normalizedWeightVector = SupervisedTrainingPattern.NormalizeVector(_outputLayerNeuronWeights[outputNeuronIndex]);
        //        Array.Copy(normalizedWeightVector, _outputLayerNeuronWeights[outputNeuronIndex], _inputLayerNeuronCount);
        //    }
        //}

        /// <summary>
        /// Trains the networks with a training set.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="trainingIterationIndex">The index of the trianing iteration.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="neighbourhoodRadius">The neighbourhood neighbourhoodRadius.</param>
        private void TrainSet(DataSet trainingSet, int trainingIterationIndex, double learningRate, double neighbourhoodRadius)
        {
            // Invoke the beginning-training-set event.
            InvokeBeginTrainingSetEvent(trainingSet, trainingIterationIndex);

            foreach (LabeledDataPoint trainingPattern in trainingSet.RandomPoints)
            {
                TrainPattern(trainingPattern, trainingIterationIndex, learningRate, neighbourhoodRadius);
            }

            // Invoke the end-training-set event.
            InvokeEndTrainingSetEvent(trainingSet, trainingIterationIndex);
        }

        /// <summary>
        /// Trains the network with a training pattern.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <param name="trainingIterationIndex">The index of the trianing iteration.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="neighbourhoodRadius">The neighbourhood neighbourhoodRadius.</param>
        private void TrainPattern(LabeledDataPoint trainingPattern, int trainingIterationIndex, double learningRate, double neighbourhoodRadius)
        {
            // Invoke the beginning-training-pattern event.
            InvokeBeginTrainingPatternEvent(trainingPattern, trainingIterationIndex);

            // Evaluate the network.
            int[] winnerOutputNeuronCoordinates = Evaluate(trainingPattern.Input);

            // Adapt the weights of the output neurons.
            AdaptOutputLayerNeuronWeights(trainingPattern, winnerOutputNeuronCoordinates, learningRate, neighbourhoodRadius);

            // Invoke the end-training-pattern event.
            InvokeEndTrainingPatternEvent(trainingPattern, trainingIterationIndex);
        }

        /// <summary>
        /// Adapts the weights of the output neurons.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <param name="winnerOutputNeuronCoordinates">The coordinates of the winner output neuron.</param>
        /// <param name="learningRate">The learning rate.</param>
        /// <param name="neighbourhoodRadius">The neighbourhood neighbourhoodRadius.</param>
        private void AdaptOutputLayerNeuronWeights(LabeledDataPoint trainingPattern, int[] winnerOutputNeuronCoordinates, double learningRate, double neighbourhoodRadius)
        {
            // (OPTIMIZATION) No need to adapt if the learning rate is zero.
            if (learningRate == 0.0)
            {
                return;
            }

            for (int outputNeuronIndex = 0; outputNeuronIndex < _outputLayerNeuronCount; ++outputNeuronIndex)
            {
                // Get the coordinates of the output neuron.
                int[] outputNeuronCoordinates = OutputNeuronIndexToCoordinates(outputNeuronIndex);

                // Calculate the distance between the output neuron and the winner output neuron.
                double distanceBetweenOutputNeurons = Vector.Distance(outputNeuronCoordinates, winnerOutputNeuronCoordinates);

                // Calculate the neighbourhood.
                double neighbourhood = _neighbourhoodFunction.CalculateNeighbourhood(distanceBetweenOutputNeurons, neighbourhoodRadius);
                
                // (OPTIMIZATION) No need to adapt if the neighbourhood is zero.
                if (neighbourhood == 0.0)
                {
                    continue;
                }

                for (int weightIndex = 0; weightIndex < _inputLayerNeuronCount; ++weightIndex)
                {
                    double weightDelta = learningRate * neighbourhood * (trainingPattern.Input[weightIndex] - _outputLayerNeuronWeights[outputNeuronIndex][weightIndex]);
                    _outputLayerNeuronWeights[outputNeuronIndex][weightIndex] += weightDelta;
                }
            }
        }

        /// <summary>
        /// Converts the index of an output neuron to its coordinates.
        /// </summary>
        /// <param name="outputNeuronIndex">The index of the output neuron.</param>
        /// <returns>The coordiantes of the output neuron.</returns>
        private int[] OutputNeuronIndexToCoordinates(int outputNeuronIndex)
        {
            int[] outputNeuronCoordinates = new int[_outputLayerDimension];
            for (int i = _outputLayerDimension - 1; i >= 0; --i)
            {
                int lowerDimensionsNeuronCount = 1;
                for (int j = i - 1; j >= 0; --j)
                {
                    lowerDimensionsNeuronCount *= _outputLayerDimensions[j];
                }
                outputNeuronCoordinates[i] = outputNeuronIndex / lowerDimensionsNeuronCount;
                outputNeuronIndex = outputNeuronIndex % lowerDimensionsNeuronCount;
            }
            return outputNeuronCoordinates;
        }

        /// <summary>
        /// Converts the coordiantes of an output neuron to its index.
        /// </summary>
        /// <param name="outputNeuronCoordinates">The coordinates of the output neuron.</param>
        /// <returns>The index of the output neuron.</returns>
        private int OutputNeuronCoordinatesToIndex(int[] outputNeuronCoordinates)
        {
            int outputNeuronIndex = 0;
            for (int i = _outputLayerDimension - 1; i >= 0; --i)
            {
                int lowerDimensionsNeuronCount = 1;
                for (int j = i - 1; j >= 0; --j)
                {
                    lowerDimensionsNeuronCount *= _outputLayerDimensions[j];
                }
                outputNeuronIndex += outputNeuronCoordinates[i] * lowerDimensionsNeuronCount;
            }
            return outputNeuronIndex;
        }

        private ICollection<int> GetNeighbourOutputNeuronsIndices(int outputNeuronIndex)
        {
            int[] outputNeuronCoordinates = OutputNeuronIndexToCoordinates(outputNeuronIndex);

            ICollection<int> neighbourOutputNeuronsIndices = new HashSet<int>();
            for (int d = 0; d < _outputLayerDimension; ++d)
            {
                int outputNeuronCoordinate = outputNeuronCoordinates[d];

                if (outputNeuronCoordinate > 0)
                {
                    int[] neighbourOutputNeuronCoordinates = new int[_outputLayerDimension];
                    Array.Copy(outputNeuronCoordinates, neighbourOutputNeuronCoordinates, _outputLayerDimension);
                    neighbourOutputNeuronCoordinates[d] = outputNeuronCoordinate - 1;

                    int neighbourOutputNeuronIndex = OutputNeuronCoordinatesToIndex(neighbourOutputNeuronCoordinates);
                    neighbourOutputNeuronsIndices.Add(neighbourOutputNeuronIndex);
                }

                if (outputNeuronCoordinate < _outputLayerDimensions[d] - 1)
                {
                    int[] neighbourOutputNeuronCoordinates = new int[_outputLayerDimension];
                    Array.Copy(outputNeuronCoordinates, neighbourOutputNeuronCoordinates, _outputLayerDimension);
                    neighbourOutputNeuronCoordinates[d] = outputNeuronCoordinate + 1;

                    int neighbourOutputNeuronIndex = OutputNeuronCoordinatesToIndex(neighbourOutputNeuronCoordinates);
                    neighbourOutputNeuronsIndices.Add(neighbourOutputNeuronIndex);
                }
            }

            return neighbourOutputNeuronsIndices;
        }

        #region Event invokers

        /// <summary>
        /// Invokes the begin-training-set event.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="trainingIterationIndex">The index of the trianing iteration.</param>
        private void InvokeBeginTrainingSetEvent(DataSet trainingSet, int trainingIterationIndex)
        {
            if (BeginTrainingSetEvent != null)
            {
                BeginTrainingSetEvent(this, new DataSetEventArgs(trainingSet, trainingIterationIndex));
            }
        }

        /// <summary>
        /// Invokes the end-training-set event.
        /// </summary>
        /// <param name="trianingSet">The training set.</param>
        /// <param name="trainingIterationIndex">The index of the trianing iteration.</param>
        private void InvokeEndTrainingSetEvent(DataSet trianingSet, int trainingIterationIndex)
        {
            if (EndTrainingSetEvent != null)
            {
                EndTrainingSetEvent(this, new DataSetEventArgs(trianingSet, trainingIterationIndex));
            }
        }

        /// <summary>
        /// Invokes the begin-training-pattern event.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <param name="trainingIterationIndex">The index of the training iteration.</param>
        private void InvokeBeginTrainingPatternEvent(LabeledDataPoint trainingPattern, int trainingIterationIndex)
        {
            if (BeginTrainingPatternEvent != null)
            {
                BeginTrainingPatternEvent(this, new DataPointEventArgs(trainingPattern, trainingIterationIndex));
            }
        }

        /// <summary>
        /// Invokes the end-training-pattern event.
        /// </summary>
        /// <param name="trainingPattern">The training pattern.</param>
        /// <param name="trainingIterationIndex">The index of the training iteration.</param>
        private void InvokeEndTrainingPatternEvent(LabeledDataPoint trainingPattern, int trainingIterationIndex)
        {
            if (EndTrainingPatternEvent != null)
            {
                EndTrainingPatternEvent(this, new DataPointEventArgs(trainingPattern, trainingIterationIndex));
            }
        }

        #endregion Event invokers

        #region Input layer

        /// <summary>
        /// The number of neurons in the input layer (input neurons).
        /// </summary>
        private int _inputLayerNeuronCount;

        /// <summary>
        /// The outputs of the input neurons.
        /// </summary>
        private double[] _inputLayerNeuronOutputs;

        #endregion // Input layer

        #region Output layer

        /// <summary>
        /// The dimension of the output layer.
        /// </summary>
        private int _outputLayerDimension;

        /// <summary>
        /// The dimensions of the output neuron.
        /// </summary>
        private int[] _outputLayerDimensions;

        /// <summary>
        /// The number of neurons in the output layer (output neurons).
        /// </summary>
        private int _outputLayerNeuronCount;

        /// <summary>
        /// The weights of the output neurons.
        /// </summary>
        private double[][] _outputLayerNeuronWeights;

        /// <summary>
        /// The outputs of the output neurons.
        /// </summary>
        private double[] _outputLayerNeuronOutputs;

        /// <summary>
        /// The diameter of the output layer.
        /// </summary>
        private double _outputLayerDiameter;

        #endregion // Output layer

        #region Training

        /// <summary>
        /// The learning rate function.
        /// </summary>
        private ILearningRateFunction _learningRateFunction;

        /// <summary>
        /// The neighbourhood neighbourhoodRadius function.
        /// </summary>
        private ILearningRateFunction _neighbourhoodRadiusFunction;

        /// <summary>
        /// The neighbourhood function.
        /// </summary>
        private INeighbourhoodFunction _neighbourhoodFunction;

        #endregion // Training
    }
}
