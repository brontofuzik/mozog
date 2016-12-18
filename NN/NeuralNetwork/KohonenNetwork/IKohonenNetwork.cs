using System.Drawing;

namespace NeuralNetwork.KohonenNetwork
{
    public interface IKohonenNetwork
    {
        event TrainingPatternEventhandler BeginTrainingPatternEvent;

        event TrainingSetEventHandler BeginTrainingSetEvent;

        event TrainingPatternEventhandler EndTrainingPatternEvent;

        event TrainingSetEventHandler EndTrainingSetEvent;

        int InputNeuronCount
        {
            get;
        }

        int OutputNeuronCount
        {
            get;
        }

        /// <summary>
        /// Trains the network with a training set for a number of iterations.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="trainingIterationCount">The number of training iterations.</param>
        void Train(MultilayerPerceptron.Training.TrainingSet trainingSet, int trainingIterationCount);

        /// <summary>
        /// Evaluates the network.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <returns>The coordinates of the output.</returns>
        int[] Evaluate(double[] inputVector);

        /// <summary>
        /// Gets the weights of an output neuron's synapses.
        /// </summary>
        /// <param name="outputNeuronIndex">The index of the neuron.</param>
        /// <returns>The weights of the output neuron's synapses.</returns>
        double[] GetOutputNeuronSynapseWeights(int outputNeuronIndex);

        /// <summary>
        /// Gets the weights of an output neuron's synapses.
        /// </summary>
        /// <param name="outputNeuronCoordinates">The coordinates of the </param>
        /// <returns>The weights of the output neuron's synapses.</returns>
        double[] GetOutputNeuronSynapseWeights(int[] outputNeuronCoordinates);

        /// <summary>
        /// Converts the network to its bitmap representation.
        /// </summary>
        /// <param name="bitmapWidth">The width of the bitmap.</param>
        /// <param name="bitmapHeight">The height of the bitmap.</param>
        /// <returns>The bitmap representation of the network.</returns>
        Bitmap ToBitmap(int bitmapWidth, int bitmapHeight);
    }
}
