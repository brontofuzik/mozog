using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <remarks>
    /// 
    /// </remarks>
    public abstract class ConnectorDecorator
        : IConnector
    {
        /// <summary>
        /// 
        /// </summary>
        protected IConnector decoratedConnector;

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
                return decoratedConnector.Blueprint;
            }
        }

        /// <summary>
        /// Gets the synapses.
        /// </summary>
        ///
        /// <value>
        /// The synapses.
        /// </value>
        public virtual List<ISynapse> Synapses
        {
            get
            {
                return decoratedConnector.Synapses;
            }
        }

        /// <summary>
        /// Gets the number of synapses. 
        /// </summary>
        /// 
        /// <value>
        /// The number of synapses.
        /// </value>
        public virtual int SynapseCount
        {
            get
            {
                return decoratedConnector.SynapseCount;
            }
        }

        /// <summary>
        /// Gets the source layer.
        /// </summary>
        ///
        /// <value>
        /// The source layer.
        /// </value>
        public virtual ILayer SourceLayer
        {
            get
            {
                return decoratedConnector.SourceLayer;
            }
            set
            {
                decoratedConnector.SourceLayer = value;
            }
        }

        /// <summary>
        /// Gets the target layer.
        /// </summary>
        /// 
        /// <value>
        /// The target layer.
        /// </value>
        public virtual IActivationLayer TargetLayer
        {
            get
            {
                return decoratedConnector.TargetLayer;
            }
            set
            {
                decoratedConnector.TargetLayer = value;
            }
        }

        /// <summary>
        /// Gets the parent network.
        /// </summary>
        /// 
        /// <value>
        /// The parent network.
        /// </value>
        public virtual INetwork ParentNetwork
        {
            get
            {
                return decoratedConnector.ParentNetwork;
            }
            set
            {
                decoratedConnector.ParentNetwork = value;
            }
        }

        /// <summary>
        /// Creates a new connector decorator.
        /// </summary>
        /// <param name="decoratedConnector">The connector to be decorated.</param>
        /// <param name="parentNetwork">The parent network.</param>
        public ConnectorDecorator( IConnector decoratedConnector, INetwork parentNetwork )
        {
            this.decoratedConnector = decoratedConnector;
            ParentNetwork = parentNetwork;
        }

        /// <summary>
        /// Connects the connector.
        /// 
        /// A connector is said to be connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector being connected.</param>
        public virtual void Connect(IConnector connector)
        {
            decoratedConnector.Connect(connector);
        }

        /// <summary>
        /// Disconnects the connector.
        /// 
        /// A connector is said to be disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connector">The connector to be disconnected.</param>
        public virtual void Disconnect(IConnector connector)
        {
            decoratedConnector.Disconnect(connector);
        }

        /// <summary>
        /// Returns the decorated connector.
        /// </summary>
        /// <param name="parentNetwork">The parent netowrk.</param>
        /// <returns>
        /// The decorated connector.
        /// </returns>
        public virtual IConnector GetDecoratedConnector(INetwork parentNetwork)
        {
            // Undecorate the synapses.
            for (int i = 0; i < SynapseCount; i++)
            {
                Synapses[ i ] = (Synapses[ i ] as SynapseDecorator).GetDecoratedSynapse(decoratedConnector);
            }

            // Reintegrate.
            ParentNetwork = parentNetwork;

            return decoratedConnector;
        }

        /// <summary>
        /// Initializes the connector.
        /// </summary>
        public virtual void Initialize()
        {
            decoratedConnector.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        public virtual void Jitter(double jitterNoiseLimit)
        {
            decoratedConnector.Jitter(jitterNoiseLimit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return decoratedConnector.ToString();
        }
    }
}
