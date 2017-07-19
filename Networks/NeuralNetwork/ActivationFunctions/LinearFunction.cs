using System;

namespace NeuralNetwork.ActivationFunctions
{
    public class LinearFunction : IDifferentiableActivationFunction1
    {
        public double Evaluate(double x) => x;

        public double EvaluateDerivative(double x) => 1.0;

        public override string ToString() => "Lin";
    }

    public class ReLU : IDifferentiableActivationFunction1
    {
        public double Evaluate(double x) => Math.Max(0, x);

        public double EvaluateDerivative(double x) => x >= 0 ? 1 : 0;
    }

    public class Softplus : IDifferentiableActivationFunction1
    {
        public double Evaluate(double x) => Math.Log(1 + Math.Exp(x));

        public double EvaluateDerivative(double x) => 1 / (1 + Math.Exp(x));
    }
}
