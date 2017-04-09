using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public class DataPointEventArgs : EventArgs
    {
        public DataPointEventArgs(ILabeledDataPoint dataPoint, int interation)
        {
            DataPoint = dataPoint;
            Iteration = interation;
        }

        public ILabeledDataPoint DataPoint { get; }

        public int Iteration { get; }
    }
}
