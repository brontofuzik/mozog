using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    public class Synapse : ISynapse
    {
        /// <summary>
        /// Creates a new synapse (from a blueprint).
        /// </summary>
        /// <param name="blueprint">The blueprint of the synapse.</param>
        /// <param name="parentConnector">The parent connector.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Condition 1: <c>blueprint</c> is <c>null</c>.
        /// Condition 2: <c>parentConnector</c> is <c>null</c>.
        /// </exception>
        public Synapse(SynapseBlueprint blueprint, IConnector parentConnector)
        {
            Require.IsNotNull(blueprint, nameof(blueprint));
            Blueprint = blueprint;

            Require.IsNotNull(parentConnector, nameof(parentConnector));
            ParentConnector = parentConnector;
        }

        public SynapseBlueprint Blueprint { get; }

        public double Weight { get; set; }

        public INeuron SourceNeuron { get; set; }

        public IActivationNeuron TargetNeuron { get; set; }

        public IConnector ParentConnector { get; set; }

        public void Connect()
        {
            Connect(this);
        }

        public void Connect(ISynapse synapse)
        {
            // 1. Make the synapse aware of its source neuron ...
            ILayer sourceLayer = ParentConnector.SourceLayer;
            SourceNeuron = sourceLayer.GetNeuronByIndex(Blueprint.SourceNeuronIndex);
            // ... and vice versa.
            SourceNeuron.TargetSynapses.Add(synapse);

            // 2. Make the synapse aware of its target neuron ...
            ILayer targetLayer = ParentConnector.TargetLayer;
            TargetNeuron = (IActivationNeuron)targetLayer.GetNeuronByIndex(Blueprint.TargetNeuronIndex);
            // ... and vice versa.
            TargetNeuron.SourceSynapses.Add(synapse);
        }

        public void Disconnect()
        {
            Disconnect(this);
        }

        public void Disconnect(ISynapse synapse)
        {
            // 1. Make its source neuron not aware of it ...
            SourceNeuron.TargetSynapses.Remove(synapse);
            // ... and vice versa.
            SourceNeuron = null;

            // 2. Make its target neuron not aware of it ...
            TargetNeuron.SourceSynapses.Remove(synapse);
            // ... and vice versa.
            TargetNeuron = null;
        }

        public void Initialize()
        {
            Weight = StaticRandom.Double(-1, +1);
        }

        /// <summary>
        /// Jitters the synapse so that the neural network deviates from its local minimum position where further learning is of no use.
        /// </summary>
        /// <param name="jitterNoiseLimit">The maximum absolute jitter noise added.</param>
        public void Jitter(double jitterNoiseLimit)
        {
            Weight += StaticRandom.Double(-jitterNoiseLimit, +jitterNoiseLimit);
        }

        public override string ToString() => Weight.ToString("F2");
    }
}