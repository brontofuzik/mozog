using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.HopfieldNet.HopfieldNetworkImps;
using Mozog.Utils.Math;

namespace NeuralNetwork.HopfieldNet
{
    public static class HopfieldNetwork
    {
        public static HopfieldNetwork<Position1D> Build1DNetwork(int neurons, bool sparse = false, ActivationFunction activation = null, Topology<Position1D> topology = null)
        {
            return new HopfieldNetwork<Position1D>(neurons, sparse, activation, topology, i => new Position1D(i));
        }

        public static HopfieldNetwork<Position2D> Build2DNetwork(int rows, int cols, bool sparse = false, ActivationFunction activation = null, Topology<Position2D> topology = null)
        {
            return new HopfieldNetwork<Position2D>(rows, cols, sparse, activation, topology, i => new Position2D(i / rows, i % rows, cols));
        }
    }

    public class HopfieldNetwork<T> : IHopfieldNetwork
        where T : IPosition
    {
        private IMatrix weights;
        private double[] biases;
        private double[] outputs;

        private ActivationFunction activation;
        private Topology<T> topology;
        private Func<int, T> positionFactory;

        // 1D
        internal HopfieldNetwork(int neurons, bool sparse = false, ActivationFunction activation = null,
            Topology<T> topology = null, Func<int, T> positionFactory = null)
        {
            Neurons = neurons;
            weights = sparse ? (IMatrix)new SparseMatrix(neurons, neurons) : (IMatrix)new FullMatrix(neurons, neurons);
            biases = new double[neurons];
            outputs = new double[neurons];

            // Default activation
            ActivationFunction defaultActivation = ((input, _) => System.Math.Sign(input));
            this.activation = activation ?? defaultActivation;

            // Default topology
            Topology<T> defaultTopology = (p, net) => Enumerable.Range(0, Neurons)
                .Where(sourceIndex => sourceIndex != p.Index)
                .Select(index => positionFactory(index));
            this.topology = topology ?? defaultTopology;

            this.positionFactory = positionFactory;
        }

        // 2D
        internal HopfieldNetwork(int rows, int cols, bool sparse = false, ActivationFunction activationFunction = null,
            Topology<T> topology = null, Func<int, T> positionFactory = null)
            : this(rows * cols, sparse, activationFunction, topology, positionFactory)
        {
            Rows = rows;
            Cols = cols;
        }

        public IEnumerable<T> Topology(T neuron) => topology(neuron, this);

        public int Neurons { get; }

        public int? Rows { get; }

        public int? Cols { get; }

        public int Synapses => (weights.Size - Neurons) / 2;

        public double Energy
        {
            get
            {
                double energy = 0.0;

                // Syanpse energy
                for (int neuron = 0; neuron < Neurons; neuron++)
                    for (int source = neuron + 1; source < Neurons; source++)
                        energy -= weights[neuron, source] * outputs[neuron] * outputs[source];

                // Neuron energy
                for (int neuron = 0; neuron < Neurons; neuron++)
                    energy -= biases[neuron] * outputs[neuron];

                return energy;
            }
        }

        #region Initialization

        public void Initialize(InitNeuronBias<T> initNeuronBias, InitSynapseWeight<T> initSynapseWeight)
        {
            for (int neuronIndex = 0; neuronIndex < Neurons; neuronIndex++)
                InitializeNeuron(neuronIndex, initNeuronBias, initSynapseWeight);
        }

        private void InitializeNeuron(int neuron, InitNeuronBias<T> initNeuronBias, InitSynapseWeight<T> initSynapseWeight)
        {
            SetNeuronBias(neuron, initNeuronBias);

            var sourceNeurons = Topology(positionFactory(neuron));
            foreach (var source in sourceNeurons)
                SetSynapseWeight(neuron, source.Index, initSynapseWeight);
        }

        public double GetNeuronBias(int neuron)
        {
            return biases[neuron];
        }

        public void SetNeuronBias(int neuron, double bias)
        {
            biases[neuron] = bias;
        }

        private void SetNeuronBias(int neuron, InitNeuronBias<T> initNeuronBias)
        {
            SetNeuronBias(neuron, initNeuronBias(positionFactory(neuron), this));
        }

        public double GetSynapseWeight(int neuron, int source)
        {
            return weights[neuron, source];
        }

        public void SetSynapseWeight(int neuron, int source, double weight)
        {
            weights[neuron, source] = weights[source, neuron] = weight;
        }

        private void SetSynapseWeight(int neuron, int source, InitSynapseWeight<T> initSynapseWeight)
        {
            SetSynapseWeight(neuron, source, initSynapseWeight(positionFactory(neuron), positionFactory(source), this));
        }

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
                Evaluate(i / (double)iterations);
            }

            return (double[])outputs.Clone();
        }

        private void Evaluate(double progress)
        {
            foreach (int n in RandomNeurons)
                EvaluateNeuron(n, progress);
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
