using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public abstract class LayerBase<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        private readonly NetworkArchitecture.Layer layerPlan;

        #region Construction

        internal LayerBase(NetworkArchitecture.Layer layerPlan)
        {
            this.layerPlan = layerPlan;
            layerPlan.Neurons.Times(() => AddNeuron(MakeNeuron()));
        }

        public void Connect(ILayer sourceLayer)
        {
            foreach (var neuron in Neurons_Untyped)
            {
                foreach (var sourceNeuron in sourceLayer.Neurons_Untyped)
                {
                    neuron.Connect(sourceNeuron);
                }
            }
        }

        protected abstract TNeuron MakeNeuron();

        private void AddNeuron(TNeuron neuron)
        {
            Neurons.Add(neuron);
            neuron.Layer = this;
        }

        #endregion // Construction

        public IList<TNeuron> Neurons { get; } = new List<TNeuron>();

        public IEnumerable<INeuron> Neurons_Untyped => Neurons.AsEnumerable().Cast<INeuron>();

        public int NeuronCount => Neurons.Count;

        public INetwork Network { get; set; }

        public virtual void Initialize()
        {
            Neurons_Untyped.ForEach(n => n.Initialize());
        }
    }
}
