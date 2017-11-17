using System;
using NeuralNetwork.Data;

namespace NeuralNetwork.Kohonen
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
