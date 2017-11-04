using Mozog.Utils;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps
{
    class HopfieldSynapse
    {
        /// <summary>
        /// Initializes a new instance of the Synapse class.
        /// </summary>
        /// <param name="neuron1">The first neuron.</param>
        /// <param name="neuron2">The second neuron.</param>
        public HopfieldSynapse(HopfieldNeuron neuron1, HopfieldNeuron neuron2)
        {
            Require.IsNotNull(neuron1, nameof(neuron1));
            Require.IsNotNull(neuron2, nameof(neuron2));

            _neuron1 = neuron1;
            _neuron2 = neuron2;
            _weight = 0.0;
        }

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
            return neuron == _neuron1 ? _neuron2 : _neuron1;
        }

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
    }
}
