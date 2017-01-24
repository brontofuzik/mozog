using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Connectors;
using NeuralNetwork.MultilayerPerceptron.Layers;
using NeuralNetwork.MultilayerPerceptron.Neurons;


namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /// <summary>
    /// A synapse.
    ///</summary>
    public class Synapse
        : ISynapse
    {
        /// <summary>
        /// The blueprint of the synapse.
        /// </summary>
        private SynapseBlueprint blueprint;

        /// <summary>
        /// The weight.
        /// </summary>
        private double weight;

        /// <summary>
        /// The source neuron.
        /// </summary>
        private INeuron sourceNeuron;

        /// <summary>
        /// The target neuron.
        /// </summary>
        private IActivationNeuron targetNeuron;

        /// <summary>
        /// The parent conenctor.
        /// </summary>
        private IConnector parentConnector;

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
                return blueprint;
            }
        }

        /// <summary>
        /// Gets or sets the weight.
        /// </summary>
        ///
        /// <value>
        /// The weight.
        /// </value>
        public double Weight
        {
            get
            {
                return weight;
            }
            set
            {
                weight = value;
            }
        }

        /// <summary>
        /// Gets the source neuron.
        /// </summary>
        /// 
        /// <value>
        /// The source neuron.
        /// </value>
        public INeuron SourceNeuron
        {
            get
            {
                return sourceNeuron;
            }
            set
            {
                sourceNeuron = value;
            }
        }

        /// <summary>
        /// Gets the target neuron.
        /// </summary>
        /// 
        /// <value>
        /// The tager neuron.
        /// </value>
        public IActivationNeuron TargetNeuron
        {
            get
            {
                return targetNeuron;
            }
            set
            {
                targetNeuron = value;
            }
        }

        /// <summary>
        /// Gets the parent connector.
        /// </summary>
        /// 
        /// <value>
        /// The parent connector.
        /// </value>
        public IConnector ParentConnector
        {
            get
            {
                return parentConnector;
            }
            set
            {
                parentConnector = value;
            }
        }

        /// <summary>
        /// Creates a new synapse (from a blueprint).
        /// </summary>
        /// <param name="blueprint">The blueprint of the synapse.</param>
        /// <param name="parentConnector">The parent connector.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Condition 1: <c>blueprint</c> is <c>null</c>.
        /// Condition 2: <c>parentConnector</c> is <c>null</c>.
        /// </exception>
        public Synapse(SynapseBlueprint blueprint, IConnector parentConnector)
        {
            // Validate the synapse blueprint.
            Require.IsNotNull(blueprint, "blueprint");
            this.blueprint = blueprint;

            // Validate the parent connector.
            Require.IsNotNull(parentConnector, "parentConnector");
            this.parentConnector = parentConnector;
        }

        /// <summary>
        /// Connects the synapse.
        /// 
        /// A synapse is said to be connected if:
        /// 1. it is aware of its source neuron (and vice versa), and
        /// 2. it is aware of its target neuron (and vice versa).
        /// </summary>
        public void Connect()
        {
            Connect(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be connected.</param>
        public void Connect(ISynapse synapse)
        {
            // 1. Make the synapse aware of its source neuron ...
            ILayer sourceLayer = parentConnector.SourceLayer;
            sourceNeuron = sourceLayer.GetNeuronByIndex(blueprint.SourceNeuronIndex);
            // ... and vice versa.
            sourceNeuron.TargetSynapses.Add(synapse);

            // 2. Make the synapse aware of its target neuron ...
            ILayer targetLayer = parentConnector.TargetLayer;
            targetNeuron = (IActivationNeuron)targetLayer.GetNeuronByIndex(blueprint.TargetNeuronIndex);
            // ... and vice versa.
            targetNeuron.SourceSynapses.Add(synapse);
        }

        /// <summary>
        /// Disconnects the synapse.
        /// 
        /// A synapse is said to be disconnected if:
        /// 1. its source neuron is not aware of it (and vice versa) and 
        /// 2. its target neuron is not aware of it (and vice versa).
        /// </summary>
        public void Disconnect()
        {
            Disconnect(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="synapse">The synapse to be disconnected.</param>
        public void Disconnect(ISynapse synapse)
        {
            // 1. Make its source neuron not aware of it ...
            sourceNeuron.TargetSynapses.Remove(synapse);
            // ... and vice versa.
            sourceNeuron = null;

            // 2. Make its target neuron not aware of it ...
            targetNeuron.SourceSynapses.Remove(synapse);
            // ... and vice versa.
            targetNeuron = null;
        }

        /// <summary>
        /// Initializes the synapse.
        /// </summary>
        public void Initialize()
        {
            weight = Random.Double(-1, +1);
        }


        /// <summary>
        /// Jitters the synapse so that the neural network deviates from its local minimum position where further learning is of no use.
        /// </summary>
        /// 
        /// <param name="jitterNoiseLimit">The maximum absolute jitter noise added.</param>
        public void Jitter(double jitterNoiseLimit)
        {
            weight += Random.Double(-jitterNoiseLimit, +jitterNoiseLimit);
        }

        /// <summary>
        /// Returns a string representation of the synapse.
        /// </summary>
        /// 
        /// <returns>
        /// A string representation of the synapse.
        /// </returns>
        public override string ToString()
        {
            return weight.ToString("F2");
        }
    }
}