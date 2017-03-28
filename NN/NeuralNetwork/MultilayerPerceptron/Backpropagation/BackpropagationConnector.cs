using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class BackpropagationConnector : IConnector
    {
        public BackpropagationConnector(IConnector connector, INetwork parentNetwork)
        {
            // Decorate the synapses.
            for (int i = 0; i < SynapseCount; i++)
            {
                Synapses[i] = new BackpropagationSynapse(Synapses[i],this);
            }
        }

        // Factory
        internal BackpropagationConnector()
        {
        }

        public double Momentum { get; set; }

        public void SetSynapseLearningRates(double synapseLearningRate)
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.SetLearningRate(synapseLearningRate);
            }
        }

        public void ResetSynapsePartialDerivatives()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.ResetPartialDerivative();
            }
        }

        public void UpdateSynapsePartialDerivatives()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdatePartialDerivative();
            }
        }

        public void UpdateSynapseWeights()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateWeight();
            }
        }

        public void UpdateSynapseLearningRates()
        {
            foreach (BackpropagationSynapse synapse in Synapses)
            {
                synapse.UpdateLearningRate();
            }
        }

        public override string ToString() => "BP" + base.ToString();
    }
}

