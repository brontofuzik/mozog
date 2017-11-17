using System.Collections.Generic;
using NeuralNetwork.MLP.ActivationFunctions;

namespace NeuralNetwork.MLP
{
    public interface ILayer
    {
        IEnumerable<INeuron> Neurons { get; }

        int NeuronCount { get; }

        INetwork Network { get; set; }

        double[] Input { get; }

        double[] Output { get; set; }

        void Connect(ILayer layer);
    }

    public interface ILayer<TNeuron> : ILayer
        where TNeuron : INeuron
    {
        new IEnumerable<TNeuron> Neurons { get; }
    }

    public interface IInputLayer : ILayer<IInputNeuron>
    {
        //void SetOutput(double[] outputVector);
    }

    public interface IActivationLayer : ILayer<IActivationNeuron>
    {
        //IActivationFunction Activation { get; }

        IActivationFunction1 ActivationFunc1 { get; }

        IActivationFunction2 ActivationFunc2 { get; }

        void Evaluate();
    }
}
