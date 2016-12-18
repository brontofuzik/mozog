using System;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators
{
    /// <summary>
    /// A backpropagation network.
    /// </summary>
    public class BackpropagationNetwork
        : NetworkDecorator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        public BackpropagationNetwork(INetwork network)
            : base(network)
        {
            // 1. Disconnect the network.
            Disconnect();

            // 2. Decorate the network components.
            // 2.1. Decorate the layers.
            // 2.1.1. Decorate the bias layer.
            BiasLayer.ParentNetwork = this;

            // 2.1.2. Decorate the input layer.
            InputLayer.ParentNetwork = this;

            // 2.1.3. Decorate the hidden layers.
            for (int i = 0; i < HiddenLayerCount; i++)
            {
                HiddenLayers[i] = new BackpropagationLayer(HiddenLayers[i], this);
            }

            // 2.1.4. Decorate the output layer.
            OutputLayer = new BackpropagationLayer(OutputLayer, this);

            // 2.2. Decorate the connectors.
            for (int i = 0; i < ConnectorCount; i++)
            {
                Connectors[i] = new BackpropagationConnector(Connectors[i], this);
            }

            // 3. Connect the network.
            Connect();
        }

        /// <summary>
        /// Sets the learning rates of all synapses in the network.
        /// </summary>
        /// <param name="synapseLearningRate"></param>
        public void SetSynapseLearningRates( double synapseLearningRate )
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.SetSynapseLearningRates( synapseLearningRate );
            }
        }

        /// <summary>
        /// Sets the momenta of all connectors in the network.
        /// </summary>
        /// <param name="connectorMomentum"></param>
        public void SetConnectorMomenta( double connectorMomentum )
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.SetMomentum( connectorMomentum );
            }
        }

        /// <summary>
        /// Resets the error.
        /// </summary>
        public void ResetError()
        {
            _error = 0.0;
        }

        /// <summary>
        /// Resets the partial derivatives of all synapses in the network.
        /// </summary>
        public void ResetSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.ResetSynapsePartialDerivatives();
            }
        }

        /// <summary>
        /// Updates the error.
        /// </summary>
        public void UpdateError()
        {
            double partialError = 0.0;
            foreach (BackpropagationNeuron outputNeuron in OutputLayer.Neurons)
            {
                partialError += Math.Pow( outputNeuron.PartialDerivative, 2 );
            }

            _error += 0.5 * partialError;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate( double[] desiredOutputVector )
        {
            // Output layer.
            (OutputLayer as BackpropagationLayer).Backpropagate( desiredOutputVector );

            // Hidden layers (backwards).
            for (int i = HiddenLayers.Count - 1; i >= 0; i--)
            {
                (HiddenLayers[i] as BackpropagationLayer).Backpropagate();
            }
        }

        /// <summary>
        /// Updates the partial derivatives of all synapses in the netowork.
        /// </summary>
        public void UpdateSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapsePartialDerivatives();
            }
        }

        /// <summary>
        /// Updates the weights of all synapses in the network.
        /// </summary>
        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseWeights();
            }
        }

        /// <summary>
        /// Updates the learning rates of all synapses in the network.
        /// </summary>
        public void UpdateSynapseLearningRates()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseLearningRates();
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation network.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation network.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        /// <summary>
        /// Gets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public double Error
        {
            get
            {
                return _error;
            }
        }

        /// <summary>
        /// The error of the network.
        /// </summary>
        private double _error;
    }
}

