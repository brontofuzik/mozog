namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public interface INeighbourhoodFunction
    {
        double Evaluate(double distance, double radius);
    }
}
