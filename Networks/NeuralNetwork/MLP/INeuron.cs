using System.Collections.Generic;

namespace NeuralNetwork.MLP
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
    }

    public interface IInputNeuron : INeuron
    {
    }

    public interface IActivationNeuron : INeuron
    {
        void Evaluate();

        void EvaluateInput();

        void EvaluateOutput();
    }
}
