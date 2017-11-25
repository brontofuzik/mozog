using System.Collections.Generic;

namespace NeuralNetwork.Hopfield
{
    public delegate double ActivationFunction(double input, double progress);

    public delegate IEnumerable<int[]> Topology(int[] neuron, HopfieldNetwork net);

    public delegate double InitNeuronBias(int[] neuron, HopfieldNetwork net);

    public delegate double InitSynapseWeight(int[] neuron, int[] sourceNeuron, HopfieldNetwork net);
}