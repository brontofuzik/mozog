using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public abstract class LayerBase<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        // Factory
        internal LayerBase(NetworkArchitecture.Layer layer)
        {
            layer.Neurons.Times(() => AddNeuron(MakeNeuron()));
        }

        protected void AddNeuron(TNeuron neuron)
        {
            Neurons.Add(neuron);
            neuron.Layer = this;
        }

        protected abstract TNeuron MakeNeuron();

        public IList<TNeuron> Neurons { get; } = new List<TNeuron>();

        public IEnumerable<INeuron> Neurons_Untyped => Neurons.AsEnumerable().Cast<INeuron>();

        public int NeuronCount => Neurons.Count;

        public List<IConnector> SourceConnectors { get; } = new List<IConnector>();

        public List<IConnector> TargetConnectors { get; } = new List<IConnector>();

        public INetwork Network { get; set; }

        public virtual void Initialize()
        {
            // Override if necessary.
        }
    }
}
