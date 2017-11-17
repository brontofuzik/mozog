namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public interface INeighbourhoodFunction
    {
        double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius);
    }
}
