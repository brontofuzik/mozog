namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <summary>
    /// <para>
    /// An interface of an activation function of a neuron.
    /// </para>
    /// <para>
    /// Definition: An activation function of a neuron is a function that calculates the output (or state) of a neuron.
    /// </para>
    /// </summary>
    public interface IActivationFunction
    {
        #region Methods

        /// <summary>
        /// Evaluates the activation fuction for the input (the inner potential) of a neuron.
        /// </summary>
        /// <param name="x">The input (the inner potential) of a neuron.</param>
        /// <returns>
        /// The output (the state) of the neuron.
        /// </returns>
        double Evaluate( double x );

        #endregion // Methods
    }
}
