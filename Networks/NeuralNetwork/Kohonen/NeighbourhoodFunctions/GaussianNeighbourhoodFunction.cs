using System;
namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public class GaussianNeighbourhoodFunction
        : AbstractNeighbourhoodFunction
    {
        public override double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius)
        {
            return 1 * Math.Exp(-(distanceBetweenOutputNeurons * distanceBetweenOutputNeurons / (2 * neighbourhoodRadius * neighbourhoodRadius)));
        }
    }
}
