using System;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationNeuron : NeuronBase, IActivationNeuron
    {
        #region Construction

        internal ActivationNeuron()
        {
        }

        protected override ISynapse MakeSynapse() => new Synapse();

        #endregion // Construction

        private new IActivationLayer Layer => (IActivationLayer)base.Layer;

        public virtual void Evaluate()
        {
            // Ref
            Input = SourceSynapses.Sum(s => s.SourceNeuron.Output * s.Weight);
            Output = Layer.ActivationFunction.Evaluate(Input);
        }

        // Jitter
        //public void Jitter(double noiseLimit)
        //{
        //    SourceSynapses.ForEach(s => s.Jitter(noiseLimit));
        //}

        public override string ToString()
        {
            var synapses = String.Join(", ", SourceSynapses);
            return $"AN([{synapses}]), {Input:F2}, {Output:F2})";
        }
    }
}