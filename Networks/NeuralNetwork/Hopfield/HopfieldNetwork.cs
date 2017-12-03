using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.Hopfield.HopfieldNetworkImps;
using Mozog.Utils.Math;
using NeuralNetwork.Common;

namespace NeuralNetwork.Hopfield
{
    public class HopfieldNetwork : IHopfieldNetwork
    {
        private readonly IMatrix weights;
        private readonly double[] biases;
        private readonly double[] outputs;

        private readonly ActivationFunction activation;
        private readonly Topology topology;

        public HopfieldNetwork(int[] dimensions, bool sparse = false, ActivationFunction activation = null, Topology topology = null)
        {
            Dimensions = dimensions;
            Neurons = dimensions.Product();
            weights = sparse ? (IMatrix)new SparseMatrix(Neurons, Neurons) : (IMatrix)new FullMatrix(Neurons, Neurons);
            biases = new double[Neurons];
            outputs = new double[Neurons];

            // Default activation: signum function
            double DefaultActivation(double input, double _) => System.Math.Sign(input);

            this.activation = activation ?? DefaultActivation;

            // Default topology: all neurons
            IEnumerable<int[]> DefaultTopology(int[] neuron, HopfieldNetwork net)
            {
                var index = PositionToIndex(neuron);
                return NeuronsEnumerable.Where(source => source != index).Select(source => IndexToPosition(source));
            }

            this.topology = topology ?? DefaultTopology;
        }

        #region Events

        public event EventHandler<TrainingEventArgs> EvaluatingIteration;

        public event EventHandler<TrainingEventArgs> IterationEvaluated;

        public event EventHandler<InitializationEventArgs> NeuronInitialized;

        #endregion // Events

        public IEnumerable<int[]> Topology(int[] neuron) => topology(neuron, this);

        public int[] Dimensions { get; }

        public int Neurons { get; }

        private IEnumerable<int> NeuronsEnumerable => Enumerable.Range(0, Neurons);

        public int Synapses => (weights.Size - Neurons) / 2;

        public double Energy
        {
            get
            {
                double energy = 0.0;

                // Syanpse energy
                for (int neuron = 0; neuron < Neurons; neuron++)
                    foreach (int source in weights.GetSourceNeurons(neuron))
                        if (source > neuron)
                            energy -= weights[neuron, source] * outputs[neuron] * outputs[source];

                // Neuron energy
                for (int neuron = 0; neuron < Neurons; neuron++)
                    energy -= biases[neuron] * outputs[neuron];

                return energy;
            }
        }

        #region Initialization

        public void Initialize(InitNeuronBias initNeuronBias, InitSynapseWeight initSynapseWeight)
        {
            foreach (int n in NeuronsEnumerable)
                InitializeNeuron(n, initNeuronBias, initSynapseWeight);
        }

        private void InitializeNeuron(int neuron, InitNeuronBias initNeuronBias, InitSynapseWeight initSynapseWeight)
        {
            SetNeuronBias(neuron, initNeuronBias);

            var sourceNeurons = Topology(IndexToPosition(neuron));
            foreach (var sn in sourceNeurons)
                SetSynapseWeight(neuron, PositionToIndex(sn), initSynapseWeight);
        }

        public double GetNeuronBias(int neuron) => biases[neuron];

        public double GetNeuronBias(int[] neuron)
            => GetNeuronBias(PositionToIndex(neuron));

        public void SetNeuronBias(int neuron, double bias)
        {
            biases[neuron] = bias;

            NeuronInitialized?.Invoke(this, new InitializationEventArgs(neuron));
        }

        public void SetNeuronBias(int[] neuron, double bias)
            => SetNeuronBias(PositionToIndex(neuron), bias);

        private void SetNeuronBias(int neuron, InitNeuronBias initNeuronBias)
            => SetNeuronBias(neuron, initNeuronBias(IndexToPosition(neuron), this));

        public double GetSynapseWeight(int neuron, int source) => weights[neuron, source];

        public double GetSynapseWeight(int[] neuron, int[] source)
            => GetSynapseWeight(PositionToIndex(neuron), PositionToIndex(source));

