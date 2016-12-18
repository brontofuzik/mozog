namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <summary>
    /// <para>
    /// An interface of a derivable activation function of a neuron.
    /// </para>
    /// <para>
    /// Definition: An derivable activation function of a neuron is a function that calculates the output (or state) of a neuron from its input (or inner potential).
    /// </para>
    /// </summary>
    public interface IDerivableActivationFunction
        : IActivationFunction
    {
        #region Methods

        /// <summary>
        /// Evaluates the activation function's derivative for the input (the inner potential) of a neuron. 
        /// </summary>
        /// <param name="x">The input (the inner potential) of a neuron.</param>
        /// <returns>
        /// </returns>
        double EvaluateDerivative(double x);

        #endregion // Methods
    }
}
