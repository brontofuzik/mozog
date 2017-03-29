using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Synapse : ISynapse
    {
        public Synapse(INeuron sourceNeuron, INeuron targetNeuron, IConnector connector)
        {
            SourceNeuron = sourceNeuron;
            TargetNeuron = targetNeuron;
            Connector = connector;
        }

        // Factory
        internal Synapse()
        {
        }

        public double Weight { get; set; }

        public INeuron SourceNeuron { get; set; }

        public INeuron TargetNeuron { get; set; }

        public IConnector Connector { get; set; }

        public void Connect()
        {
            SourceNeuron.TargetSynapses.Add(this);
            TargetNeuron.SourceSynapses.Add(this);
        }

        public virtual void Initialize()
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