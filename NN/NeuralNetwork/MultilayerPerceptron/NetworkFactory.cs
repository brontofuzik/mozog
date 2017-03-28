using NeuralNetwork.Construction;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.MultilayerPerceptron
{
    public class NetworkFactory : INetworkFactory
    {
        public INetwork MakeNetwork() => new Network();

        public IInputLayer MakeInputLayer() => new InputLayer();

        public IActivationLayer MakeActivationLayer() => new ActivationLayer();

        public IInputNeuron MakeInputNeuron() => new InputNeuron();

        public IActivationNeuron MakeActivationNeuron() => new ActivationNeuron();

        public IConnector MakeConnector() => new Connector();

        public ISynapse MakeSynapse() => new Synapse();
    }
}
