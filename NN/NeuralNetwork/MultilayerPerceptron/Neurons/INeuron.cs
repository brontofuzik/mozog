using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// A neuron is the fundamental building block of a neural network.
    /// </summary>
    public interface INeuron
    {
        double Output { get; }

        List<ISynapse> TargetSynapses { get; }

        ILayer ParentLayer { get; set; }

        void Initialize();
    }
}
