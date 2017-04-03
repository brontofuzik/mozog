using System.Collections.Generic;
using NeuralNetwork.ActivationFunctions;

namespace NeuralNetwork.Interfaces
{
    public interface ILayer
    {
        IEnumerable<INeuron> Neurons_Untyped { get; }

        int NeuronCount { get; }

        INetwork Network { get; set; }

        void Connect(ILayer layer);

        void Initialize();
    }

    public interface ILayer<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        IEnumerable<TNeuron> Neurons_Typed { get; }
    }

    public interface IInputLayer : ILayer<IInputNeuron>
    {
        void SetOutput(double[] outputVector);
    }

    public interface IActivationLayer : ILayer<IActivationNeuron>
    {
        IActivationFunction ActivationFunction { get; }

        void Evaluate();

        // TODO Jitter
        //void Jitter(double noiseLimit)

        double[] GetOutput();
    }
}
