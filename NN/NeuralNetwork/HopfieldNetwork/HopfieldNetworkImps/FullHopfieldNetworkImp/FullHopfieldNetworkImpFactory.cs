namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp
{
    public class FullHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            Utilities.RequireNumberPositive(neuronCount, nameof(neuronCount));
            Utilities.RequireObjectNotNull(activationFunction, nameof(activationFunction));

            return new FullHopfieldNetworkImp(neuronCount, activationFunction);
        }
    }
}
