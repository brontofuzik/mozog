namespace NeuralNetwork.MultilayerPerceptron.Backpropagation.Decorators
{
    /* TODO Backprop
    class BackpropagationConnector : ConnectorDecorator
    {
        /// <summary>
        /// Creates a new backpropagation connector by decorating a connector.
        /// </summary>
        /// <param name="connector">The connector to be decorated as backpropagation connector.</param>
        /// <param name="parentNetwork">The parent network.</param>
        public BackpropagationConnector(IConnector connector,INetwork parentNetwork)
            : base(connector, parentNetwork)
        {
            // Decorate the synapses.
            for (int i = 0; i < SynapseCount; i++)
            {
                Synapses[i] = new BackpropagationSynapse(Synapses[i],this);
            }
        }

        /// <summary>
        /// Connects the (backpropagation) connector.
        /// 
        /// A connector is said to be connected if:
        /// 1. it is aware of its source layer (and vice versa),
        /// 2. it is aware of its target layer (and vice versa), and
        /// 3. its synapses are connected.
        /// </summary>
        public override void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// Disconnects the (backpropagation) connector.
        /// 
        /// A connector is said to be disconnected if:
        /// 1. its source layer is not aware of it (and vice versa),
        /// 2. its target layer is not aware of it (and vice versa), and
        /// 3. its synapses are disconnected.
        /// </summary>
        public override void Disconnect()
        {
            Disconnect(this);
        }

        /// <summary>
        /// Sets the learning rates of all synapses in the network.
        /// </summary>
        /// <param name="synapseLearningRate"></param>
        public void SetSynapseLearningRates(double synapseLearningRate)
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.SetLearningRate(synapseLearningRate);
            }
        }

        /// <summary>
        /// Sets the momentum of the connector.
        /// </summary>
        /// <param name="momentum"></param>
        public void SetMomentum(double momentum)
        {
            _momentum = momentum;
        }

        /// <summary>
        /// Resets the partial derivatives of all synapses in the network.
        /// </summary>
        public void ResetSynapsePartialDerivatives()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.ResetPartialDerivative();
            }
        }

        /// <summary>
        /// Updates the partial derivatives of all synapses in the network.
        /// </summary>
        public void UpdateSynapsePartialDerivatives()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdatePartialDerivative();
            }
        }

        /// <summary>
        /// Updates the weights of all synapses in the connector.
        /// </summary>
        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateWeight();
            }
        }

        /// <summary>
        /// Updates the learning rates of all synapses in the connector.
        /// </summary>
        public void UpdateSynapseLearningRates()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateLearningRate();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation connector.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation connector.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        public double Momentum
        {
            get
            {
                return _momentum;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private double _momentum;
    }
    */
}

