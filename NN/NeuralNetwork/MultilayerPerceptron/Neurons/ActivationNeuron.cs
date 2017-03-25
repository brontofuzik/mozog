using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// A hidden neuron.
    /// </summary>
    public class ActivationNeuron : IActivationNeuron
    {
        private IActivationLayer parentLayer;

        public ActivationNeuron(IActivationLayer parentLayer)
        {
            Require.IsNotNull(parentLayer, nameof(parentLayer));
            this.parentLayer = parentLayer;
        }

        public double Input { get; private set; }

        public double Output { get; private set; }

        public List< ISynapse > SourceSynapses { get; } = new List<ISynapse>();

        public List< ISynapse > TargetSynapses { get; } = new List<ISynapse>();

        public ILayer ParentLayer
        {
            get { return parentLayer; }
            set { parentLayer = value as IActivationLayer; }
        }

        public void Initialize()
        {
            Input = 0.0;
            Output = 0.0;
        }

        public virtual void Evaluate()
        {
            // Ref
            Input = SourceSynapses.Sum(s => s.SourceNeuron.Output * s.Weight);
            Output = parentLayer.ActivationFunction.Evaluate(Input);
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