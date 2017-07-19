namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public interface INeighbourhoodFunction
    {
        double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius);
    }
}
