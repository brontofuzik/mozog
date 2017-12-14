﻿using System;
using System.Drawing;
using NeuralNetwork.Common;
using NeuralNetwork.Data;

namespace NeuralNetwork.Kohonen
{
    public interface IKohonenNetwork
    {
        event EventHandler<TrainingEventArgs> TrainingIteration;

        event EventHandler<TrainingEventArgs> IterationTrained;

        int InputSize { get; }

        int OutputSize { get; }

        void Train(DataSet data, int iterations);

        int[] Evaluate(double[] input);

        double[] GetOutputNeuronSynapseWeights(int neuron);

        double[] GetOutputNeuronSynapseWeights(int[] neuron);

        Bitmap ToBitmap(int width, int height);
    }
}
