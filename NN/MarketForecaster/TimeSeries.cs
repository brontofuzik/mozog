using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeuralNetwork.Data;

namespace MarketForecaster
{
    class TimeSeries
    {
        private const char separator = ' ';
        private readonly double[] data;

        public TimeSeries(string fileName)
        {
            var dataList = new List<double>();

            var lines = File.ReadLines(fileName);
            foreach (var line in lines)
            {
                if (line.Trim().Length == 0)
                    continue;

                dataList.AddRange(line.Trim().Split(separator).Select(Double.Parse));
            }

            data = dataList.ToArray();
        }

        //public double this[int index] => data[index];

        private int Length => data.Length;

        public DataSet BuildTrainingSet(int[] lags, int[] leaps)
        {
            var trainingSet = new DataSet(lags.Length, leaps.Length);

            // The following assumes that lags and leaps are ordered in ascending fashion.
            int maxLag = lags[0];
            int maxLeap = leaps[leaps.Length - 1];

            for (int i = -maxLag; i < Length - maxLeap; i++)
            {
                var input = new double[lags.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    input[j] = data[i + lags[j]];
                }

                var output = new double[leaps.Length];
                for (int j = 0; j < output.Length; j++)
                {
                    output[j] = data[i + leaps[j]];
                }

                trainingSet.Add(new LabeledDataPoint(input, output));
            }

            return trainingSet;
        }
    }
}
