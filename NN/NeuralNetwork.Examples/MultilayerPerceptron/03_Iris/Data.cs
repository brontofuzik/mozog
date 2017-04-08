using System;
using System.IO;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Iris
{
    public class Data
    {
        private const string path = @"D:\projects\_matfyz_\mozog\datasets\Iris.csv";

        public static DataSet Create()
        {
            var data = new DataSet(4, 3);

            foreach (var line in File.ReadLines(path))
            {
                string[] items = line.Split(',');

                var input = new[] { Double.Parse(items[0]), Double.Parse(items[1]), Double.Parse(items[2]), Double.Parse(items[3]) };
                double[] output;
                switch (items[4])
                {
                    case "Iris-setosa":
                        output = new[] { 1.0, 0.0, 0.0 };
                        break;
                    case "Iris-versicolor":
                        output = new[] { 0.0, 1.0, 0.0 };
                        break;
                    case "Iris-virginica":
                        output = new[] { 0.0, 0.0, 1.0 };
                        break;
                    default:
                        output = new[] { 0.0, 0.0, 0.0 };
                        break;
                }

                data.Add(new LabeledDataPoint(input, output, items[4]));
            }

            return data;
        }

        /*
        public class IrisEncoder : IEncoder<double, int>
        {
            public double[] EncodeInput(double input)
            {
                throw new System.NotImplementedException();
            }

            public double[] EncodeOutput(int output)
            {
                throw new System.NotImplementedException();
            }

            public int DecodeOutput(double[] output)
            {
                throw new System.NotImplementedException();
            }
        }
        */
    }
}
