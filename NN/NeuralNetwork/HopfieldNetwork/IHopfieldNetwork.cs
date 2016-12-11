using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.HopfieldNetwork
{
    public interface IHopfieldNetwork
    {
        #region Methods

        /// <summary>
        /// Trains the Hopfield network.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        void Train(TrainingSet trainingSet);

        /// <summary>
        /// Gets the bias of a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <returns>The bias of the neuron.</returns>
        double GetNeuronBias(int neuronIndex);

        /// <summary>
        /// Sets the bias of a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        void SetNeuronBias(int neuronIndex, double neuronBias);

        /// <summary>
        /// Gets the weight of a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neruon.</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        /// <returns>The weight of the synapse.</returns>
        double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex);

        /// <summary>
        /// Sets the weight of a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double synapseWeight);

        /// <summary>
        /// Evaluates the Hopfield network.
        /// </summary>
        /// <param name="patternToRecall">The pattern to recall.</param>
        /// <param name="evaluationIterationCount">The number of iterations.</param>
        /// <returns>The recalled pattern.</returns>
        double[] Evaluate(double[] patternToRecall, int evaluationIterationCount);

        /// <summary>
        /// Converts the Hopfield network to its string representation.
        /// </summary>
        /// <returns>The string representation of the Hopfield network.</returns>
        string ToString();

        #endregion // Methods

        #region Properties

        /// <summary>
        /// Gets the number of neurons in the Hopfield network.
        /// </summary>
        /// <value>
        /// The number of neurons in the Hopfield network.
        /// </value>
        int NeuronCount
        {
            get;
        }

        /// <summary>
        /// Gets the number of synapses in the Hopfield network.
        /// </summary>
        /// <value>
        /// The number of synapses in the Hopfield network.
        /// </value>
        int SynapseCount
        {
            get;
        }

        /// <summary>
        /// Gets the energy of the Hopfield network.
        /// </summary>
        /// <value>
        /// The energy of the Hopfield network.
        /// </value>
        double Energy
        {
            get;
        }

        #endregion // Properties
    }
}
