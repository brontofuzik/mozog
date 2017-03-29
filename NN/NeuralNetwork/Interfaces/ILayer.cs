using System.Collections.Generic;
using NeuralNetwork.ActivationFunctions;

namespace NeuralNetwork.Interfaces
{
    public interface ILayer
    {
        IEnumerable<INeuron> Neurons_Untyped { get; }

        INetwork Network { get; set; }

        void Initialize();
    }

    public interface ILayer<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        IList<TNeuron> Neurons_Typed { get; }

        int NeuronCount { get; }

        List<IConnector> SourceConnectors { get; }

        List<IConnector> TargetConnectors { get; }
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
