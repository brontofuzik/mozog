namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    public class SparseHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            Utilities.RequireNumberPositive(neuronCount, nameof(neuronCount));
            Utilities.RequireObjectNotNull(activationFunction, nameof(activationFunction));

            return new SparseHopfieldNetworkImp(neuronCount, activationFunction);
        }
    }
}
