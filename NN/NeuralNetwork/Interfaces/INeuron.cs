using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface INeuron
    {
        ILayer Layer { get; set; }

        double Input { get; }

        double Output { get; set; }

        List<ISynapse> SourceSynapses { get; }

        List<ISynapse> TargetSynapses { get; }

        void Initialize();
    }

    public interface IActivationNeuron : INeuron
    {
        void Evaluate();
    }

    public interface IInputNeuron : INeuron
    {
    }
}
