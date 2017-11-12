using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.HopfieldNet.HopfieldNetworkImps;

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
        private IHopfieldNetworkImpl networkImpl;
        private Topology<T> topology;

        private Func<int, T> positionFactory;

        // 1D
        internal HopfieldNetwork(int neuronCount, bool sparse = false, ActivationFunction activation = null,
            Topology<T> topology = null, Func<int, T> positionFactory = null)
        {
            // Default activation
            ActivationFunction defaultActivation = ((input, _) => Math.Sign(input));
            activation = activation ?? defaultActivation;

            this.positionFactory = positionFactory;

            // Default topology
            Topology<T> defaultTopology = (p, net) => Enumerable.Range(0, Neurons)
                .Where(sourceIndex => sourceIndex != p.Index)
                .Select(index => positionFactory(index));
            this.topology = topology ?? defaultTopology;

            networkImpl = sparse
                ? (IHopfieldNetworkImpl)new SparseHopfieldNetworkImpl(neuronCount, activation)
                : (IHopfieldNetworkImpl)new FullHopfieldNetworkImpl(neuronCount, activation);
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

        public int Neurons => networkImpl.Neurons;

        public int? Rows { get; }

        public int? Cols { get; }

        public int Synapses => networkImpl.Synapses;

        public double Energy => networkImpl.Energy;

        #region Initialization

        //public void SetTopology(Topology<int> topology)
        //{
        //    topology_Int = topology;
        //    topology_Pos = (p, net) => topology(NeuronPositionToIndex(p), net).Select(i => NeuronIndexToPosition(i));
        //}

        //public void SetTopology(Topology<Position> topology)
        //{
        //    topology_Pos = topology;
        //    topology_Int = (i, net) => topology(NeuronIndexToPosition(i), net).Select(p => NeuronPositionToIndex(p));
        //}

        // Index-based
        public void Initialize(InitNeuronBias<T> initNeuronBias, InitSynapseWeight<T> initSynapseWeight)
        {
            for (int neuronIndex = 0; neuronIndex < Neurons; neuronIndex++)
                InitializeNeuron(neuronIndex, initNeuronBias, initSynapseWeight);
        }

        //// Position-based
        //public void Initialize(InitNeuronBias<Position> initNeuronBias, InitSynapseWeight<Position> initSynapseWeight)
        //{
        //    InitNeuronBias<int> initNeuronBiasByIndex = (neuronIndex, net)
        //        => initNeuronBias(NeuronIndexToPosition(neuronIndex), net);

        //    InitSynapseWeight<int> initSynapseWeightByIndex = (neuronIndex, sourceNeuronIndex, net)
        //        => initSynapseWeight(NeuronIndexToPosition(neuronIndex), NeuronIndexToPosition(sourceNeuronIndex), net);

        //    Initialize(initNeuronBiasByIndex, initSynapseWeightByIndex);
        //}

        //private Position NeuronIndexToPosition(int index) => new Position(index / Cols.Value, index % Cols.Value);

        //private int NeuronPositionToIndex(Position p) => p.Row * Cols.Value + p.Col;

        private void InitializeNeuron(int neuronIndex, InitNeuronBias<T> initNeuronBias, InitSynapseWeight<T> initSynapseWeight)
        {
            SetNeuronBias(neuronIndex, initNeuronBias);
            InitializeSynapses(neuronIndex, initSynapseWeight);
        }

        private void InitializeSynapses(int neuronIndex, InitSynapseWeight<T> initSynapseWeight)
        {
            var sourceNeurons = Topology(positionFactory(neuronIndex));
            foreach (var sourceNeuronIndex in sourceNeurons)
                SetSynapseWeight(neuronIndex, sourceNeuronIndex.Index, initSynapseWeight);
        }

        public double GetNeuronBias(int neuronIndex)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, Neurons - 1);

            return networkImpl.GetNeuronBias(neuronIndex);
        }

        public void SetNeuronBias(int neuronIndex, double bias)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, Neurons - 1);

            networkImpl.SetNeuronBias(neuronIndex, bias);
        }

        private void SetNeuronBias(int neuronIndex, InitNeuronBias<T> initNeuronBias)
        {
            SetNeuronBias(neuronIndex, initNeuronBias(positionFactory(neuronIndex), this));
        }

        public double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, Neurons - 1);
            Require.IsWithinRange(sourceNeuronIndex, nameof(sourceNeuronIndex), 0, Neurons - 1);

            return networkImpl.GetSynapseWeight(neuronIndex, sourceNeuronIndex);
        }

        public void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double weight)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, Neurons - 1);
            Require.IsWithinRange(sourceNeuronIndex, nameof(sourceNeuronIndex), 0, Neurons - 1);

            networkImpl.SetSynapseWeight(neuronIndex, sourceNeuronIndex, weight);
        }

        private void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, InitSynapseWeight<T> initSynapseWeight)
        {
            SetSynapseWeight(neuronIndex, sourceNeuronIndex, initSynapseWeight(positionFactory(neuronIndex), positionFactory(sourceNeuronIndex), this));
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

        private void TrainNeuron(int neuronIndex, DataSet data)
        {
            SetNeuronBias(neuronIndex, 0.0);

            for (int sourceNeuronIndex = 0; sourceNeuronIndex < Neurons; sourceNeuronIndex++)
            {
                if (sourceNeuronIndex == neuronIndex) continue;
                TrainSynapse(neuronIndex, sourceNeuronIndex, data);
            }
        }

        private void TrainSynapse(int neuronIndex, int sourceNeuronIndex, DataSet data)
        {
            double weight = data.Select(p => p.Input[neuronIndex] * p.Input[sourceNeuronIndex]).Sum();
            SetSynapseWeight(neuronIndex, sourceNeuronIndex, weight);
        }

        #endregion // Training

        public double[] Evaluate(double[] input, int iterations)
        {
            if (input.Length != Neurons)
                throw new ArgumentException("The pattern to recall is not compatible with the Hopfield network.", nameof(input));

            if (iterations < 0)
                throw new ArgumentException("The number of iterations cannot be negative.", nameof(iterations));

            networkImpl.SetNetworkInput(input);

            for (int i = 0; i < iterations; ++i)
            {
                Trace.WriteLine($"{i}: Energy = {networkImpl.Energy:0.000}");
                networkImpl.Evaluate(i / (double)iterations);
            }

            return networkImpl.GetNetworkOutput();
        }

        // TODO Hopfield
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int targetNeuronIndex = 0; targetNeuronIndex < Neurons; ++targetNeuronIndex)
            {
                sb.Append("[");
                for (int sourceNeuronIndex = 0; sourceNeuronIndex < Neurons; ++sourceNeuronIndex)
                {
                    if (sourceNeuronIndex == targetNeuronIndex)
                    {
                        double neuronBias = networkImpl.GetNeuronBias(targetNeuronIndex);
                        sb.Append(neuronBias + " ");
                    }
                    else // (neuronIndex != sourceNeuronIndex)
                    {
                        double synapseWeight = networkImpl.GetSynapseWeight(sourceNeuronIndex, targetNeuronIndex);
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

    public struct Position
    {
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public int Row { get; }

        public int Col { get; }
    }
}
