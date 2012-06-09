namespace NeuralNetwork.HopfieldNetwork
{
    /// <summary>
    /// The activation function of the Hopfield neuron.
    /// </summary>
    /// <param name="input">The input of the Hopfield neuron.</param>
    /// <param name="evalautionProgressRatio">The progress ratio of the evaluation.</param>
    /// <returns>The output of the Hopfield neuron.</returns>
    public delegate double ActivationFunction(double input, double evalautionProgressRatio);
}
