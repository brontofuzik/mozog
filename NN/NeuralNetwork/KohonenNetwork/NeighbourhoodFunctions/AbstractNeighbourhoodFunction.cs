namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public abstract class AbstractNeighbourhoodFunction
        : INeighbourhoodFunction
    {
        #region Public members

        public abstract double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius);

        #endregion // Public members

        
        #region Protected members

        protected AbstractNeighbourhoodFunction()
        {
        }

        #endregion // Protected members
    }
}
