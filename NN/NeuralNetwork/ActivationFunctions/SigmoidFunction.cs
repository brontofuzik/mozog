using System;

namespace NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Sigmoid_function
    /// </summary>
    public class SigmoidFunction : IDifferentiableActivationFunction1
    {
        public double Evaluate(double x) => 1 / (1 + Math.Exp(-x));

        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return y * (1 - y);
        }

        public override string ToString() => "Sig";
    }
}
