using System;
using System.Linq;
using Mozog.Utils.Math;

namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    class HopfieldImpl
    {
        IMatrix weights;
        private double[] biases;
        private double[] outputs;

        private ActivationFunction activation;

        public HopfieldImpl(int neurons, ActivationFunction activation)
        {
            Neurons = neurons;
            weights = new FullMatrix(neurons, neurons);
            biases = new double[neurons];
            outputs = new double[neurons];

            this.activation = activation;
        }

        public int Neurons { get; }

        public int Synapses => (weights.Size - Neurons) / 2;

        public double GetNeuronBias(int neuron)
        {
            return biases[neuron];
        }

        public void SetNeuronBias(int neuron, double bias)
        {
            biases[neuron] = bias;
        }

        public double GetSynapseWeight(int neuron, int sourceNeuron)
        {
            return weights[neuron, sourceNeuron];
        }

        public void SetSynapseWeight(int neuron, int sourceNeuron, double weight)
        {
            weights[neuron, sourceNeuron] = weights[sourceNeuron, neuron] = weight;
        }

        public void SetNetworkInput(double[] input)
        {
            Array.Copy(input, outputs, input.Length);
        }

        public void Evaluate(double progress)
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

        public double[] GetNetworkOutput()
        {
            return (double[])outputs.Clone();
        }

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

        private int[] RandomNeurons
        {
            get
            {
                int[] randomNeurons = Enumerable.Range(0, Neurons).ToArray();
                StaticRandom.Shuffle(randomNeurons);
                return randomNeurons;
            }
        }
    }
}
