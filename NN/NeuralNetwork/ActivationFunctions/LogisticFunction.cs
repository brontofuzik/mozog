using System;
using Mozog.Utils;

namespace NeuralNetwork.ActivationFunctions
{
    /// <summary>
    /// http://en.wikipedia.org/wiki/Logistic_function">
    /// </summary>
    public class LogisticFunction : IDifferentiableActivationFunction
    {
        public LogisticFunction(double gain)
        {
            Require.IsPositive(gain, nameof(gain));
            Gain = gain;
        }

        public LogisticFunction()
            : this(1.0)
        {
        }

        public double Gain { get; }

        public double Evaluate(double x) => 1 / (1 + Math.Exp(-Gain * x));

        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return Gain * y * (1 - y);
        }

        public override string ToString() => "Log";
    }
}
