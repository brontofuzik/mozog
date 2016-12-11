using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp;

namespace NeuralNetwork.Examples.Hopfield
{
    class EightQueensHopfieldNetwork
    {
        #region Public members

        #region Instance constructors

        /// <summary>
        /// Initializes a new instance of the EightQueensNetwork class.
        /// </summary>
        public EightQueensHopfieldNetwork()
        {
            _hopfieldNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(64, eightQueensNetworkActivationFunction, new SparseHopfieldNetworkImpFactory());
        }

        #endregion // Instance constructors

        #region Instance methods

        /// <summary>
        /// Trains the eight-queens network.
        /// </summary>
        public void Train()
        {
            // Train the neurons.
            for (int neuronYCoordinate = 0; neuronYCoordinate < 8; ++neuronYCoordinate)
            {
                for (int neuronXCoordinate = 0; neuronXCoordinate < 8; ++neuronXCoordinate)
                {
                    trainNeuron(neuronXCoordinate, neuronYCoordinate);
                }
            }
        }

        /// <summary>
        /// Evaluates the eight-queens network.
        /// </summary>
        /// <param name="evaluationIterationCount">The number of evaluation iterations.</param>
        /// <returns>The recalled pattern.</returns>
        public double[] Evaluate(int evaluationIterationCount)
        {
            double[] patternToRecall = new double[NeuronCount];
            double[] recalledPatter = _hopfieldNetwork.Evaluate(patternToRecall, evaluationIterationCount);
            return recalledPatter;
        }

        #endregion // Instance methods

        #endregion // Public members


        #region Private members

        #region Static methods

        /// <summary>
        /// The activation function of the eight-queens network.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double eightQueensNetworkActivationFunction(double input, double evaluationProgressRatio)
        {
            return (input > 0) ? 1.0 : 0.0;
        }

        #endregion // Static methods

        #region Instance methods

        /// <summary>
        /// Trains a neuron.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        private void trainNeuron(int neuronXCoordinate, int neuronYCoordinate)
        {
            // Calculate the bias of the neuron.
            double neuronBias = 1.0;

            // Set the bias of the neuron.
            setNeuronBias(neuronXCoordinate, neuronYCoordinate, neuronBias);

            // The source neurons in the same row.
            // Left
            for (int sourceNeuronXCoordinate = neuronXCoordinate - 1, sourceNeuronYCoordinate = neuronYCoordinate; 0 <= sourceNeuronXCoordinate; --sourceNeuronXCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }
            // Right
            for (int sourceNeuronXCoordinate = neuronXCoordinate + 1, sourceNeuronYCoordinate = neuronYCoordinate; sourceNeuronXCoordinate < 8; ++sourceNeuronXCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }

            // The source neurons in the same column.
            // Up
            for (int sourceNeuronXCoordinate = neuronXCoordinate, sourceNeuronYCoordinate = neuronYCoordinate - 1; 0 <= sourceNeuronYCoordinate; --sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }
            // Down
            for (int sourceNeuronXCoordinate = neuronXCoordinate, sourceNeuronYCoordinate = neuronYCoordinate + 1; sourceNeuronYCoordinate < 8; ++sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }

            // The source neurons in the same primary diagonal.
            // Left & Up
            for (int sourceNeuronXCoordinate = neuronXCoordinate - 1, sourceNeuronYCoordinate = neuronYCoordinate - 1; 0 <= sourceNeuronXCoordinate && 0 <= sourceNeuronYCoordinate; --sourceNeuronXCoordinate, --sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }
            // Right & Down
            for (int sourceNeuronXCoordinate = neuronXCoordinate + 1, sourceNeuronYCoordinate = neuronYCoordinate + 1; sourceNeuronXCoordinate < 8 && sourceNeuronYCoordinate < 8; ++sourceNeuronXCoordinate, ++sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }

            // The source neurons in the same secondary diagonal.
            // Left & Down
            for (int sourceNeuronXCoordinate = neuronXCoordinate - 1, sourceNeuronYCoordinate = neuronYCoordinate + 1; 0 <= sourceNeuronXCoordinate && sourceNeuronYCoordinate < 8; --sourceNeuronXCoordinate, ++sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }
            // Right & Up
            for (int sourceNeuronXCoordinate = neuronXCoordinate + 1, sourceNeuronYCoordinate = neuronYCoordinate - 1; sourceNeuronXCoordinate < 8 && 0 <= sourceNeuronYCoordinate; ++sourceNeuronXCoordinate, --sourceNeuronYCoordinate)
            {
                trainSynapse(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate);
            }
        }

