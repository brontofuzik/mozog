using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public class DataPointEventArgs : EventArgs
    {
        public DataPointEventArgs(LabeledDataPoint dataPoint, int interation)
        {
            DataPoint = dataPoint;
            Iteration = interation;
        }

        public LabeledDataPoint DataPoint { get; }

        public int Iteration { get; }
    }
}
