using System;
using NeuralNetwork.Common;
using NeuralNetwork.Data;

namespace NeuralNetwork.Hopfield
{
    public interface IHopfieldNetwork
    {
        event EventHandler<TrainingEventArgs> EvaluatingIteration;

        event EventHandler<TrainingEventArgs> IterationEvaluated;

        event EventHandler<InitializationEventArgs> NeuronInitialized;

        int[] Dimensions { get; }

        int Neurons { get; }

        // Not used?
        //int Synapses { get; }

        double Energy { get; }

        double GetNeuronBias(int neuron);

        void SetNeuronBias(int neuron, double bias);

        double GetSynapseWeight(int neuron, int source);

        void SetSynapseWeight(int neuron, int source, double weight);

        void Train(IDataSet data, bool batch = true);

        void Train(IDataPoint point);

        double[] Evaluate(double[] input, int iterations);

        string ToString();
    }
}
