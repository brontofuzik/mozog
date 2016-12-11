using System.Collections.Generic;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps
{
    public interface IHopfieldNetworkImp
    {
        #region Methods

        /// <summary>
        /// Gets the bias of the neuron.
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>The bias of the neuron.</returns>
        double GetNeuronBias(int neuronIndex);

        /// <summary>
        /// Sets the bias of the neuron.
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        void SetNeuronBias(int neuronIndex, double neuronBias);

        /// <summary>
        /// Gets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the first neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the second neruon.</param>
        /// <returns>The weight of the synapse.</returns>
        double GetSynapseWeight(int neuron1Index, int neuron2Index);

        /// <summary>
        /// Sets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the first neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the second neruon.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        void SetSynapseWeight(int neuron1Index, int neuron2Index, double synapseWeight);

        /// <summary>
        /// Set the input of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <param name="patternToRecall">The pattern to recall.</param>
        void SetNetworkInput(double[] patternToRecall);

        /// <summary>
        /// Evaluats the network implementation.
        /// </summary>
        void Evaluate(double evaluationProgressRatio);

        /// <summary>
        /// Gets the output of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <returns>The recalled pattern.</returns>
        double[] GetNetworkOutput();

        #endregion // Methods


        #region Properties

        /// <summary>
        /// Gets the number of neurons.
        /// </summary>
        /// <value>
        /// The number of neurons.
        /// </value>
        int NeuronCount
        {
            get;
        }

        /// <summary>
        /// Gets the number of synapses.
        /// </summary>
        /// <value>
        /// The number of synapses.
        /// </value>
        int SynapseCount
        {
            get;
        }

        /// <summary>
        /// Gets the energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        double Energy
        {
            get;
        }

        #endregion // Properties        
    }
}
