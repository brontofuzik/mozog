using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Construction
{
    public interface INetworkFactory
    {
        INetwork MakeNetwork();

        IInputLayer MakeInputLayer();

        IActivationLayer MakeActivationLayer();

        IInputNeuron MakeInputNeuron();

        IActivationNeuron MakeActivationNeuron();

        IConnector MakeConnector();

        ISynapse MakeSynapse();
    }
}
