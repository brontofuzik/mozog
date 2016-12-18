using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.BackpropagationTeacher.Decorators
{
    /// <summary>
    /// A backpropagation synapse.
    /// </summary>
    class BackpropagationSynapse
        : SynapseDecorator
    {
        /// <summary>
        /// Creates a new backpropagation synapse by decorating a synapse.
        /// </summary>
        /// <param name="synapse">The synapse to be doecorated as backpropagation synapse.</param>
        public BackpropagationSynapse(ISynapse synapse,IConnector parentConnector)
            : base(synapse, parentConnector)
        {
            _k = 1.01;
        }

        /// <summary>
        /// Connects the (backpropagation) synapse.
        /// 
        /// A synapse is said to be connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        public override void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// Disconnects the (backpropagation) synapse.
        /// 
        /// A synapse is said to be disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        public override void Disconnect()
        {
            Disconnect(this);
        }

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _partialDerivative = 0.0;
            _weightChange = 0.0;
            _previousWeightChange = 0.0;
        }

        /// <summary>
        /// Sets the learning rate of the synapse.
        /// </summary>
        /// <param name="learningRate"></param>
        public void SetLearningRate(double learningRate)
        {
            _learningRate = learningRate;
        }

        /// <summary>
        /// Resets the partial derivative of the synapse.
        /// </summary>
        public void ResetPartialDerivative()
        {
            _partialDerivative = 0.0;
        }

        /// <summary>
        /// Updates the partial derivative of the synapse using the source neuron output and the target neuron error.
        /// </summary>
        public void UpdatePartialDerivative()
        {
            BackpropagationNeuron targetNeuron = TargetNeuron as BackpropagationNeuron;
            _partialDerivative += SourceNeuron.Output * targetNeuron.Error;
        }

        /// <summary>
        /// Updates the weight of the synapse using the partial derivative.
        /// </summary>
        public void UpdateWeight()
        {
            // Update the previous weight change and the current weight change.
            _previousWeightChange = _weightChange;
            _weightChange = -_learningRate * _partialDerivative;

            // Update the weight.
            Weight += _weightChange + (ParentConnector as BackpropagationConnector).Momentum * _previousWeightChange;

            // Update the learning rate.

        }

        /// <summary>
        /// Updates the learning rate of the synapse using the previous and the current weight changes.
        /// </summary>
        public void UpdateLearningRate()
        {
            if (_previousWeightChange * _weightChange > 0)
            {
                _learningRate *= _k;
            }
            else
            {
                _learningRate /= 2.0;
            }
        }

        /// <summary>
        /// Returns a string representation of the backpropagation synapse.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the backpropagation synapse.
        /// </returns>
        public override string ToString()
        {
            return "BP" + base.ToString();
        }

        /// <summary>
        /// The partial derivative of the error function with respect to the synapse.
        /// </summary>
        private double _partialDerivative;

        /// <summary>
        /// The change of weight.
        /// </summary>
        private double _weightChange;

        /// <summary>
        /// The previous change of weight (used with momentum modification).
        /// </summary>
        private double _previousWeightChange;

        /// <summary>
        /// The learning rate of the synapse.
        /// </summary>
        private double _learningRate;

        /// <summary>
        /// 
        /// </summary>
        private double _k;
    }
}