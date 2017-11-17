using System;
namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public class GaussianNeighbourhoodFunction : INeighbourhoodFunction
    {
        public double Evaluate(double distance, double radius)
            => 1 * Math.Exp(-(distance * distance / (2 * radius * radius)));
    }
}
