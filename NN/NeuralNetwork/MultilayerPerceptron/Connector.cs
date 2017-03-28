using System.Collections.Generic;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Connector : IConnector
    {
        public Connector(ILayer sourceLayer, IActivationLayer targetLayer, INetwork network)
        {
            SourceLayer = sourceLayer;
            TargetLayer = targetLayer;
            Network = network;
        }

        // Factory
        internal Connector()
        {
        }

        public List<ISynapse> Synapses { get; } = new List<ISynapse>();

        public int SynapseCount => Synapses.Count;

        public ILayer SourceLayer { get; set; }

        public ILayer TargetLayer { get; set; }

        public INetwork Network { get; set; }

        public void Connect()
        {
            foreach (var targetNeuron in TargetLayer.Ns)
            {
                foreach (var sourceNeuron in SourceLayer.Ns)
                {
                    var synapse = new Synapse(sourceNeuron, targetNeuron, this);
                    synapse.Connect();
                    Synapses.Add(synapse);
                }
            }
        }

        public void Initialize()
        {
            Synapses.ForEach(s => s.Initialize());
        }

        public void Jitter(double jitterNoiseLimit)
        {
            Synapses.ForEach(s => s.Jitter(jitterNoiseLimit));
        }
    }
}