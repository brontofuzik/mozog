namespace NeuralNetwork.MultilayerPerceptron.ActivationFunctions
{
    public class LinearActivationFunction : IDerivableActivationFunction
    {
        public double Evaluate(double x) => x;

        public double EvaluateDerivative(double x) => 1.0;
    }
}
