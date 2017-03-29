using System;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class ActivationNeuron : NeuronBase, IActivationNeuron
    {
        internal ActivationNeuron()
        {
        }

        private new IActivationLayer Layer => (IActivationLayer)base.Layer;

        public virtual void Evaluate()
        {
            // Ref
            Input = SourceSynapses.Sum(s => s.SourceNeuron.Output * s.Weight);
            Output = Layer.ActivationFunction.Evaluate(Input);
        }

        /// <summary>
        /// Returns a string representation of the activation neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the activation neuron.
        /// </returns>
        public override string ToString()
        {
            var synapses = String.Join(", ", SourceSynapses);
            return $"AN([{synapses}]), {Input:F2}, {Output:F2})";
        }
    }
}