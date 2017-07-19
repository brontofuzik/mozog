namespace NeuralNetwork.Interfaces
{
    internal interface INetworkSerializer
    {
        void Serialize(INetwork network, string fileName);

        INetwork Deserialize(string fileName);
    }
}
