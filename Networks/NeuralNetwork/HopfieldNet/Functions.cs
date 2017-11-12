using System.Collections.Generic;

namespace NeuralNetwork.HopfieldNet
{
    public delegate double ActivationFunction(double input, double progress);

    public delegate IEnumerable<T> Topology<T>(T neuron, HopfieldNetwork net)
        where T : struct;

    public delegate double InitNeuronBias<T>(T neuron, HopfieldNetwork net)
        where T : struct;

    public delegate double InitSynapseWeight<T>(T neuron, T sourceNeuron, HopfieldNetwork net)
        where T : struct;
}