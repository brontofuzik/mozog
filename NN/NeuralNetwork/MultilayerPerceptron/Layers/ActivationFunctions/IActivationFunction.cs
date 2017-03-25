namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <summary>
    /// An activation function of a neuron is a function that calculates the output (or state) of a neuron.
    /// </summary>
    public interface IActivationFunction
    {
        double Evaluate(double x);
    }
}
