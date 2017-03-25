using System;

namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <remarks>
    /// http://en.wikipedia.org/wiki/Hyperbolic_tangent</c>
    /// </remarks>
    public class HyperbolicTangentActivationFunction : IDerivableActivationFunction
    {
        public double Evaluate(double x) => Math.Tanh(x);

        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return 1 - Math.Pow(y, 2);
        }
    }
}