        public void SetSynapseWeight(int neuron, int source, double weight)
            => weights[neuron, source] = weights[source, neuron] = weight;

        public void SetSynapseWeight(int[] neuron, int[] source, double weight)
            => SetSynapseWeight(PositionToIndex(neuron), PositionToIndex(source), weight);

        private void SetSynapseWeight(int neuron, int source, InitSynapseWeight initSynapseWeight)
            => SetSynapseWeight(neuron, source, initSynapseWeight(IndexToPosition(neuron), IndexToPosition(source), this));

        #endregion // Initialization

        #region Training

        public void Train(DataSet data)
        {
            Require.IsNotNull(data, nameof(data));

            if (data.InputSize != Neurons)
                throw new ArgumentException("The training set is not compatible with the network.", nameof(data));

            for (int neuronIndex = 0; neuronIndex < Neurons; ++neuronIndex)
                TrainNeuron(neuronIndex, data);
        }

        private void TrainNeuron(int neuron, DataSet data)
        {
            SetNeuronBias(neuron, 0.0);

            for (int sourceNeuronIndex = 0; sourceNeuronIndex < Neurons; sourceNeuronIndex++)
            {
                if (sourceNeuronIndex == neuron) continue;
                TrainSynapse(neuron, sourceNeuronIndex, data);
            }
        }

        private void TrainSynapse(int neuron, int source, DataSet data)
        {
            double weight = data.Select(p => p.Input[neuron] * p.Input[source]).Sum();
            SetSynapseWeight(neuron, source, weight);
        }

        #endregion // Training

        #region Evaluation

        public double[] Evaluate(double[] input, int iterations)
        {
            if (input.Length != Neurons)
                throw new ArgumentException("The pattern to recall is not compatible with the Hopfield network.", nameof(input));

            if (iterations < 0)
                throw new ArgumentException("The number of iterations cannot be negative.", nameof(iterations));

            Array.Copy(input, outputs, input.Length);

            for (int i = 0; i < iterations; i++)
            {
                Trace.WriteLine($"{i}: Energy = {Energy:0.000}");
                Evaluate(i, iterations);
            }

            return (double[])outputs.Clone();
        }

        private void Evaluate(int interation, int iterations)
        {
            EvaluatingIteration?.Invoke(this, new TrainingEventArgs(interation));

            double progress = interation / (double) iterations;
            foreach (int n in RandomNeurons)
                EvaluateNeuron(n, progress);

            EvaluatingIteration?.Invoke(this, new TrainingEventArgs(interation));
        }

        private void EvaluateNeuron(int neuron, double progress)
        {
            double input = weights.GetSourceNeurons(neuron).Sum(source => weights[neuron, source] * outputs[source])
                + biases[neuron];
            outputs[neuron] = activation(input, progress);
        }

        private int[] RandomNeurons
        {
            get
            {
                int[] randomNeurons = Enumerable.Range(0, Neurons).ToArray();
                StaticRandom.Shuffle(randomNeurons);
                return randomNeurons;
            }
        }

        #endregion // Evaluation

        private int[] IndexToPosition(int index)
        {
            var position = new int[Dimensions.Length];
            for (int i = Dimensions.Length - 1; i >= 0; i--)
            {
                position[i] = index % Dimensions[i];
                index /= Dimensions[i];
            }
            return position;
        }

        private int PositionToIndex(int[] position)
        {
            var index = 0;
            for (int i = 0; i < Dimensions.Length; i++)
            {
                index = index * Dimensions[i] + position[i];
            }
            return index;
        }

        // TODO Hopfield
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int neuron = 0; neuron < Neurons; neuron++)
            {
                sb.Append("[");
                for (int source = 0; source < Neurons; source++)
                {
                    if (source == neuron)
                    {
                        double neuronBias = GetNeuronBias(neuron);
                        sb.Append(neuronBias + " ");
                    }
                    else // (neuronIndex != sourceNeuronIndex)
                    {
                        double synapseWeight = GetSynapseWeight(source, neuron);
                        sb.Append(synapseWeight + " ");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");

                sb.Append("\n");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }
    }
}
