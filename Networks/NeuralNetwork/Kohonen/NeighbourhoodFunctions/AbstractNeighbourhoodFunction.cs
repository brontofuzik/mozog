namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public abstract class AbstractNeighbourhoodFunction
        : INeighbourhoodFunction
    {
        public abstract double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius);    

        protected AbstractNeighbourhoodFunction()
        {
        }
    }
}
