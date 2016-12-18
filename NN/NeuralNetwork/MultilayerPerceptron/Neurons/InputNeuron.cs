using System;
using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;


namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// An input neuron.
    /// </summary>
    public class InputNeuron
        : INeuron
    {
        /// <summary>
        /// 
        /// </summary>
        private double output;
        
        /// <summary>
        /// 
        /// </summary>
        private List<ISynapse> targetSynapses;
        
        /// <summary>
        /// 
        /// </summary>
        private InputLayer parentLayer;

        /// <summary>
        /// Gets or sets the output of the neuron.
        /// </summary>
        /// 
        /// <value>
        /// The output of the neuron.
        /// </value>
        public double Output
        {
            get
            {
                return output;
            }
            set
            {
                output = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ISynapse> TargetSynapses
        {
            get
            {
                return targetSynapses;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ILayer ParentLayer
        {
            get
            {
                return parentLayer;
            }
            set
            {
                parentLayer = value as InputLayer;
            }
        }

        /// <summary>
        /// Creates a new input neuron.
        /// </summary>
        /// <param name="parentLayer">The parent layer.</param>
        public InputNeuron(InputLayer parentLayer)
        {
            targetSynapses = new List<ISynapse>();

            // Validate the parent layer.
            Utilities.RequireObjectNotNull(parentLayer, "parentLayer");
            this.parentLayer = parentLayer;
        }

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public void Initialize()
        {
            output = 0.0;
        }

        /// <summary>
        /// Returns a string representation of the input neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the input neuron.
        /// </returns>
        public override string ToString()
        {
            return String.Format("IN(" + output.ToString("F2")+ ")");
        }
    }
}
