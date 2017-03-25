using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Layers
{
    public interface IActivationLayer : ILayer
    {
        new List<IActivationNeuron> Neurons { get; }

        IActivationFunction ActivationFunction { get; }

        List<IConnector> SourceConnectors { get; }
        
        void Evaluate();

        double[] GetOutputVector();
    }
}
