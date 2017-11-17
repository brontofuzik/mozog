using System;
using NeuralNetwork.Data;

namespace NeuralNetwork.Kohonen
{
    public class DataSetEventArgs : EventArgs
    {
        public DataSetEventArgs(IDataSet dataSet, int iteration)
        {
            DataSet = dataSet;
            Iteration = iteration;
        }

        public IDataSet DataSet { get; }

        public int Iteration { get; }
    }
}
