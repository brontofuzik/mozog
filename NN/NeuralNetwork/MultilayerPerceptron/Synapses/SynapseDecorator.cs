using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Neurons;


namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SynapseDecorator
        : ISynapse
    {
        #region Protected instance fields

        /// <summary>
        /// 
        /// </summary>
        protected ISynapse decoratedSynapse;

        #endregion // Protected instance fields

        #region Public insatnce properties

        /// <summary>
        /// Gets the blueprint of the synapse.
        /// </summary>
        /// <value>
        /// The blueprint of the synapse.
        /// </value>
        public SynapseBlueprint Blueprint
        {
            get
            {
                return decoratedSynapse.Blueprint;
            }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        ///
        /// <value>
        /// The weight.
        /// </value>
        public virtual double Weight
        {
            get
            {
                return decoratedSynapse.Weight;
            }
            set
            {
                decoratedSynapse.Weight = value;
            }
        }

        /// <summary>
        /// Gets the source neuron.
        /// </summary>
        /// 
        /// <value>
        /// The source neuron.
        /// </value>
        public virtual INeuron SourceNeuron
        {
            get
            {
                return decoratedSynapse.SourceNeuron;
            }
            set
            {
                decoratedSynapse.SourceNeuron = value;
            }
        }

        /// <summary>
        /// Gets the target neuron.
        /// </summary>
        /// 
        /// <value>
        /// The tager neuron.
        /// </value>
        public virtual IActivationNeuron TargetNeuron
        {
            get
            {
                return decoratedSynapse.TargetNeuron;
            }
            set
            {
                decoratedSynapse.TargetNeuron = value;
            }
        }

        /// <summary>
        /// Gets the parent connector.
        /// </summary>
        /// 
        /// <value>
        /// The parent connector.
        /// </value>
        public virtual IConnector ParentConnector
        {
            get
            {
                return decoratedSynapse.ParentConnector;
            }
            set
            {
                decoratedSynapse.ParentConnector = value;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoratedSynapse">The synapse to be decorated.</param>
        /// <param name="parentConnector">The parent connector.</param>
        public SynapseDecorator( ISynapse decoratedSynapse, IConnector parentConnector )
        {
            this.decoratedSynapse = decoratedSynapse;
            ParentConnector = parentConnector;
        }

        #endregion // Public instance constructors

        #region Public instance methods

        /// <summary>
        /// Connects the synapse.
        /// 
        /// A synapse is said to be connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        public abstract void Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be connected.</param>
        public virtual void Connect(ISynapse synapse)
        {
            decoratedSynapse.Connect(synapse);
        }

        /// <summary>
        /// Disconnects the synapse.
        /// 
        /// A synapse is said to be disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        public abstract void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be disconnected.</param>
        public virtual void Disconnect(ISynapse synapse)
        {
            decoratedSynapse.Disconnect(synapse);
        }

        /// <summary>
        /// Returns the decorated synapse.
        /// </summary>
        /// <param name="parentConnector">The parent connector.</param>
        /// <returns>
        /// The decorated synapse.
        /// </returns>
        public virtual ISynapse GetDecoratedSynapse(IConnector parentConnector)
        {
            // Reintegrate.
            ParentConnector = parentConnector;

            return decoratedSynapse;
        }

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        public virtual void Initialize()
        {
            decoratedSynapse.Initialize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jitterNoiseLimit"></param>
        public virtual void Jitter(double jitterNoiseLimit)
        {
            decoratedSynapse.Jitter(jitterNoiseLimit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return decoratedSynapse.ToString();
        }

        #endregion // Public instance methods
    }
}
