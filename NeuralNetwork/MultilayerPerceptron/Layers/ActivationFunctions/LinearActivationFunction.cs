namespace NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions
{
    public class LinearActivationFunction
        : IDerivableActivationFunction
    {
        #region Public instance methods

        /// <summary>
        /// Evaluates the activation fuction for the input (the inner potential) of a neuron.
        /// </summary>
        /// <param name="x">The input (the inner potential) of a neuron.</param>
        /// <returns>
        /// The output (the state) of the neuron.
        /// </returns>
        public double Evaluate( double x )
        {
            return x;
        }


        /// <summary>
        /// Evaluates the activation function's derivative for the input (the inner potential) of a neuron. 
        /// </summary>
        /// <param name="x">The input (the inner potential) of a neuron.</param>
        /// <returns>
        /// </returns>
        public double EvaluateDerivative( double x )
        {
            return 1.0;
        }

        #endregion // Public instance methods
    }
}
