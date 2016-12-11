using System;

namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public class DiscreteNeighbourhoodFunction
        : AbstractNeighbourhoodFunction
    {
        #region Public members

        #region Instance methods

        public override double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius)
        {
            return distanceBetweenOutputNeurons <= Math.Abs(neighbourhoodRadius) ? 1.0 : 0.0;
        }

        #endregion // Instance methods

        #endregion Public members
    }
}
