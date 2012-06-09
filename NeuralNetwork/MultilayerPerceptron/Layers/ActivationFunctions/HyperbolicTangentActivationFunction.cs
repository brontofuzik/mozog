using System;

namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    /// <remarks>
    /// A hyperbolic tangent activation function of a neuron.
    /// See <c>http://en.wikipedia.org/wiki/Hyperbolic_tangent</c> for further details. 
    /// </remarks>
    public class HyperbolicTangentActivationFunction :
        IDerivableActivationFunction
    {
        #region Public instance methods

        /// <summary>
        /// Evaluates the activation fuction for the input (or inner potential) of a neuron.
        /// </summary>
        /// <param name="x">The input (or inner potential) of a neuron.</param>
        /// <returns></returns>
        public double Evaluate( double x )
        {
            return Math.Tanh( x );
        }

        /// <summary>
        /// Evaluates the activation function's derivative for the input (or inner potential) of a neuron. 
        /// </summary>
        /// <param name="x">The input (or inner potential) of a neuron.</param>
        /// <returns></returns>
        public double EvaluateDerivative( double x )
        {
            double y = Evaluate( x );
            return (1 - Math.Pow( y, 2 ));
        }

        #endregion // Public instance methods
    }
}
