using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp
{
    public class FullHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        #region Public members

        #region Instance methods

        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            #region Preconditions

            // The number of neurons must be positive.
            Utilities.RequireNumberPositive(neuronCount, "neuronCount");

            // The activation function must be provided.
            Utilities.RequireObjectNotNull(activationFunction, "activationFunction");

            #endregion // Preconditions

            return new FullHopfieldNetworkImp(neuronCount, activationFunction);
        }

        #endregion // Instance methods

        #endregion // Public members
    }
}
