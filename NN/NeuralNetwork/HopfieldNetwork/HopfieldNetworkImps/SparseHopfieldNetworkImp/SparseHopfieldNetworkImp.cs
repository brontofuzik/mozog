using System;
using System.Collections.Generic;

namespace NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp
{
    class SparseHopfieldNetworkImp
        : IHopfieldNetworkImp
    {
        #region Public members

        #region Instance methods

        /// <summary>
        /// Gets the bias of the neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <returns>The bias of the neuron.</returns>
        public double GetNeuronBias(int neuronIndex)
        {
            return getNeuron(neuronIndex).Bias;
        }

        /// <summary>
        /// Sets the bias of the neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        public void SetNeuronBias(int neuronIndex, double neuronBias)
        {
            getNeuron(neuronIndex).Bias = neuronBias;
        }

        /// <summary>
        /// Gets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the source neruon.</param>
        /// <returns>The weight of the synapse.</returns>
        public double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex)
        {
            HopfieldSynapse synapse = getSynapse(neuronIndex, sourceNeuronIndex);
            if (synapse != null)
            {
                return synapse.Weight;
            }
            else // (synapse == null)
            {
                return 0.0;
            }
        }

        /// <summary>
        /// Sets the weight of the synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the source neruon.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        public void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double synapseWeight)
        {
            HopfieldSynapse synapse = getSynapse(neuronIndex, sourceNeuronIndex);
            if (synapse != null)
            {
                if (synapseWeight != 0.0)
                {
                    synapse.Weight = synapseWeight;
                }
                else // (synapseWeight == 0.0)
                {
                    // Remove synapse.
                    synapse.Neuron1.Synapses.Remove(synapse);
                    synapse.Neuron2.Synapses.Remove(synapse);
                    _synapses.Remove(synapse);
                }
            }
            else // (synapse == null)
            {
                if (synapseWeight != 0.0)
                {
                    // Add synapse.
                    HopfieldNeuron neuron1 = getNeuron(neuronIndex);
                    HopfieldNeuron neuron2 = getNeuron(sourceNeuronIndex);
                    synapse = new HopfieldSynapse(neuron1, neuron2);
                    synapse.Weight = synapseWeight;

                    neuron1.Synapses.Add(synapse);
                    neuron2.Synapses.Add(synapse);
                    _synapses.Add(synapse);
                }
                else // (synapseWeight == 0.0)
                {
                    // Do nothing.
                }
            }
        }

        /// <summary>
        /// Set the input of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <param name="patternToRecall">The pattern to recall.</param>
        public void SetNetworkInput(double[] patternToRecall)
        {
            #region Preconditions

            // The length of the pattern to recall must match the number of neurons in the network.
            if (patternToRecall.Length != NeuronCount)
            {
                throw new ArgumentException("The length of the pattern to recall must match the number of neurons in the network.", "patternToRecall");
            }

            #endregion // Preconditions

            for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
            {
                double neuronOutput = patternToRecall[neuronIndex];
                setNeuronOutput(neuronIndex, neuronOutput);
            }
        }

        /// <summary>
        /// Evaluats the network implementation.
        /// </summary>
        public void Evaluate(double evaluationProgressRatio)
        {
            foreach (HopfieldNeuron neuron in neuronsRandomOrder)
            {
                neuron.Evaluate(evaluationProgressRatio);
            }
        }

