using System.Collections.Generic;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <remarks>
    /// A connector in a neural network.
    /// </remarks>
    public class Connector : IConnector
    {
        /// <summary>
        /// The blueprint of the connector.
        /// </summary>
        private readonly ConnectorBlueprint blueprint;

        public Connector(ConnectorBlueprint blueprint, INetwork parentNetwork)
        {
            Require.IsNotNull(blueprint, nameof(blueprint));
            this.blueprint = blueprint;

            // Create the synapses.
            Synapses = new List<ISynapse>(this.blueprint.SynapseCount);
            foreach (SynapseBlueprint synapseBlueprint in this.blueprint.SynapseBlueprints)
            {
                ISynapse synapse = new Synapse(synapseBlueprint, this);
                Synapses.Add(synapse);
            }

            Require.IsNotNull(parentNetwork, nameof(parentNetwork));
            this.ParentNetwork = parentNetwork;
        }

        public virtual ConnectorBlueprint Blueprint => blueprint;

        public List< ISynapse > Synapses { get; }

        public int SynapseCount => Synapses.Count;

        public ILayer SourceLayer { get; set; }

        public IActivationLayer TargetLayer { get; set; }

        public INetwork ParentNetwork { get; set; }

        public void Connect()
        {
            Connect(this);
        }

        public void Connect(IConnector connector)
        {
            // 1. Make the connector aware of its source layer ...
            SourceLayer = ParentNetwork.GetLayerByIndex(blueprint.SourceLayerIndex);
            // ... and vice versa.
            SourceLayer.TargetConnectors.Add(connector);

            // 2. Make the connector aware of its target layer ...
            TargetLayer = (IActivationLayer)ParentNetwork.GetLayerByIndex(blueprint.TargetLayerIndex);
            // ... and vice versa.
            TargetLayer.SourceConnectors.Add(connector);

            // 3. Connect the synapses.
            foreach (ISynapse synapse in Synapses)
            {
                synapse.Connect();
            }
        }

        public void Disconnect()
        {
            Disconnect(this);
        }

        public void Disconnect(IConnector connector)
        {
            // 1. Make the connector's source layer not aware of it ...
            SourceLayer.TargetConnectors.Remove(connector);
            // ... and vice versa.
            SourceLayer = null;

            // 2. Make the connector's target layer not aware of it ...
            TargetLayer.SourceConnectors.Remove(connector);
            // ... and vice versa.
            TargetLayer = null;

            // 3. Disconnect the synapses.
            foreach (ISynapse synapse in Synapses)
            {
                synapse.Disconnect();
            }
        }

        public void Initialize()
        {
            foreach (ISynapse synapse in Synapses)
            {
                synapse.Initialize();
            }
        }

        public void Jitter(double jitterNoiseLimit)
        {
            foreach (ISynapse synapse in Synapses)
            {
                synapse.Jitter(jitterNoiseLimit);
            }
        }
    }
}