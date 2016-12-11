using System;
namespace NeuralNetwork.KohonenNetwork.NeighbourhoodFunctions
{
    public class GaussianNeighbourhoodFunction
        : AbstractNeighbourhoodFunction
    {
        #region Public members

        #region Instance methods

        public override double CalculateNeighbourhood(double distanceBetweenOutputNeurons, double neighbourhoodRadius)
        {
            return 1 * Math.Exp(-((distanceBetweenOutputNeurons * distanceBetweenOutputNeurons) / (2 * neighbourhoodRadius * neighbourhoodRadius)));
        }

        #endregion // Instance methods

        #endregion // Public members
    }
}
