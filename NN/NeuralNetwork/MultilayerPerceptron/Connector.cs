using System.Collections.Generic;
using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class Connector : IConnector
    {
        private readonly NetworkArchitecture.Connector blueprint;

        internal Connector(NetworkArchitecture.Connector blueprint)
        {
            this.blueprint = blueprint;
        }

        public List<ISynapse> Synapses { get; } = new List<ISynapse>();

        public int SynapseCount => Synapses.Count;

        public ILayer SourceLayer { get; set; }

        public ILayer TargetLayer { get; set; }

        public INetwork Network { get; set; }

        public void Connect()
        {
            foreach (var targetNeuron in TargetLayer.Neurons_Untyped)
            {
                foreach (var sourceNeuron in SourceLayer.Neurons_Untyped)
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