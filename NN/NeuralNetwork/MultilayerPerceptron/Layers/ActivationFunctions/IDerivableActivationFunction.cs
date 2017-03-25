namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <summary>
    /// A derivable activation function of a neuron is a function that calculates the output (or state) of a neuron from its input (or inner potential).
    /// </summary>
    public interface IDerivableActivationFunction : IActivationFunction
    {
        double EvaluateDerivative(double x);
    }
}
