using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    /// <summary>
    /// A layer is a collection of neurons.
    /// </summary>
    public interface ILayer
    {
        List<INeuron> Neurons { get; }

        int NeuronCount { get; }

        List<IConnector> TargetConnectors { get; }

        INetwork ParentNetwork { get; set; }

        INeuron GetNeuronByIndex(int neuronIndex);

        void Initialize();
    }
}
