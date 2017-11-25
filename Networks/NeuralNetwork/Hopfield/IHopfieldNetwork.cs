using NeuralNetwork.Data;

namespace NeuralNetwork.Hopfield
{
    public interface IHopfieldNetwork
    {
        int[] Dimensions { get; }

        int Neurons { get; }

        int Synapses { get; }

        double Energy { get; }

        double GetNeuronBias(int neuron);

        void SetNeuronBias(int neuron, double bias);

        double GetSynapseWeight(int neuron, int source);

        void SetSynapseWeight(int neuron, int source, double weight);

        void Train(DataSet dataSet);

        double[] Evaluate(double[] input, int iterations);

        string ToString();
    }
}
