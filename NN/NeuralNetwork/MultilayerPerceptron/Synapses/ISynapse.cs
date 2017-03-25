using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /// <summary>
    /// A synapse is a connection between two neurons.
    /// </summary>
    public interface ISynapse
    {
        SynapseBlueprint Blueprint { get; }

        double Weight { get; set; }

        INeuron SourceNeuron { get; set; }

        IActivationNeuron TargetNeuron { get; set; }

        IConnector ParentConnector { get; set; }

        /// <summary>
        /// A synapse is connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        void Connect();

        void Connect(ISynapse synapse);

        /// <summary>
        /// A synapse is disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        void Disconnect();

        void Disconnect(ISynapse synapse);

        void Initialize();

        void Jitter(double jitterNoiseLimit);
    }
}
