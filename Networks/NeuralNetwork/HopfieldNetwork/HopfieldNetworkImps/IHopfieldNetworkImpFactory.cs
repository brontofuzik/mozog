namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps
{
    public interface IHopfieldNetworkImpFactory
    {
        IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction);
    }
}
