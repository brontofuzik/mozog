using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationNetwork : Network
    {
        internal BackpropagationNetwork(NetworkArchitecture architecture)
            : base(architecture)
        {
        }

        protected override ILayer MakeLayer(NetworkArchitecture.Layer layer)
            => layer.Activation != null ? (ILayer)new BackpropagationLayer(layer) : (ILayer)new InputLayer(layer);

        protected override IConnector MakeConnector(NetworkArchitecture.Connector connector)
            => new BackpropagationConnector(connector);

        public double Error { get; private set; }

        private new IEnumerable<BackpropagationConnector> Connectors
            => base.Connectors.Cast<BackpropagationConnector>();

        public void SetSynapseLearningRates(double synapseLearningRate)
        {
            Connectors.ForEach(c => c.SetLearningRates(synapseLearningRate));
        }

        public void SetConnectorMomenta(double connectorMomentum)
        {
            Connectors.ForEach(c => c.Momentum = connectorMomentum);
        }

        public void ResetError()
        {
            Error = 0.0;
        }

        public void ResetSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.ResetPartialDerivatives();
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
            (OutputLayer as BackpropagationLayer).Backpropagate(desiredOutputVector);

            // Hidden layers (backwards).
            for (int i = Layers.Count - 2; i > 0; i--)
            {
                (Layers[i] as BackpropagationLayer).Backpropagate();
            }
        }

        public void UpdateSynapsePartialDerivatives()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdatePartialDerivatives();
            }
        }

        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateWeights();
            }
        }

        public void UpdateSynapseLearningRates()
        {
            foreach (BackpropagationConnector connector in Connectors)
            {
                connector.UpdateLearningRates();
            }
        }

        public override string ToString() => "BP" + base.ToString();
    }
}

