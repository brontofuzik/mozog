﻿namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    public class SparseHopfieldNetworkImpFactory
        : IHopfieldNetworkImpFactory
    {
        public IHopfieldNetworkImp CreateHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            #region Preconditions

            // The number of neurons must be positive.
            Utilities.RequireNumberPositive(neuronCount, "neuronCount");

            // The activation function must be provided.
            Utilities.RequireObjectNotNull(activationFunction, "activationFunction");

            #endregion // Preconditions

            return new SparseHopfieldNetworkImp(neuronCount, activationFunction);
        }
    }
}
