using NeuralNetwork.MultilayerPerceptron.Synapses;

namespace NeuralNetwork.MultilayerPerceptron.Connectors
{
    /// <remarks>
    /// A blueprint of a connector.
    /// </remarks>
    public class ConnectorBlueprint
    {
        #region Private instance fields

        /// <summary>
        /// The index of the source layer (within the network).
        /// </summary>
        private int sourceLayerIndex;

        /// <summary>
        /// The index of the target layer (within the network).
        /// </summary>
        private int targetLayerIndex;

        /// <summary>
        /// The blueprints of the synapses.
        /// </summary>
        private SynapseBlueprint[] synapseBlueprints;

        #endregion // Private instance fields

        #region Public instance properties

        /// <summary>
        /// Gets the index of the source layer (within the network).
        /// </summary>
        /// 
        /// <value>
        /// The index of the source layer (within the network).
        /// </value>
        public int SourceLayerIndex
        {
            get
            {
                return sourceLayerIndex;
            }
        }

        /// <summary>
        /// Gets the index of the target layer (within the network).
        /// </summary>
        /// 
        /// <value>
        /// The index of the target layer (within the network).
        /// </value>
        public int TargetLayerIndex
        {
            get
            {
                return targetLayerIndex;
            }
        }

        /// <summary>
        /// Gets the number of synapses comprising the connector.
        /// </summary>
        /// <value>
        /// The number of synapses comprising the connector.
        /// </value>
        public int SynapseCount
        {
            get
            {
                return synapseBlueprints.Length;
            }
        }

        /// <summary>
        /// Gets the blueprints of the synapses.
        /// </summary>
        /// <value>
        /// The blueprints of the synapses.
        /// </value>
        public SynapseBlueprint[] SynapseBlueprints
        {
            get
            {
                return synapseBlueprints;
            }
        }

        #endregion // Public instance properties

        #region Public instance constructors

        /// <summary>
        /// Creates a new connector blueprint.
        /// </summary>
        /// <param name="sourceLayerIndex">The index of the source layer (within the network).</param>
        /// <param name="sourceLayerNeuronCount">The number of neurons comprising the source layer.</param>
        /// <param name="targetLayerIndex">The index of the target layer (within the network).</param>
        /// <param name="targetLayerNeuronCount">The number of neurons comprising the target layer.</param>
        public ConnectorBlueprint( int sourceLayerIndex, int sourceLayerNeuronCount, int targetLayerIndex, int targetLayerNeuronCount)
        {
            // 1.
            this.sourceLayerIndex = sourceLayerIndex;
            this.targetLayerIndex = targetLayerIndex;

            // 2. Create the synapse blueprints.
            synapseBlueprints = new SynapseBlueprint[sourceLayerNeuronCount * targetLayerNeuronCount];

            int i = 0;
            for (int targetNeuronIndex = 0; targetNeuronIndex < targetLayerNeuronCount; targetNeuronIndex++)
            {
                for (int sourceNeuronIndex = 0; sourceNeuronIndex < sourceLayerNeuronCount; sourceNeuronIndex++)
                {
                    // 2.1. Create the synpase blueprint between the source neuron and the target neuron.
                    synapseBlueprints[i++] = new SynapseBlueprint(sourceNeuronIndex, targetNeuronIndex);
                }
            }
        }

        #endregion // Public instance constructors
    }
}