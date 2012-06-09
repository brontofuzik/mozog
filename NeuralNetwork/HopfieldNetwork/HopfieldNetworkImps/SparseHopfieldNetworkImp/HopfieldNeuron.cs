using System.Collections.Generic;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    class HopfieldNeuron
    {
        #region Public members

        #region Instance constructors

        /// <summary>
        /// Initializes a new instance of the Neuron class.
        /// </summary>
        /// <param name="index">The index of the neuron.</param>
        /// <param name="activationFunction">The activation function.</param>
        public HopfieldNeuron(int index, ActivationFunction activationFunction)
        {
            #region Preconditions

            // The index must be non-negative.
            Utilities.RequireNumberNonNegative(index, "index");

            // The activation function must be provided.
            Utilities.RequireObjectNotNull(activationFunction, "activationFunction");

            #endregion // Preconditions

            _index = index;
            _synapses = new HashSet<HopfieldSynapse>();
            _bias = 0.0;
            _input = 0.0;
            _activationFunction = activationFunction;
            _output = 0.0;
        }

        #endregion // Instance constructors

        #region Instance methods

        /// <summary>
        /// Initializes the neuron.
        /// </summary>
        public void Initialize()
        {
            _bias = 0.0;
            _input = 0.0;
            _output = 0.0;
        }

        /// <summary>
        /// Evalautes the neuron.
        /// </summary>
        public void Evaluate(double evaluationProgressRatio)
        {
            // Calculate the input of the neuron.
            _input = 0.0;
            foreach (HopfieldSynapse synapse in _synapses)
            {
                _input += synapse.GetSourceNeuron(this).Output * synapse.Weight;
            }
            _input += _bias;

            // Calculate the output of the neuron.
            _output = _activationFunction(_input, evaluationProgressRatio);
        }

        #endregion // Instance methods

        #region Instance properties

        /// <summary>
        /// Gets the index.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public int Index
        {
            get
            {
                return _index;
            }
        }

        /// <summary>
        /// Gets the synapses.
        /// </summary>
        /// <value>
        /// The synapses.
        /// </value>
        public ICollection<HopfieldSynapse> Synapses
        {
            get
            {
                return _synapses;
            }
        }

        /// <summary>
        /// Gets or sets the bias of the neuron.
        /// </summary>
        /// <value>
        /// The bias of the neuron.
        /// </value>
        public double Bias
        {
            get
            {
                return _bias;
            }
            set
            {
                _bias = value;
            }
        }

        /// <summary>
        /// Gets or sets the output of the neuron.
        /// </summary>
        public double Output
        {
            get
            {
                return _output;
            }
            set
            {
                _output = value;
            }
        }

        #endregion // Instance properties

        #endregion // Public members


        #region Private members

        #region Instance fields

        /// <summary>
        /// The index of the neuron.
        /// (IMMUTABLE)
        /// </summary>
        private readonly int _index;

        /// <summary>
        /// The synapses.
        /// </summary>
        private ICollection<HopfieldSynapse> _synapses;

        /// <summary>
        /// The bias of the neuron.
        /// </summary>
        private double _bias;

        /// <summary>
        /// The input of the neuron.
        /// </summary>
        private double _input;

        /// <summary>
        /// The activation function.
        /// (IMMUTABLE)
        /// </summary>
        private readonly ActivationFunction _activationFunction;

        /// <summary>
        /// The output of the neuron.
        /// </summary>
        private double _output;

        #endregion // Instance fields

        #endregion // Private members
    }
}
