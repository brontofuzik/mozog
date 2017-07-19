using System;

namespace NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Logistic_function
    /// </summary>
    public class LogisticFunction : IDifferentiableActivationFunction1
    {
        // Maximum value
        private readonly double L;

        // Steepness/gain
        private readonly double k;

        // Midpoint
        private readonly double x0;

        public LogisticFunction(double L = 1.0, double k = 1.0, double x0 = 0.0)
        {
            this.L = L;
            this.k = k;
            this.x0 = x0;
        }

        public double Evaluate(double x) => L / (1 + Math.Exp(-k * (x - x0)));

        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return y * (1 - y);
        }

        public override string ToString() => "Log";
    }

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
