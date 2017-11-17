﻿using System.Collections.Generic;

namespace NeuralNetwork.MLP
{
    public abstract class NeuronBase : INeuron
    {
        public ILayer Layer { get; set; }

        public double Input { get; protected set; }

        public double Output { get; set; }

        public List<ISynapse> SourceSynapses { get; } = new List<ISynapse>();

        // TODO Move to BackpropagationNeuron?
        public List<ISynapse> TargetSynapses { get; } = new List<ISynapse>();

        public void Connect(INeuron sourceNeuron)
        {
            var synapse = MakeSynapse();
            AddSynapse(synapse, sourceNeuron);
        }

        protected abstract ISynapse MakeSynapse();

        private void AddSynapse(ISynapse synapse, INeuron sourceNeuron)
        {
            SourceSynapses.Add(synapse);
            synapse.TargetNeuron = this;

            sourceNeuron.TargetSynapses.Add(synapse);
            synapse.SourceNeuron = sourceNeuron;
        }
    }
}
