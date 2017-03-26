using System.Collections.Generic;
using NeuralNetwork.MultilayerPerceptron.ActivationFunctions;

namespace NeuralNetwork.Interfaces
{
    public interface ILayer
    {
        IEnumerable<INeuron> Ns { get; }

        void Initialize();
    }

    public interface ILayer<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        IList<TNeuron> Neurons { get; }

        int NeuronCount { get; }

        List<IConnector> SourceConnectors { get; }

        List<IConnector> TargetConnectors { get; }

        INetwork Network { get; set; }
    }

    public interface IActivationLayer : ILayer<IActivationNeuron>
    {
        IActivationFunction ActivationFunction { get; }

        void Evaluate();

        double[] GetOutputVector();
    }

    public interface IInputLayer : ILayer<IInputNeuron>
    {
        void SetOutputVector(double[] outputVector);
    }
}
