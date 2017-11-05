using System;
using Mozog.Utils.Math;

namespace NeuralNetwork.Examples.HopfieldNet
{
    class Multiflop
    {
        public static void Run()
        {
            Console.WriteLine("TestMultiflopNetwork");
            Console.WriteLine("====================");

            // --------------------------------
            // Step 1: Create the training set.
            // --------------------------------

            // Do nothing.

            // ---------------------------
            // Step 2: Create the network.
            // ---------------------------

            int neuronCount = 4;
            Multiflop multiflopNetwork = new Multiflop(neuronCount);

            // --------------------------
            // Step 3: Train the network.
            // --------------------------

            multiflopNetwork.Train();

            // -------------------------
            // Step 4: Test the network.
            // -------------------------

            int iterationCount = 10;
            double[] recalledPattern = multiflopNetwork.Evaluate(iterationCount);

            Console.WriteLine(Vector.ToString(recalledPattern));

            Console.WriteLine();
        }

        /// <summary>
        /// Initializes a new instance of the MultiflopHopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount"></param>
        public Multiflop(int neuronCount)
        {
            _hopfieldNetwork = new NeuralNetwork.HopfieldNet.HopfieldNetwork(neuronCount, false, multiflopNetworkActivationFunction);
        }

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
        private NeuralNetwork.HopfieldNet.HopfieldNetwork _hopfieldNetwork;
    }
}
