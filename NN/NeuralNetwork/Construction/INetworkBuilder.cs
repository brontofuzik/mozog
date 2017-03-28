using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Construction
{
    interface INetworkBuilder
    {
        INetwork Build(NetworkArchitecture architecture, INetworkFactory factory);
    }
}
