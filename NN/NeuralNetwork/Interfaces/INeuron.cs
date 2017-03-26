using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface INeuron
    {
        double Input { get; }

        double Output { get; set; }

        List<ISynapse> SourceSynapses { get; }

        List<ISynapse> TargetSynapses { get; }

        void Initialize();
    }

    public interface IActivationNeuron : INeuron
    {
        IActivationLayer Layer { get; set; }

        void Evaluate();
    }

    public interface IInputNeuron : INeuron
    {
        IInputLayer Layer { get; set; }
    }
}
