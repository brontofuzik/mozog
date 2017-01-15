using System;
using NeuralNetwork.Utils;


namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
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
    public class LogisticActivationFunction
        : IDerivableActivationFunction
    {
        /// <summary>
        /// The gain.
        /// </summary>
        private double gain;

        /// <summary>
        /// Gets the gain.
        /// </summary>
        /// <value>
        /// The gain.
        /// </value>
        public double Gain
        {
            get
            {
                return gain;
            }
        }
        
        /// <summary>
        /// Creates a new logistic sigmoid activation function.
        /// </summary>
        /// <param name="gain">The gain.</param>
        /// <exception name="System.ArgumentException">
        /// Condition: <c>gain</c> is less than or equal to zero.
        /// </exception>
        public LogisticActivationFunction(double gain)
        {
            // Validate the arguments.
            Require.IsPositive(gain, "gain");

            // Initialize the instance fields.
            this.gain = gain;
        }

        /// <summary>
        /// Creates a new logistic sigmoid activation fucntion.
        /// </summary>
        public LogisticActivationFunction()
            : this(1.0)
        {
        }

        /// <summary>
        /// Evaluates the activation fuction for the input (or inner potential) of a neuron.
        /// </summary>
        /// <param name="x">The input (or inner potential) of a neuron.</param>
        /// <returns></returns>
        public double Evaluate(double x)
        {
            return 1 / (1 + Math.Exp(-gain * x));
        }

        /// <summary>
        /// Evaluates the activation function's derivative for the input (or inner potential) of a neuron. 
        /// </summary>
        /// <param name="x">The input (or inner potential) of a neuron.</param>
        /// <returns></returns>
        public double EvaluateDerivative(double x)
        {
            double y = Evaluate(x);
            return gain * y * (1 - y);
        }
    }
}
