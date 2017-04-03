using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public abstract class LayerBase<TNeuron> : ILayer<TNeuron>
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
            foreach (var neuron in Neurons)
            {
                foreach (var sourceNeuron in sourceLayer.Neurons)
                {
                    neuron.Connect(sourceNeuron);
                }
            }
        }

        protected abstract TNeuron MakeNeuron();

        private void AddNeuron(TNeuron neuron)
        {
            NeuronList.Add(neuron);
            neuron.Layer = this;
        }

        #endregion // Construction

        protected IList<TNeuron> NeuronList { get; } = new List<TNeuron>();

        public IEnumerable<TNeuron> Neurons => NeuronList;

        IEnumerable<INeuron> ILayer.Neurons => Neurons.Cast<INeuron>();

        public int NeuronCount => NeuronList.Count;

        public INetwork Network { get; set; }

        public virtual void Initialize()
        {
            NeuronList.ForEach(n => n.Initialize());
        }

        public override string ToString()
            => $"[\n{String.Join(",\n", NeuronList.Select((n, i) => $"\t\t{i}: {n}"))}\n\t]";
    }
}
