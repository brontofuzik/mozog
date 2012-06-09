namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public interface INeighbourhoodFunction
    {
        #region Methods

        double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius);

        #endregion // Methods
    }
}
