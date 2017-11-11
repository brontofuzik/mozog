using System;
using System.Linq;
using Mozog.Utils.Math;

namespace NeuralNetwork.HopfieldNet.HopfieldNetworkImps
{
    class FullHopfieldNetworkImpl : IHopfieldNetworkImpl
    {
        private double[,] weights;
        private double[] biases;

        private ActivationFunction activationFunction;

        private double[] outputs;

        public FullHopfieldNetworkImpl(int neuronCount, ActivationFunction activationFunction)
        {
            Neurons = neuronCount;
            this.activationFunction = activationFunction;

            weights = new double[Neurons, Neurons];
            biases = new double[Neurons];
            outputs = new double[Neurons];
        }

        public int Neurons { get; }

        public int Synapses => (weights.Length - Neurons) / 2;

        public double GetNeuronBias(int neuronIndex)
        {
            return biases[neuronIndex];
        }

        public void SetNeuronBias(int neuronIndex, double bias)
        {
            biases[neuronIndex] = bias;
        }

        public double GetSynapseWeight(int neuron1Index, int neuron2Index)
        {
            return weights[neuron1Index, neuron2Index];
        }

        public void SetSynapseWeight(int neuron1Index, int neuron2Index, double weight)
        {
            weights[neuron1Index, neuron2Index] = weights[neuron2Index, neuron1Index] = weight;
        }

        public void SetNetworkInput(double[] input)
        {
            Array.Copy(input, outputs, Neurons);
        }

        public void Evaluate(double progress)
        {
            foreach (int n in RandomNeurons)
                EvaluateNeuron(n, progress);
        }

        private void EvaluateNeuron(int neuron, double progress)
        {
            // Calculate neuron input
            double input = 0.0;
            for (int sourceNeuron = 0; sourceNeuron < Neurons; sourceNeuron++)
            {
                input += weights[neuron, sourceNeuron] * outputs[sourceNeuron];
            }
            input += biases[neuron];

            // Calculate neuron output
            outputs[neuron] = activationFunction(input, progress);
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
                {
                    for (int sourceNeuron = neuron + 1; sourceNeuron < Neurons; sourceNeuron++)
                    {
                        energy -= weights[neuron, sourceNeuron] * outputs[neuron] * outputs[sourceNeuron];
                    }
                }

                // Neuron energy
                for (int neuron = 0; neuron < Neurons; neuron++)
                {
                    energy -= biases[neuron] * outputs[neuron];
                }

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
