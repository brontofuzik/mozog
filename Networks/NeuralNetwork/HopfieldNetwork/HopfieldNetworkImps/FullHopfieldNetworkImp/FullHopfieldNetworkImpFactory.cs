using Mozog.Utils;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp
{
    public class FullHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            Require.IsPositive(neuronCount, nameof(neuronCount));
            Require.IsNotNull(activationFunction, nameof(activationFunction));

            return new FullHopfieldNetworkImp(neuronCount, activationFunction);
        }
    }
}
