using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.FullHopfieldNetworkImp;

namespace NeuralNetwork.Examples.HopfieldNetwork
{
    class MultiflopHopfieldNetwork
    {
        /// <summary>
        /// Initializes a new instance of the MultiflopHopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount"></param>
        public MultiflopHopfieldNetwork(int neuronCount)
        {
            _hopfieldNetwork = new NeuralNetwork.HopfieldNetwork.HopfieldNetwork(neuronCount, multiflopNetworkActivationFunction, new FullHopfieldNetworkImpFactory());
        }

        /// <summary>
        /// Trains the multiflop network.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        public void Train()
        {
            // Train the neurons.
            for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
            {
                TrainNeuron(neuronIndex);
            }
        }

        /// <summary>
        /// Evaluates the multiflop network.
        /// </summary>
        /// <param name="evaluationIterationCount">The number of evaluation iterations.</param>
        /// <returns>The recalled pattern.</returns>
        public double[] Evaluate(int evaluationIterationCount)
        {
            double[] patternToRecall = new double[NeuronCount];
            double[] recalledPatter = _hopfieldNetwork.Evaluate(patternToRecall, evaluationIterationCount);
            return recalledPatter;
        }

        /// <summary>
        /// The activation function of the multiflop network.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static double multiflopNetworkActivationFunction(double input, double evaluationProgressRatio)
        {
            return input > 0 ? 1.0 : 0.0;
        }

        /// <summary>
        /// Trains a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        private void TrainNeuron(int neuronIndex)
        {
            // No validation here.

            double neuronBias = 1.0;
            _hopfieldNetwork.SetNeuronBias(neuronIndex, neuronBias);

            // Train the synapses.
            for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
            {
                if (sourceNeuronIndex == neuronIndex)
                {
                    continue;
                }

                TrainSynapse(neuronIndex, sourceNeuronIndex);
            }
        }

        /// <summary>
        /// Trains a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        private void TrainSynapse(int neuronIndex, int sourceNeuronIndex)
        {
            // No validation here.

            double synapseWeight = -2.0;
            _hopfieldNetwork.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        private int NeuronCount
        {
            get
            {
                return _hopfieldNetwork.NeuronCount;
            }
        }

        /// <summary>
        /// The underlying Hopfield network.
        /// </summary>
        private NeuralNetwork.HopfieldNetwork.HopfieldNetwork _hopfieldNetwork;
    }
}
