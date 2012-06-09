using System.Collections.Generic;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// An interface of an activation neuron.
    /// </summary>
    public interface IActivationNeuron
        : INeuron
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        double Input
        {
            get;
        }

        List<ISynapse> SourceSynapses
        {
            get;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Evaluates the neuron.
        /// </summary>
        void Evaluate();

        #endregion // Methods
    }
}
