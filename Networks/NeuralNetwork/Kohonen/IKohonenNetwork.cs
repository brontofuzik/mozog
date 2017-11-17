using System;
using System.Drawing;
using NeuralNetwork.Data;

namespace NeuralNetwork.Kohonen
{
    public interface IKohonenNetwork
    {
        event EventHandler<DataSetEventArgs> BeforeTrainingSet;

        event EventHandler<DataSetEventArgs> AfterTrainingSet;

        event EventHandler<DataPointEventArgs> BeforeTrainingPoint;

        event EventHandler<DataPointEventArgs> AfterTrainingPoint;

        int InputNeuronCount { get; }

        int OutputNeuronCount { get; }

        void Train(DataSet trainingSet, int iterations);

        int[] Evaluate(double[] input);

        double[] GetOutputNeuronSynapseWeights(int neuronIndex);

        double[] GetOutputNeuronSynapseWeights(int[] neuronCoordinates);

        Bitmap ToBitmap(int width, int height);
    }
}
