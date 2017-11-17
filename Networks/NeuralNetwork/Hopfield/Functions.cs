using System.Collections.Generic;

namespace NeuralNetwork.Hopfield
{
    public delegate double ActivationFunction(double input, double progress);

    public delegate IEnumerable<T> Topology<T>(T neuron, HopfieldNetwork<T> net)
        where T : IPosition;

    public delegate double InitNeuronBias<T>(T neuron, HopfieldNetwork<T> net)
        where T : IPosition;

    public delegate double InitSynapseWeight<T>(T neuron, T sourceNeuron, HopfieldNetwork<T> net)
        where T : IPosition;
}