using System;
using System.Linq;
using System.Collections.Generic;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    class SparseHopfieldNetworkImpl : IHopfieldNetworkImpl
    {
        private HopfieldNeuron[] neurons;
        private ICollection<HopfieldSynapse> synapses = new HashSet<HopfieldSynapse>();

        public SparseHopfieldNetworkImpl(int neuronCount, ActivationFunction activationFunction)
        {
            neurons = Enumerable.Range(0, neuronCount).Select(n => new HopfieldNeuron(n, activationFunction)).ToArray();
        }

        public int NeuronCount => neurons.Length;

        public int SynapseCount => synapses.Count;

        public double GetNeuronBias(int neuronIndex)
            => neurons[neuronIndex].Bias;

        public void SetNeuronBias(int neuronIndex, double bias)
        {
            neurons[neuronIndex].Bias = bias;
        }

        public double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex)
            => GetSynapse(neuronIndex, sourceNeuronIndex)?.Weight ?? 0.0;

        public void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double weight)
        {
            if (weight != 0.0)
            {
                CreateSynapseIfNonexistent(neuronIndex, sourceNeuronIndex).Weight = weight;
            }
            else
            {
                DeleteSynapseIfExistent(neuronIndex, sourceNeuronIndex);
            }
        }

        public void SetNetworkInput(double[] input)
        {
            if (input.Length != NeuronCount)
                throw new ArgumentException("The length of the pattern to recall must match the number of neurons in the network.", nameof(input));

            neurons.ForEach(n => n.Output = input[n.Index]);
        }

        public void Evaluate(double progress)
        {
            foreach (var neuron in NeuronsRandom)
                neuron.Evaluate(progress);
        }

        public double[] GetNetworkOutput()
        {
            return neurons.Select(n => n.Output).ToArray();
        }

        public double Energy
        {
            get
            {
                double energy = 0.0;

                // Calculate the energy of the synapses.
                foreach (HopfieldSynapse synapse in synapses)
                {
                    energy -= synapse.Weight * synapse.Neuron1.Output * synapse.Neuron2.Output;
                }

                // Calculate the energy of the neurons.
                foreach (HopfieldNeuron neuron in neurons)
                {
                    energy -= neuron.Bias * neuron.Output;
                }

                return energy;
            }
        }

        private HopfieldNeuron[] NeuronsRandom
        {
            get
            {
                var neuronsRandom = (HopfieldNeuron[])neurons.Clone();
                StaticRandom.Shuffle(neuronsRandom);
                return neuronsRandom;
            }
        }

        private HopfieldSynapse GetSynapse(int neuron1Index, int neuron2Index)
        {
            var neuron1 = neurons[neuron1Index];
            var neuron2 = neurons[neuron2Index];

            foreach (HopfieldSynapse synapse in neuron2.Synapses)
            {
                if (synapse.GetOtherNeuron(neuron2) == neuron1)
                {
                    return synapse;
                }
            }
            return null;
        }

        private HopfieldSynapse CreateSynapseIfNonexistent(int neuron1, int neuron2)
            => GetSynapse(neuron1, neuron2) ?? CreateSynapse(neuron1, neuron2);

        private HopfieldSynapse CreateSynapse(int neuron1, int neuron2)
        {
            var n1 = neurons[neuron1];
            var n2 = neurons[neuron2];
            var synapse = new HopfieldSynapse(n1, n2);

            synapses.Add(synapse);
            n1.Synapses.Add(synapse);
            n2.Synapses.Add(synapse);

            return synapse;
        }

        private void DeleteSynapseIfExistent(int neuron1, int neuron2)
        {
            var synapse = GetSynapse(neuron1, neuron2);
            if (synapse != null)
                DeleteSynapse(synapse);
        }

        private void DeleteSynapse(HopfieldSynapse synapse)
        {
            synapses.Remove(synapse);
            synapse.Neuron1.Synapses.Remove(synapse);
            synapse.Neuron2.Synapses.Remove(synapse);
        }
    }
}
