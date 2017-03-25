using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Neurons
{
    public interface IActivationNeuron : INeuron
    {
        double Input { get; }

        List<ISynapse> SourceSynapses { get; }

        void Evaluate();
    }
}
