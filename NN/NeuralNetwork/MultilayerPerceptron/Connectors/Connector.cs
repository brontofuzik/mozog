using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <remarks>
    /// A connector in a neural network.
    /// </remarks>
    public class Connector
        : IConnector
    {
        /// <summary>
        /// The blueprint of the connector.
        /// </summary>
        private ConnectorBlueprint blueprint;

        /// <summary>
        /// The synapses of the connector.
        /// </summary>
        private List<ISynapse> synapses;

        /// <summary>
        /// The source layer of the connector.
        /// </summary>
        private ILayer sourceLayer;

        /// <summary>
        /// The target layer of the connector.
        /// </summary>
        private IActivationLayer targetLayer;

        /// <summary>
        /// The parent network of the connector.
        /// </summary>
        private INetwork parentNetwork;

        /// <summary>
        /// Gets the blueprint of the connector.
        /// </summary>
        /// 
        /// <value>
        /// The blueprint of the connector.
        /// </value>
        public virtual ConnectorBlueprint Blueprint
        {
            get
            {
                return blueprint;
            }
        }

        /// <summary>
        /// Gets the synapses.
        /// </summary>
        ///
        /// <value>
        /// The synapses.
        /// </value>
        public List< ISynapse > Synapses
        {
            get
            {
                return synapses;
            }
        }

        /// <summary>
        /// Gets the number of synapses. 
        /// </summary>
        /// 
        /// <value>
        /// The number of synapses.
        /// </value>
        public int SynapseCount
        {
            get
            {
                return synapses.Count;
            }
        }

        /// <summary>
        /// Gets the source layer.
        /// </summary>
        ///
        /// <value>
        /// The source layer.
        /// </value>
        public ILayer SourceLayer
        {
            get
            {
                return sourceLayer;
            }
            set
            {
                sourceLayer = value;
            }
        }

        /// <summary>
        /// Gets the target layer.
        /// </summary>
        /// 
        /// <value>
        /// The target layer.
        /// </value>
        public IActivationLayer TargetLayer
        {
            get
            {
                return targetLayer;
            }
            set
            {
                targetLayer = value;
            }
        }

        /// <summary>
        /// Gets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        public INetwork ParentNetwork
        {
            get
            {
                return parentNetwork;
            }
            set
            {
                parentNetwork = value;
            }
        }

        /// <summary>
        /// Creates a new connector (from a blueprint).
        /// </summary>
        /// <param name="blueprint">The blueprint of the connector.</param>
        /// <param name="parentNetwork">The parent network.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Condition 1: <c>blueprint</c> is <c>null</c>.
        /// Condition 2: <c>parentNetwork</c> is <c>null</c>.
        /// </exception>
        public Connector(ConnectorBlueprint blueprint, INetwork parentNetwork)
        {
            // Validate the connector blueprint.
            Utilities.RequireObjectNotNull(blueprint, "blueprint");
            this.blueprint = blueprint;

            // Create the synapses.
            synapses = new List<ISynapse>(this.blueprint.SynapseCount);
            foreach (SynapseBlueprint synapseBlueprint in this.blueprint.SynapseBlueprints)
            {
                ISynapse synapse = new Synapse(synapseBlueprint, this);
                synapses.Add(synapse);
            }

            // Validate the parent network.
            Utilities.RequireObjectNotNull(parentNetwork, "parentNetwork");
            this.parentNetwork = parentNetwork;
        }

        /// <summary>
        /// Connects the connector.
        /// 
        /// A connector is said to be connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        public void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector being connected.</param>
        public void Connect(IConnector connector)
        {
            // 1. Make the connector aware of its source layer ...
            sourceLayer = parentNetwork.GetLayerByIndex(blueprint.SourceLayerIndex);
            // ... and vice versa.
            sourceLayer.TargetConnectors.Add(connector);

            // 2. Make the connector aware of its target layer ...
            targetLayer = (IActivationLayer)parentNetwork.GetLayerByIndex(blueprint.TargetLayerIndex);
            // ... and vice versa.
            targetLayer.SourceConnectors.Add(connector);

            // 3. Connect the synapses.
            foreach (ISynapse synapse in synapses)
            {
                synapse.Connect();
            }
        }

        /// <summary>
        /// Disconnects the connector.
        /// 
        /// A connector is said to be disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        public void Disconnect()
        {
            Disconnect(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector to be disconnected.</param>
        public void Disconnect(IConnector connector)
        {
            // 1. Make the connector's source layer not aware of it ...
            sourceLayer.TargetConnectors.Remove(connector);
            // ... and vice versa.
            sourceLayer = null;

            // 2. Make the connector's target layer not aware of it ...
            targetLayer.SourceConnectors.Remove(connector);
            // ... and vice versa.
            targetLayer = null;

            // 3. Disconnect the synapses.
            foreach (ISynapse synapse in synapses)
            {
                synapse.Disconnect();
            }
        }

        /// <summary>
        /// Initializes the connector.
        /// </summary>
        public void Initialize()
        {
            foreach (ISynapse synapse in synapses)
            {
                synapse.Initialize();
            }
        }

        /// <summary>
        /// Jitters the connector so that the neural network deviates from its local minimum position where further learning is of no use.
        /// </summary>
        /// 
        /// <param name="jitterNoiseLimit">The maximum absolute jitter noise added.</param>
        public void Jitter(double jitterNoiseLimit)
        {
            foreach (ISynapse synapse in synapses)
            {
                synapse.Jitter(jitterNoiseLimit);
            }
        }
    }
}