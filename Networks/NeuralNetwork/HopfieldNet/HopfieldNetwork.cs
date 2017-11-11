using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Mozog.Utils;
using NeuralNetwork.Data;
using NeuralNetwork.HopfieldNet.HopfieldNetworkImps;

namespace NeuralNetwork.HopfieldNet
{
    public class HopfieldNetwork : IHopfieldNetwork
    {
        private static ActivationFunction DefaultActivation = (double input, double evaluationProgress) => Math.Sign(input);

        private IHopfieldNetworkImpl networkImpl;

        // 1D
        public HopfieldNetwork(int neuronCount, bool sparse = false, ActivationFunction activationFunction = null)
        {
            activationFunction = activationFunction ?? DefaultActivation;

            networkImpl = sparse
                ? (IHopfieldNetworkImpl)new SparseHopfieldNetworkImpl(neuronCount, activationFunction)
                : (IHopfieldNetworkImpl)new FullHopfieldNetworkImpl(neuronCount, activationFunction);
        }

        // 2D
        public HopfieldNetwork(int rows, int cols, bool sparse = false, ActivationFunction activationFunction = null)
            : this(rows * cols, sparse, activationFunction)
        {
            Rows = rows;
            Cols = cols;
        }

        public int Neurons => networkImpl.Neurons;

        public int? Rows { get; }

        public int? Cols { get; }

        public int Synapses => networkImpl.Synapses;

        public double Energy => networkImpl.Energy;

        #region Initialization

        // Index-based
        public void Initialize(Func<int, double> initNeuronBias, Func<int, int, double> initSynapseWeight)
        {
            for (int neuronIndex = 0; neuronIndex < Neurons; neuronIndex++)
                InitializeNeuron(neuronIndex, initNeuronBias, initSynapseWeight);
        }

        // Position-based
        public void Initialize(Func<Position, double> initNeuronBias, Func<Position, Position, double> initSynapseWeight)
        {
            Func<int, double> initNeuronBiasByIndex = (int neuronIndex)
                => initNeuronBias(NeuronIndexToPosition(neuronIndex));

            Func<int, int, double> initSynapseWeightByIndex = (int neuronIndex, int sourceNeuronIndex)
                => initSynapseWeight(NeuronIndexToPosition(neuronIndex), NeuronIndexToPosition(sourceNeuronIndex));

            Initialize(initNeuronBiasByIndex, initSynapseWeightByIndex);
        }

        private Position NeuronIndexToPosition(int index) => new Position(index / Cols.Value, index % Cols.Value);

        private void InitializeNeuron(int neuronIndex, Func<int, double> initNeuronBias, Func<int, int, double> initSynapseWeight)
        {
            SetNeuronBias(neuronIndex, initNeuronBias);
            InitializeSynapses(neuronIndex, initSynapseWeight);
        }

        private void InitializeSynapses(int neuronIndex, Func<int, int, double> initSynapseWeight)
        {
            for (int sourceNeuronIndex = 0; sourceNeuronIndex < Neurons; sourceNeuronIndex++)
            {
                if (sourceNeuronIndex == neuronIndex) continue;
                SetSynapseWeight(neuronIndex, sourceNeuronIndex, initSynapseWeight);
            }
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

        private void SetNeuronBias(int neuronIndex, Func<int, double> initNeuronBias)
        {
            SetNeuronBias(neuronIndex, initNeuronBias(neuronIndex));
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

        private void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, Func<int, int, double> initSynapseWeight)
        {
            SetSynapseWeight(neuronIndex, sourceNeuronIndex, initSynapseWeight(neuronIndex, sourceNeuronIndex));
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
