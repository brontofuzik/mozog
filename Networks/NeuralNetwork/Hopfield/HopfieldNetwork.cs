using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.Hopfield.HopfieldNetworkImps;
using Mozog.Utils.Math;

namespace NeuralNetwork.Hopfield
{
    public class HopfieldNetwork : IHopfieldNetwork
    {
        private readonly IMatrix weights;
        private readonly double[] biases;
        private readonly double[] outputs;

        private readonly ActivationFunction activation;
        private readonly Topology topology;
        private readonly Func<int, int[]> positionFactory;

        public static HopfieldNetwork Build1DNetwork(int neurons, bool sparse = false, ActivationFunction activation = null, Topology topology = null)
            => new HopfieldNetwork(new[] {neurons}, sparse, activation, topology,
                i => new[] {i});

        public static HopfieldNetwork Build2DNetwork(int rows, int cols, bool sparse = false, ActivationFunction activation = null, Topology topology = null)
            => new HopfieldNetwork(new[] { rows, cols }, sparse, activation, topology,
                i => new[] {i / cols, i % cols});

        public static HopfieldNetwork Build3DNetwork(int rows, int cols, int depth, bool sparse = false, ActivationFunction activation = null, Topology topology = null)
            => new HopfieldNetwork(new[] { rows, cols, depth }, sparse, activation, topology,
                i => new[] { i / (cols * depth), (i % (cols * depth)) / depth, (i % (cols * depth)) % depth});

        public HopfieldNetwork(int[] dimensions, bool sparse = false, ActivationFunction activation = null,
            Topology topology = null, Func<int, int[]> positionFactory = null)
        {
            Dimensions = dimensions;
            Neurons = dimensions.Product();
            weights = sparse ? (IMatrix)new SparseMatrix(Neurons, Neurons) : (IMatrix)new FullMatrix(Neurons, Neurons);
            biases = new double[Neurons];
            outputs = new double[Neurons];

            // Default activation
            double DefaultActivation(double input, double _) => System.Math.Sign(input);

            this.activation = activation ?? DefaultActivation;

            // Default topology
            IEnumerable<int[]> DefaultTopology(int[] p, HopfieldNetwork net)
                => Enumerable.Range(0, Neurons)
                .Where(sourceIndex => sourceIndex != Index(p))
                .Select(index => positionFactory(index));

            this.topology = topology ?? DefaultTopology;

            this.positionFactory = positionFactory;
        }

        // TODO Hopfield
        private int Index(int[] position) => 0;

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

            var sourceNeurons = Topology(positionFactory(neuron));
            foreach (var sn in sourceNeurons)
                SetSynapseWeight(neuron, Index(sn), initSynapseWeight);
        }

        public double GetNeuronBias(int neuron)
        {
            return biases[neuron];
        }

        public void SetNeuronBias(int neuron, double bias)
        {
            biases[neuron] = bias;
        }

        private void SetNeuronBias(int neuron, InitNeuronBias initNeuronBias)
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

        private void SetSynapseWeight(int neuron, int source, InitSynapseWeight initSynapseWeight)
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
