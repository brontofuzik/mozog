using System.Collections.Generic;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class LayerBase<TNeuron>
        where TNeuron : INeuron
    {
        public LayerBase(INetwork network)
        {
            Network = network;
        }

        // Factory
        internal LayerBase()
        {
        }

        public IList<TNeuron> Neurons { get; } = new List<TNeuron>();

        public int NeuronCount => Neurons.Count;

        public List<IConnector> SourceConnectors { get; } = new List<IConnector>();

        public List<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public INetwork Network { get; set; }
    }
}
