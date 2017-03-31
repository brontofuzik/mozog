using System.Collections.Generic;

namespace NeuralNetwork.Interfaces
{
    public interface INeuron
    {
        ILayer Layer { get; set; }

        double Input { get; }

        double Output { get; set; }

        List<ISynapse> SourceSynapses { get; }

        // TODO Move to BackpropagationNeuron?
        List<ISynapse> TargetSynapses { get; }

        void Connect(INeuron sourceNeuron);

        void Initialize();
    }

    public interface IInputNeuron : INeuron
    {
    }

    public interface IActivationNeuron : INeuron
    {
        void Evaluate();

        // TODO Jitter
        //void Jitter(double noiseLimit);
    }
}
