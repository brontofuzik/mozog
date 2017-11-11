using NeuralNetwork.Data;

namespace NeuralNetwork.HopfieldNet
{
    public interface IHopfieldNetwork
    {
        int Neurons { get; }

        int Synapses { get; }

        double Energy { get; }

        double GetNeuronBias(int neuronIndex);

        void SetNeuronBias(int neuronIndex, double neuronBias);

        double GetSynapseWeight(int neuronIndex, int sourceNeuronIndex);

        void SetSynapseWeight(int neuronIndex, int sourceNeuronIndex, double synapseWeight);

        void Train(DataSet dataSet);

        double[] Evaluate(double[] patternToRecall, int evaluationIterationCount);

        string ToString();
    }
}
