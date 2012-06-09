namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    class HopfieldSynapse
    {
        #region Public members

        #region Instance constructors

        /// <summary>
        /// Initializes a new instance of the Synapse class.
        /// </summary>
        /// <param name="neuron1">The first neuron.</param>
        /// <param name="neuron2">The second neuron.</param>
        public HopfieldSynapse(HopfieldNeuron neuron1, HopfieldNeuron neuron2)
        {
            #region Preconditions

            // The first neuron must be provided.
            Utilities.RequireObjectNotNull(neuron1, "neuron1");

            // The sedond neuron must be provided.
            Utilities.RequireObjectNotNull(neuron2, "neuron2");

            #endregion // Preconditions

            _neuron1 = neuron1;
            _neuron2 = neuron2;
            _weight = 0.0;
        }

        #endregion // Instance constructors

        #region Instance methods

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        public void Initialize()
        {
            _weight = 0.0;
        }

        /// <summary>
        /// Gets the source neuron.
        /// </summary>
        /// <param name="neuron">The neuron whose source neuron to get.</param>
        /// <returns>The source neuron.</returns>
        public HopfieldNeuron GetSourceNeuron(HopfieldNeuron neuron)
        {
            return (neuron == _neuron1) ? _neuron2 : _neuron1;
        }

        #endregion // Instance methods

        #region Instance properties

        /// <summary>
        /// Gets or sets the first neuron.
        /// </summary>
        /// <value>
        /// The first neuron.
        /// </value>
        public HopfieldNeuron Neuron1
        {
            get
            {
                return _neuron1;
            }
            set
            {
                _neuron1 = value;
            }
        }

        /// <summary>
        /// Gets or sets the second neuron.
        /// </summary>
        /// <value>
        /// The second neuron.
        /// </value>
        public HopfieldNeuron Neuron2
        {
            get
            {
                return _neuron2;
            }
            set
            {
                _neuron2 = value;
            }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
            }
        }

        #endregion // Instance properties

        #endregion // Public members


        #region Private members

        #region Instance fields

        /// <summary>
        /// The first neuron of the synapse.
        /// </summary>
        HopfieldNeuron _neuron1;

        /// <summary>
        /// The second neuron of the synapse.
        /// </summary>
        HopfieldNeuron _neuron2;

        /// <summary>
        /// The weight of the synapse.
        /// </summary>
        double _weight;

        #endregion // Instance fields

        #endregion // Private members
    }
}
