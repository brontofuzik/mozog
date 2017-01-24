using Mozog.Utils;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    public class SparseHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            Require.IsPositive(neuronCount, nameof(neuronCount));
            Require.IsNotNull(activationFunction, nameof(activationFunction));

            return new SparseHopfieldNetworkImp(neuronCount, activationFunction);
        }
    }
}
