using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    /// <summary>
    /// An interface of an activation neuron.
    /// </summary>
    public interface IActivationNeuron
        : INeuron
    {
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

        /// <summary>
        /// Evaluates the neuron.
        /// </summary>
        void Evaluate();
    }
}
