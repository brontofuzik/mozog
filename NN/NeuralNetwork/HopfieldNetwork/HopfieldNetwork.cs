using System;
using System.Text;

using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps;
using NeuralNetwork.HopfieldNetwork.HopfieldNetworkImps.SparseHopfieldNetworkImp;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.HopfieldNetwork
{
    public class HopfieldNetwork
        : IHopfieldNetwork
    {
        /// <summary>
        /// Initializes a new HopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons.</param>
        /// <param name="activationFunction">The activation function.</param>
        /// <param name="hopfieldNetworkImpFactory">The Hopfield network implementation factory.</param>
        public HopfieldNetwork(int neuronCount, ActivationFunction activationFunction, IHopfieldNetworkImpFactory hopfieldNetworkImpFactory)
        {
            _hopfieldNetworkImp = hopfieldNetworkImpFactory.CreateHopfieldNetworkImp(neuronCount, activationFunction);
        }

        /// <summary>
        /// Initializes a new HopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons.</param>
        /// <param name="activationFunction">The activation function.</param>
        public HopfieldNetwork(int neuronCount, ActivationFunction activationFunction)
            : this(neuronCount, activationFunction, new SparseHopfieldNetworkImpFactory())
        {
        }

        /// <summary>
        /// Initializes a new HopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons.</param>
        /// <param name="hopfieldNetworkImpFactory">The Hopfield network implementation factory.</param>
        public HopfieldNetwork(int neuronCount, IHopfieldNetworkImpFactory hopfieldNetworkImpFactory)
            : this(neuronCount, activationFunction, hopfieldNetworkImpFactory)
        {
        }

        /// <summary>
        /// Initializes a new HopfieldNetwork class.
        /// </summary>
        /// <param name="neuronCount">The number of neurons.</param>
        public HopfieldNetwork(int neuronCount)
            : this(neuronCount, activationFunction, new SparseHopfieldNetworkImpFactory())
        {
        }

        /// <summary>
        /// Trains the network.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        public void Train(TrainingSet trainingSet)
        {
            // The training set mmust be provided.
            Utilities.RequireObjectNotNull(trainingSet, "trainingSet");

            // The training set must be compatible with the network.
            if (trainingSet.InputVectorLength != NeuronCount)
            {
                throw new ArgumentException("The training set is not compatible with the network.", "trainingSet");
            }

            // Train the neurons.
            for (int neuronIndex = 0; neuronIndex < NeuronCount; ++neuronIndex)
            {
                TrainNeuron(neuronIndex, trainingSet);
            }
        }

        /// <summary>
        /// Gets the bias of a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <returns>The bias of the neuron.</returns>
        public double GetNeuronBias(int neuronIndex)
        {
            #region Preconditions

            // The index of the neuron must be within range.
            Utilities.RequireNumberWithinRange(neuronIndex, "neuronIndex", 0, NeuronCount - 1);

            #endregion // Preconditions

            return _hopfieldNetworkImp.GetNeuronBias(neuronIndex);
        }

        /// <summary>
        /// Sets the bias of a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="neuronBias">The bias of the neuron.</param>
        public void SetNeuronBias(int neuronIndex, double neuronBias)
        {
            #region Preconditions

            // The index of the neuron must be within range.
            Utilities.RequireNumberWithinRange(neuronIndex, "neuronIndex", 0, NeuronCount - 1);

            #endregion // Preconditions

            _hopfieldNetworkImp.SetNeuronBias(neuronIndex, neuronBias);
        }

        /// <summary>
        /// Gets the weight of a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neruon.</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        /// <returns>The weight of the synapse.</returns>
        public double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex)
        {
            #region Preconditions

            // The index of the neuron must be within range.
            Utilities.RequireNumberWithinRange(neuronIndex, "neuronIndex", 0, NeuronCount - 1);

            // The index of the source neuron must be within range.
            Utilities.RequireNumberWithinRange(sourceNeuronIndex, "sourceNeuronIndex", 0, NeuronCount - 1);

            #endregion // Preconditions

            return _hopfieldNetworkImp.GetSynapseWeight(neuronIndex, sourceNeuronIndex);
        }

        /// <summary>
        /// Sets the weight of a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        /// <param name="synapseWeight">The weight of the synapse.</param>
        public void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double synapseWeight)
        {
            #region Preconditions

            // The index of the neuron must be within range.
            Utilities.RequireNumberWithinRange(neuronIndex, "neuronIndex", 0, NeuronCount - 1);

            // The index of the source neuron must be within range.
            Utilities.RequireNumberWithinRange(sourceNeuronIndex, "sourceNeuronIndex", 0, NeuronCount - 1);

            #endregion // Preconditions

            _hopfieldNetworkImp.SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        /// <summary>
        /// Evaluates the Hopfield network.
        /// </summary>
        /// <param name="patternToRecall">The pattern to recall.</param>
        /// <param name="evaluationIterationCount">The number of evaluation iterations.</param>
        /// <returns>The recalled pattern.</returns>
        public double[] Evaluate(double[] patternToRecall, int evaluationIterationCount)
        {
            // Ensure the pattern to recall is compatible with the Hopfield network,
            // i.e. the length of the pattern is equal to the number of neurons in the network.
            if (patternToRecall.Length != NeuronCount)
            {
                throw new ArgumentException("The pattern to recall is not compatible with the Hopfield network.", "patternToRecall");
            }

            // Ensure the number of iterations is not negative.
            if (evaluationIterationCount < 0)
            {
                throw new ArgumentException("The number of iterations cannot be negative.", "iterationCount");
            }

            _hopfieldNetworkImp.SetNetworkInput(patternToRecall);
            for (int iterationIndex = 0; iterationIndex < evaluationIterationCount; ++iterationIndex)
            {
                #if DEBUG
                Console.WriteLine("{0} : network energy = {1:0.000}", iterationIndex, _hopfieldNetworkImp.Energy);
                #endif

                double evaluationProgressRatio = iterationIndex / (double)evaluationIterationCount;
                _hopfieldNetworkImp.Evaluate(evaluationProgressRatio);
            }
            double[] recalledPattern = _hopfieldNetworkImp.GetNetworkOutput();

            return recalledPattern;
        }

        /// <summary>
        /// Converts the Hopfield network to its string representation.
        /// </summary>
        /// <returns>The string representation of the Hopfield network.</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for (int targetNeuronIndex = 0; targetNeuronIndex < NeuronCount; ++targetNeuronIndex)
            {
                sb.Append("[");
                for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
                {
                    if (sourceNeuronIndex == targetNeuronIndex)
                    {
                        double neuronBias = _hopfieldNetworkImp.GetNeuronBias(targetNeuronIndex);
                        sb.Append(neuronBias + " ");
                    }
                    else // (neuronIndex != sourceNeuronIndex)
                    {
                        double synapseWeight = _hopfieldNetworkImp.GetSynapseWeight(sourceNeuronIndex, targetNeuronIndex);
                        sb.Append(synapseWeight + " ");
                    }
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");

                sb.Append("\n");
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        /// <summary>
        /// Gets the number of neurons in the Hopfield network.
        /// </summary>
        /// <value>
        /// The number of neurons in the Hopfield network.
        /// </value>
        public int NeuronCount
        {
            get
            {
                return _hopfieldNetworkImp.NeuronCount;
            }
        }

        /// <summary>
        /// Gets the number of synapses in the Hopfield network.
        /// </summary>
        /// <value>
        /// The number of synapses in the Hopfield network.
        /// </value>
        public int SynapseCount
        {
            get
            {
                return _hopfieldNetworkImp.SynapseCount;
            }
        }

        /// <summary>
        /// Gets the energy of the Hopfield network.
        /// </summary>
        /// <value>
        /// The energy of the Hopfield network.
        /// </value>
        public double Energy
        {
            get
            {
                return _hopfieldNetworkImp.Energy;
            }
        }

        private static double activationFunction(double input, double evalautionProgressRatio)
        {
            return Math.Sign(input);
        }

        /// <summary>
        /// Trains a neuron.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron.</param>
        /// <param name="trainingSet">The training set.</param>
        private void TrainNeuron(int neuronIndex, TrainingSet trainingSet)
        {
            // No validation here.

            double neuronBias = 0.0;
            SetNeuronBias(neuronIndex, neuronBias);

            // Train the synapses.
            for (int sourceNeuronIndex = 0; sourceNeuronIndex < NeuronCount; ++sourceNeuronIndex)
            {
                if (sourceNeuronIndex == neuronIndex)
                {
                    continue;
                }

                TrainSynapse(neuronIndex, sourceNeuronIndex, trainingSet);
            }
        }

        /// <summary>
        /// Trains a synapse.
        /// </summary>
        /// <param name="neuronIndex">The index of the neuron</param>
        /// <param name="sourceNeuronIndex">The index of the source neuron.</param>
        /// <param name="trainingSet">The training set.</param>
        private void TrainSynapse(int neuronIndex, int sourceNeuronIndex, TrainingSet trainingSet)
        {
            // No validation here.

            double synapseWeight = 0.0;
            foreach (SupervisedTrainingPattern trainingPattern in trainingSet)
            {
                synapseWeight += trainingPattern.InputVector[neuronIndex] * trainingPattern.InputVector[sourceNeuronIndex];
            }
            SetSynapseWeight(neuronIndex, sourceNeuronIndex, synapseWeight);
        }

        /// <summary>
        /// The Hopfield network implementation.
        /// </summary>
        private IHopfieldNetworkImp _hopfieldNetworkImp;
    }
}
