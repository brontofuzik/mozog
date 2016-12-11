using System;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp
{
    class FullHopfieldNetworkImp
        : IHopfieldNetworkImp
    {
        #region Public members

        #region Instance methods

        /// <summary>
        /// Gets the bias of the neuron.
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>The bias of the neuron.</returns>
        public double GetNeuronBias(int neuronIndex)
        {
            return _neuronBiases[neuronIndex];
        }

        /// <summary>
        /// Sets the bias of the neuron.
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        public void SetNeuronBias(int neuronIndex, double neuronBias)
        {
            _neuronBiases[neuronIndex] = neuronBias;
        }

        /// <summary>
        /// Gets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the first neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the second neruon.</param>
        /// <returns>The weight of the synapse.</returns>
        public double GetSynapseWeight(int neuron1Index, int neuron2Index)
        {
            return _synapseWeights[neuron1Index, neuron2Index];
        }

        /// <summary>
        /// Sets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the first neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the second neruon.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        public void SetSynapseWeight(int neuron1Index, int neuron2Index, double synapseWeight)
        {
            _synapseWeights[neuron1Index, neuron2Index] = _synapseWeights[neuron2Index, neuron1Index] = synapseWeight;
        }

        /// <summary>
        /// Set the input of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <param name="patternToRecall">The pattern to recall.</param>
        public void SetNetworkInput(double[] patternToRecall)
        {
            Array.Copy(patternToRecall, _neuronOutputs, NeuronCount);
        }

        /// <summary>
        /// Evaluats the network implementation.
        /// </summary>
        public void Evaluate(double progressRatio)
        {
            foreach (int neuronIndex in neuronIndicesRandomOrder)
            {
                evaluateNeuron(neuronIndex, progressRatio);
            }
        }

        /// <summary>
        /// Gets the output of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <returns>The recalled pattern.</returns>
        public double[] GetNetworkOutput()
        {
            double[] recalledPattern = new double[NeuronCount];
            Array.Copy(_neuronOutputs, recalledPattern, NeuronCount);
            return recalledPattern;
        }

        #endregion // Instance methods

        #region Instance properties

        /// <summary>
        /// Gets the number of neurons.
        /// </summary>
        /// <value>
        /// The number of neurons.
        /// </value>
        public int NeuronCount
        {
            get
            {
                return _neuronIndices.Length;
            }
        }

        /// <summary>
        /// Gets the number of synapses.
        /// </summary>
        /// <value>
        /// The number of synapses.
        /// </value>
        public int SynapseCount
        {
            get
            {
                return (_synapseWeights.Length - _neuronIndices.Length) / 2;
            }
        }

        /// <summary>
        /// Gets the energy.
        /// </summary>
        /// <value>
        /// The energy.
        /// </value>
        public double Energy
        {
            get
            {
                double energy = 0.0;

                // Calculate the energy of the synapses.
                for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
                {
                    for (int sourceNeuronIndex = neuronIndex + 1; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
                    {
                        energy -= _synapseWeights[neuronIndex, sourceNeuronIndex] * _neuronOutputs[neuronIndex] * _neuronOutputs[sourceNeuronIndex];
                    }
                }

                // Calculate the energy of the neurons.
                for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
                {
                    energy -= _neuronBiases[neuronIndex] * _neuronOutputs[neuronIndex];
                }

                return energy;
            }
        }

        #endregion // Instance properties

        #endregion // Public members


        #region Internal members

        #region Instance constructors

        internal FullHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            _neuronIndices = new int[neuronCount];
            for (int neuronIndex = 0; neuronIndex < neuronCount; ++neuronIndex)
            {
                _neuronIndices[neuronIndex] = neuronIndex;
            }
            _synapseWeights = new double[neuronCount, neuronCount];
            _neuronBiases = new double[neuronCount];
            _activationFunction = activationFunction;
            _neuronOutputs = new double[neuronCount];
        }

        #endregion // Instance constructors

        #endregion // Internal members


        #region Private members

        #region Instance methods

        private void evaluateNeuron(int neuronIndex, double evaluationProgressRatio)
        {
            // Calculate the input of the neuron.
            double neuronInput = 0.0;
            for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
            {
                neuronInput += _synapseWeights[neuronIndex, sourceNeuronIndex] * _neuronOutputs[sourceNeuronIndex];
            }
            neuronInput += _neuronBiases[neuronIndex];

            // Calculate the output of the neuron.
            _neuronOutputs[neuronIndex] = _activationFunction(neuronInput, evaluationProgressRatio);
        }

        #endregion // Instance methods

        #region Instance properties

        /// <summary>
        /// Gets the indices of the neurons in random order.
        /// </summary>
        /// <value>
        /// The indices of the neurons in random order.
        /// </value>
        private int[] neuronIndicesRandomOrder
        {
            get
            {
                int[] neuronIndicesRandomOrder = new int[NeuronCount];
                Array.Copy(_neuronIndices, neuronIndicesRandomOrder, NeuronCount);
                Utilities.Shuffle<int>(neuronIndicesRandomOrder);
                return neuronIndicesRandomOrder;
            }
        }

        /// <summary>
        /// Gets an index of a random neuron.
        /// </summary>
        /// <value>
        /// The index of a random neuron.
        /// </value>
        private int randomNeuronIndex
        {
            get
            {
                return Utilities.Next(NeuronCount);
            }
        }

        #endregion // Instance properties

        #region Instance fields

        /// <summary>
        /// The indices of the neurons.
        /// </summary>
        private int[] _neuronIndices;

        private double[,] _synapseWeights;

        /// <summary>
        /// The biases of the neurons.
        /// </summary>
        private double[] _neuronBiases;

        /// <summary>
        /// The activation function.
        /// </summary>
        private ActivationFunction _activationFunction;

        /// <summary>
        /// The outputs of the neurons.
        /// </summary>
        private double[] _neuronOutputs;

        #endregion // Instance fields

        #endregion // Private members
    }
}
