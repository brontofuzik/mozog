﻿using System;

namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public class DiscreteNeighbourhoodFunction
        : AbstractNeighbourhoodFunction
    {
        public override double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius)
        {
            return distanceBetweenOutputNeurons <= Math.Abs(neighbourhoodRadius) ? 1.0 : 0.0;
        }
    }
}
