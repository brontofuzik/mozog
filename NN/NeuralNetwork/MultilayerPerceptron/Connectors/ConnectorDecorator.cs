using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    public abstract class ConnectorDecorator : IConnector
    {
        protected IConnector decoratedConnector;

        protected ConnectorDecorator(IConnector decoratedConnector, INetwork parentNetwork)
        {
            this.decoratedConnector = decoratedConnector;
            ParentNetwork = parentNetwork;
        }

        public virtual ConnectorBlueprint Blueprint => decoratedConnector.Blueprint;

        public virtual List<ISynapse> Synapses => decoratedConnector.Synapses;

        public virtual int SynapseCount => decoratedConnector.SynapseCount;

        public virtual ILayer SourceLayer
        {
            get { return decoratedConnector.SourceLayer; }
            set { decoratedConnector.SourceLayer = value; }
        }

        public virtual IActivationLayer TargetLayer
        {
            get { return decoratedConnector.TargetLayer; }
            set { decoratedConnector.TargetLayer = value; }
        }

        public virtual INetwork ParentNetwork
        {
            get { return decoratedConnector.ParentNetwork; }
            set { decoratedConnector.ParentNetwork = value; }
        }

        /// <summary>
        /// A connector is connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        public abstract void Connect();

        public virtual void Connect(IConnector connector)
        {
            decoratedConnector.Connect(connector);
        }

        /// <summary>
        /// A connector is disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        public abstract void Disconnect();

        public virtual void Disconnect(IConnector connector)
        {
            decoratedConnector.Disconnect(connector);
        }

        public virtual IConnector GetDecoratedConnector(INetwork parentNetwork)
        {
            // Undecorate the synapses.
            for (int i = 0; i < SynapseCount; i++)
            {
                Synapses[i] = (Synapses[i] as SynapseDecorator).GetDecoratedSynapse(decoratedConnector);
            }

            // Reintegrate.
            ParentNetwork = parentNetwork;
            return decoratedConnector;
        }

        public virtual void Initialize()
        {
            decoratedConnector.Initialize();
        }

        public virtual void Jitter(double jitterNoiseLimit)
        {
            decoratedConnector.Jitter(jitterNoiseLimit);
        }

        public override string ToString() => decoratedConnector.ToString();
    }
}
