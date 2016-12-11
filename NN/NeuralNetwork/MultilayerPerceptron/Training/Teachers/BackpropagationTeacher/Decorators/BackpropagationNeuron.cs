using System;

using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Layers.ActivationFunctions;
using NeuralNetwork.MultilayerPerceptron.Neurons;


namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators
{
    /// <summary>
    /// A backpropagation neuron.
    /// </summary>
    class BackpropagationNeuron
        : ActivationNeuronDecorator
    {
        #region Public members

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="neuron">The (activation) neuron to be decorated as backpropagation (activation) neuron.</param>
        /// <param name="parentLayer">The parent layer.</param>
        public BackpropagationNeuron(IActivationNeuron activationNeuron, IActivationLayer parentLayer)
            : base(activationNeuron, parentLayer)
        {
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _partialDerivative = 0.0;
            _error = 0.0;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate(double desiredOutput)
        {
            _partialDerivative = (Output - desiredOutput);
            _error =  _partialDerivative * ActivationFunctionDerivative;
        }

        // Replaces three steps - (b), (c) and (d) - with one.
        public void Backpropagate()
        {
            _partialDerivative = 0.0;
            foreach (BackpropagationSynapse targetSynapse in TargetSynapses)
            {
                BackpropagationNeuron targetNeuron = targetSynapse.TargetNeuron as BackpropagationNeuron;
                _partialDerivative += targetNeuron.Error * targetSynapse.Weight;
            }

            _error = _partialDerivative * ActivationFunctionDerivative;
        }

        /// <summary>
        /// Returns a string representation of the backpropagation neuron.
        /// </summary>
        /// <returns>
        /// A string representation of the backpropagation neuron.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        #endregion // Public instance methods

        #region Public instance properties

        /// <summary>
        /// Gets the partial derivative of the neuron.
        /// </summary>
        public double PartialDerivative
        {
            get
            {
                return _partialDerivative;
            }
        }

        /// <summary>
        /// Gets the error of the neuron.
        /// </summary>
        public double Error
        {
            get
            {
                return _error;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double ActivationFunctionDerivative
        {
            get
            {
                return ((ParentLayer as IActivationLayer).ActivationFunction as IDerivableActivationFunction).EvaluateDerivative(Input);
            }
        }

        #endregion // Public instance properties

        #endregion // Public members

        #region Private members

        #region Private instance fields

        /// <summary>
        /// The partial derivative of the network error function with respect to the neuron.
        /// </summary>
        private double _partialDerivative;

        /// <summary>
        /// The error of the neuron.
        /// </summary>
        private double _error;

        #endregion // Private instance fields

        #endregion // Private members
    }
}