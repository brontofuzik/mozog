using System;

namespace NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// https://en.wikipedia.org/wiki/Logistic_function
    /// </summary>
    public class LogisticFunction : IDifferentiableActivationFunction
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
            double ex = Math.Exp(x);
            return ex / (1 + ex);
        }

        public override string ToString() => "Log";
    }
}
