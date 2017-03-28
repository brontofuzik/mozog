using System;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationNetwork
    {
        public BackpropagationNetwork(INetwork network)
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

        public double Error { get; private set; }

        public void SetSynapseLearningRates(double synapseLearningRate)
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.SetSynapseLearningRates(synapseLearningRate);
            }
        }

        public void SetConnectorMomenta(double connectorMomentum)
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.SetMomentum(connectorMomentum);
            }
        }

        public void ResetError()
        {
            Error = 0.0;
        }

        public void ResetSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.ResetSynapsePartialDerivatives();
            }
        }

        public void UpdateError()
        {
            double partialError = 0.0;
            foreach (BackpropagationNeuron outputNeuron in OutputLayer.Neurons)
            {
                partialError += Math.Pow(outputNeuron.PartialDerivative, 2);
            }

            Error += 0.5 * partialError;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double[] desiredOutputVector)
        {
            // Output layer.
            (OutputLayer as BackpropagationLayer).Backpropagate(desiredOutputVector);

            // Hidden layers (backwards).
            for (int i = HiddenLayers.Count - 1; i >= 0; i--)
            {
                (HiddenLayers[i] as BackpropagationLayer).Backpropagate();
            }
        }

        public void UpdateSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapsePartialDerivatives();
            }
        }

        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseWeights();
            }
        }

        public void UpdateSynapseLearningRates()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateSynapseLearningRates();
            }
        }

        public override string ToString() => "BP" + base.ToString();
    }
}

