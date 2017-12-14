using System;
using static Mozog.Utils.Math.Math;

namespace NeuralNetwork.Kohonen.NeighbourhoodFunctions
{
    public class GaussianNeighbourhoodFunction : INeighbourhoodFunction
    {
        public double Evaluate(double distance, double radius)
            => Math.Exp(-Square(distance / (2 * Square(radius))));
    }
}
