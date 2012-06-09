using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Neurons;

namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /// <summary>
    /// <para>
    /// An interface of a synapse.
    /// </para>
    /// <para>
    /// Definition: A synapse is a connection between two neurons.
    /// </para>
    /// </summary>
    public interface ISynapse
    {
        #region Properties

        /// <summary>
        /// Gets the blueprint of the synapse.
        /// </summary>
        /// <value>
        /// The blueprint of the synapse.
        /// </value>
        SynapseBlueprint Blueprint
        {
            get;
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        ///
        /// <value>
        /// The weight.
        /// </value>
        double Weight
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the source neuron.
        /// </summary>
        /// 
        /// <value>
        /// The source neuron.
        /// </value>
        INeuron SourceNeuron
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the target neuron.
        /// </summary>
        /// 
        /// <value>
        /// The tager neuron.
        /// </value>
        IActivationNeuron TargetNeuron
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the parent connector.
        /// </summary>
        /// 
        /// <value>
        /// The parent connector.
        /// </value>
        IConnector ParentConnector
        {
            get;
            set;
        }

        #endregion // Properties

        #region Methods

        /// <summary>
        /// Connects the synapse.
        /// 
        /// A synapse is said to be connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        void Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be connected.</param>
        void Connect(ISynapse synapse);

        /// <summary>
        /// Disconnects the synapse.
        /// 
        /// A synapse is said to be disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be disconnected.</param>
        void Disconnect(ISynapse synapse);

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        void Initialize();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        void Jitter(double jitterNoiseLimit);

        #endregion // Methods
    }
}