        /// <summary>
        /// Gets the output of the network, i.e. the states of the neurons in the network.
        /// </summary>
        /// <returns>The recalled pattern.</returns>
        public double[] GetNetworkOutput()
        {
            double[] recalledPattern = new double[NeuronCount];
            for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
            {
                recalledPattern[neuronIndex] = getNeuronOutput(neuronIndex);
            }
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
                return _neurons.Length;
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
                return _synapses.Count;
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
                foreach (HopfieldSynapse synapse in _synapses)
                {
                    energy -= synapse.Weight * synapse.Neuron1.Output * synapse.Neuron2.Output;
                }

                // Calculate the energy of the neurons.
                foreach (HopfieldNeuron neuron in _neurons)
                {
                    energy -= neuron.Bias * neuron.Output;
                }

                return energy;
            }
        }

        #endregion // Instance properties

        #endregion // Public members


        #region Internal members

        #region Instance constructors

        /// <summary>
        /// Initializes a new instance of the HeavyweightNetworkImp class.
        /// </summary>
        internal SparseHopfieldNetworkImp(int neuronCount, ActivationFunction activationFunction)
        {
            _neurons = new HopfieldNeuron[neuronCount];
            for (int neuronIndex = 0; neuronIndex < neuronCount; ++neuronIndex)
            {
                _neurons[neuronIndex] = new HopfieldNeuron(neuronIndex, activationFunction);
            }
            _synapses = new HashSet<HopfieldSynapse>();
        }

        #endregion // Instance constructors

        #endregion // Internal members


        #region Internal members

        #region Instance properties

        /// <summary>
        /// Gets the neurons in the network.
        /// </summary>
        /// <value>
        /// The neurons in the network.
        /// </value>
        internal HopfieldNeuron[] Neurons_construction
        {
            get
            {
                return _neurons;
            }
            set
            {
                _neurons = value;
            }
        }

        /// <summary>
        /// Gets the synapses in the network.
        /// </summary>
        /// <value>
        /// The synapses in the network.
        /// </value>
        internal ICollection<HopfieldSynapse> Synapses_construction
        {
            get
            {
                return _synapses;
            }
        }

        #endregion // Instance properties

        #endregion // Internal members


        #region Private members

        #region Instance methods

        /// <summary>
        /// Gets the neuron with a given index.
        /// </summary>
        /// <param name="sourceNeuronIndex">The index of the neuron.</param>
        /// <returns>The neuron.</returns>
        public HopfieldNeuron getNeuron(int neuronIndex)
        {
            return _neurons[neuronIndex];
        }

        /// <summary>
        /// Gets the synapse connecting neurons with given indices.
        /// </summary>
        /// <param name="neuronIndex">The index of the first neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the second neuron.</param>
        /// <returns>The synapse connecting the neurons with given indices.</returns>
        private HopfieldSynapse getSynapse(int neuron1Index, int neuron2Index)
        {
            HopfieldNeuron neuron1 = getNeuron(neuron1Index);
            HopfieldNeuron neuron2 = getNeuron(neuron2Index);

            foreach (HopfieldSynapse synapse in neuron2.Synapses)
            {
                if (synapse.GetSourceNeuron(neuron2) == neuron1)
                {
                    return synapse;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the output of a neuron specified by its index.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <returns>The output of the neuron.</returns>
        private double getNeuronOutput(int neuronIndex)
        {
            return getNeuron(neuronIndex).Output;
        }

        /// <summary>
        /// Sets the output of the neuron specified by its index.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronOutput">The output of the neuron.</param>
        private void setNeuronOutput(int neuronIndex, double neuronOutput)
        {
            getNeuron(neuronIndex).Output = neuronOutput;
        }

        #endregion // Insatnce methods

        #region Instance properties

        /// <summary>
        /// Gets the neurons in random order.
        /// </summary>
        /// <value>
        /// The neurons in random order.
        /// </value>
        private HopfieldNeuron[] neuronsRandomOrder
        {
            get
            {
                HopfieldNeuron[] neuronsRandomOrder = new HopfieldNeuron[NeuronCount];
                Array.Copy(_neurons, neuronsRandomOrder, NeuronCount);
                Utilities.Shuffle<HopfieldNeuron>(neuronsRandomOrder);
                return neuronsRandomOrder;
            }
        }

        /// <summary>
        /// Gets a random neuron.
        /// </summary>
        /// <value>
        /// A random neuron.
        /// </value>
        private HopfieldNeuron randomNeuron
        {
            get
            {
                return _neurons[Utilities.Next(NeuronCount)];
            }
        }

        #endregion // Instacne properties

        #region Instance fields

        /// <summary>
        /// The neurons in the network.
        /// </summary>
        private HopfieldNeuron[] _neurons;

        /// <summary>
        /// The synapses in the network.
        /// </summary>
        private ICollection<HopfieldSynapse> _synapses;

        #endregion // Insatnce fields

        #endregion // Private members
    }
}
