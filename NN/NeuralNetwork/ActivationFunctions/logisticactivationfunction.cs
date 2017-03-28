using System;
using Mozog.Utils;

namespace NeuralNetwork.ActivationFunctions
{
    /// <remarks>
    /// <para>
    /// A logistic sigmoid activation function of a neuron.
    /// </para>
    /// <para>
    /// Definition: A <a href="http://en.wikipedia.org/wiki/Logistic_function">logistic</a>
    /// function or logistic curve is the most common sigmoid curve. It models the S-curve
    /// of growth of some set[1] P, where P might be thought of as population. The initial
    /// stage of growth is approximately exponential; then, as saturation begins, the growth
    /// slows, and at maturity, growth stops.
    /// </para>
    /// <para>
	/// A logistic sigmoid (or sometimes the standard sigmoid curve) is a function of the form:
	/// <code>
	///                   1
	/// f(x) = --------------------
	///          1 + e ^ (- gain * x)
    /// </code>
    /// where gain > 0.
	///
    /// <code>
	/// f'(x) = gain * f(x) * (1 - f(x))   
	/// </code>     
    /// </para>
    /// </remarks>
    public class LogisticActivationFunction : IDerivableActivationFunction
    {
        public LogisticActivationFunction(double gain)
        {
            Require.IsPositive(gain, nameof(gain));
            Gain = gain;
        }

        public LogisticActivationFunction()
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
    }
}
