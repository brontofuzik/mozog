using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <summary>
    /// A connector is a collection of synapses connecting neurons from one layer to another layer.
    /// </summary>
    public interface IConnector
    {
        ConnectorBlueprint Blueprint { get; }

        List<ISynapse> Synapses { get; }

        int SynapseCount { get; }

        ILayer SourceLayer { get; set; }

        IActivationLayer TargetLayer { get; set; }

        INetwork ParentNetwork { get; set; }

        /// <summary>
        /// A connector is connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        void Connect();

        void Connect(IConnector connector);

        /// <summary>
        /// A connector is disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        void Disconnect();

        void Disconnect(IConnector connector);

        void Initialize();

        void Jitter(double jitterNoiseLimit);
    }
}
