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

        public HopfieldNetwork(int neuronCount, bool sparse, ActivationFunction activationFunction)
        {
            Require.IsPositive(neuronCount, nameof(neuronCount));
            Require.IsNotNull(activationFunction, nameof(activationFunction));

            networkImpl = sparse
                ? (IHopfieldNetworkImpl)new SparseHopfieldNetworkImpl(neuronCount, activationFunction)
                : (IHopfieldNetworkImpl)new FullHopfieldNetworkImpl(neuronCount, activationFunction);
        }

        public HopfieldNetwork(int neuronCount, ActivationFunction activationFunction)
            : this(neuronCount, false, activationFunction)
        {
        }

        public HopfieldNetwork(int neuronCount, bool sparse)
            : this(neuronCount, sparse, DefaultActivation)
        {
        }

        public HopfieldNetwork(int neuronCount)
            : this(neuronCount, false, DefaultActivation)
        {
        }

        public int NeuronCount => networkImpl.NeuronCount;

        public int SynapseCount => networkImpl.SynapseCount;

        public double Energy => networkImpl.Energy;

        public double GetNeuronBias(int neuronIndex)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, NeuronCount - 1);

            return networkImpl.GetNeuronBias(neuronIndex);
        }

        public void SetNeuronBias(int neuronIndex, double bias)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, NeuronCount - 1);

            networkImpl.SetNeuronBias(neuronIndex, bias);
        }

        public double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, NeuronCount - 1);
            Require.IsWithinRange(sourceNeuronIndex, nameof(sourceNeuronIndex), 0, NeuronCount - 1);

            return networkImpl.GetSynapseWeight(neuronIndex, sourceNeuronIndex);
        }

        public void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double weight)
        {
            Require.IsWithinRange(neuronIndex, nameof(neuronIndex), 0, NeuronCount - 1);
            Require.IsWithinRange(sourceNeuronIndex, nameof(sourceNeuronIndex), 0, NeuronCount - 1);

            networkImpl.SetSynapseWeight(neuronIndex, sourceNeuronIndex, weight);
        }

        public void Train(DataSet data)
        {
            Require.IsNotNull(data, nameof(data));

            if (data.InputSize != NeuronCount)
                throw new ArgumentException("The training set is not compatible with the network.", nameof(data));

            for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
                TrainNeuron(neuronIndex, data);
        }

        private void TrainNeuron(int neuronIndex, DataSet data)
        {
            SetNeuronBias(neuronIndex, 0.0);

            for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; sourceNeuronIndex++)
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

        public double[] Evaluate(double[] input, int iterations)
        {
            if (input.Length != NeuronCount)
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
            for (int targetNeuronIndex = 0; targetNeuronIndex < NeuronCount; ++targetNeuronIndex)
            {
                sb.Append("[");
                for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
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
}
