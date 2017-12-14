using System;

namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public class BubbleNeighbourhoodFunction : INeighbourhoodFunction
    {
        public double Evaluate(double distance, double radius)
            => distance <= Math.Abs(radius) ? 1.0 : 0.0;
    }
}