        /// <summary>
        /// Trains a synapse.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="sourceNeuronXCoordinate">The x coordinate of the source neuron.</param>
        /// <param name="sourceNeuronYCoordinate">The y coordinate of the source neuron.</param>
        private void trainSynapse(int neuronXCoordinate, int neuronYCoordinate, int sourceNeuronXCoordinate, int sourceNeuronYCoordinate)
        {
            // Calculate the bias of the synapse.
            double synapseWeight = -2.0;

            // Set the weight of the synapse.
            setSynapseWeight(neuronXCoordinate, neuronYCoordinate, sourceNeuronXCoordinate, sourceNeuronYCoordinate, synapseWeight);
        }

        /// <summary>
        /// Sets the bias of a neuron.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        private void setNeuronBias(int neuronXCoordinate, int neuronYCoordinate, double neuronBias)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronXCoordinate, neuronYCoordinate);

            // Set the bias of the neuron in the underlying network.
            _hopfieldNetwork.SetNeuronBias(neuronIndex, neuronBias);
        }

        /// <summary>
        /// Sets the weight of a synapse.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <param name="sourceNeuronXCoordinate">The x coordinate of the source neuron.</param>
        /// <param name="sourceNeuronYCoordinate">The y coordinate of the source neuron.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        private void setSynapseWeight(int neuronXCoordinate, int neuronYCoordinate, int sourceNeuronXCoordinate, int sourceNeuronYCoordinate, double synapseWeight)
        {
            // Calculate the index of the neuron.
            int neuronIndex = neuronCoordinatesToIndex(neuronXCoordinate, neuronYCoordinate);

            // Calculate the index of the source neuron.
            int sourceNeuronIndex = neuronCoordinatesToIndex(sourceNeuronXCoordinate, sourceNeuronYCoordinate);

            // Set the weight of the synapse in the underlying network.
            _hopfieldNetwork.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        /// <summary>
        /// Converts the index of a neuron to its coordinates.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        private void neuronIndexToCoordinates(int neuronIndex, out int neuronXCoordinate, out int neuronYCoordinate)
        {
            // Calculate the x coordinate of the neuron.
            neuronXCoordinate = neuronIndex % 8;
            
            // Calculate the y coordinate of the neuron.
            neuronYCoordinate = neuronIndex / 8;
        }

        /// <summary>
        /// Converts the coordinates of the neuron to its index.
        /// </summary>
        /// <param name="neuronXCoordinate">The x coordinate of the neuron.</param>
        /// <param name="neuronYCoordinate">The y coordinate of the neuron.</param>
        /// <returns>The index of the neuron.</returns>
        private int neuronCoordinatesToIndex(int neuronXCoordinate, int neuronYCoordinate)
        {
            return neuronYCoordinate * 8 + neuronXCoordinate;
        }

        #endregion // Instance methods

        #region Instance properties

        private int NeuronCount
        {
            get
            {
                return _hopfieldNetwork.NeuronCount;
            }
        }

        #endregion // Instance properties

        #region Instance fields

        /// <summary>
        /// The underlying Hopfield network.
        /// </summary>
        private NeuralNetwork.HopfieldNetwork.HopfieldNetwork _hopfieldNetwork;

        #endregion // Instance fields

        #endregion // Private members
    }
}
