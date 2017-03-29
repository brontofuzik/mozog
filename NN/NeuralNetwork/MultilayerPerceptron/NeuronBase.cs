using System.Collections.Generic;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public abstract class NeuronBase : INeuron
    {
        public ILayer Layer { get; set; }

        public double Input { get; protected set; }

        public double Output { get; set; }

        public List<ISynapse> SourceSynapses { get; } = new List<ISynapse>();

        public List<ISynapse> TargetSynapses { get; } = new List<ISynapse>();

        public virtual void Initialize()
        {
            Input = 0.0;
            Output = 0.0;
        }
    }
}
