using System;
using NeuralNetwork.Training;

namespace NeuralNetwork.KohonenNetwork
{
    public class DataSetEventArgs : EventArgs
    {
        public DataSetEventArgs(DataSet dataSet, int iteration)
        {
            DataSet = dataSet;
            Iteration = iteration;
        }

        public DataSet DataSet { get; }

        public int Iteration { get; }
    }
}
