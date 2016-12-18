namespace NeuralNetwork.MultilayerPerceptron.Synapses
{
    /// <summary>
    /// A blueprint of a synapse.
    /// </summary>
    public class SynapseBlueprint
    {
        /// <summary>
        /// The index of the source neuron (within the source layer).
        /// </summary>
        private int sourceNeuronIndex;

        /// <summary>
        /// The index of the target neuron (within the target layer).
        /// </summary>
        private int targetNeuronIndex;

        /// <summary>
        /// Gets the index of the source neuron (within the source layer).
        /// </summary>
        /// <value>
        /// The index of the source neuron (within the source layer).
        /// </value>
        public int SourceNeuronIndex
        {
            get
            {
                return sourceNeuronIndex;
            }
        }

        /// <summary>
        /// Gets the index of the target neuron (within the target layer).
        /// </summary>
        /// <value>
        /// The index of the target neuron (within the target layer).
        /// </value>
        public int TargetNeuronIndex
        {
            get
            {
                return targetNeuronIndex;
            }
        }

        /// <summary>
        /// Creates a new synapse blueprint.
        /// </summary>
        /// 
        /// <param name="neuronIndex">The index of the source neuron (within the source layer).</param>
        /// <param name="sourceNeuronIndex">The index of the target neuron (within the target layer).</param>
        public SynapseBlueprint(int sourceNeuronIndex, int targetNeuronIndex)
        {
            this.sourceNeuronIndex = sourceNeuronIndex;
            this.targetNeuronIndex = targetNeuronIndex;
        }
    }
}
