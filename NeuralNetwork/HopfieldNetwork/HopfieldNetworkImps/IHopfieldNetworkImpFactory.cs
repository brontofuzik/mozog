namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps
{
    public interface IHopfieldNetworkImpFactory
    {
        #region Methods

        IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction);

        #endregion // Methods
    }
}
