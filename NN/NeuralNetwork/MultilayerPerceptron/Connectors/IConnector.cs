using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <summary>
    /// <para>
    /// An interface of a connector.
    /// </para>
    /// <para>
    /// Definition: A connector is a collection of synapses connecting neurons from one layer to another layer.
    /// </para>
    /// </summary>
    public interface IConnector
    {
        #region Properties

        /// <summary>
        /// Gets the blueprint of the connector.
        /// </summary>
        /// 
        /// <value>
        /// The blueprint of the connector.
        /// </value>
        ConnectorBlueprint Blueprint
        {
            get;
        }

        /// <summary>
        /// Gets the synapses.
        /// </summary>
        ///
        /// <value>
        /// The synapses.
        /// </value>
        List<ISynapse> Synapses
        {
            get;
        }

        /// <summary>
        /// Gets the number of synapses. 
        /// </summary>
        /// 
        /// <value>
        /// The number of synapses.
        /// </value>
        int SynapseCount
        {
            get;
        }

        /// <summary>
        /// Gets the source layer.
        /// </summary>
        ///
        /// <value>
        /// The source layer.
        /// </value>
        ILayer SourceLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target layer.
        /// </summary>
        /// 
        /// <value>
        /// The target layer.
        /// </value>
        IActivationLayer TargetLayer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        INetwork ParentNetwork
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Connects the connector.
        /// 
        /// A connector is said to be connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        void Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector being connected.</param>
        void Connect(IConnector connector);

        /// <summary>
        /// Disconnects the connector.
        /// 
        /// A connector is said to be disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector to be disconnected.</param>
        void Disconnect(IConnector connector);

        /// <summary>
        /// Initializes the connector.
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        void Jitter(double jitterNoiseLimit);

        #endregion // Methods
    }
}
