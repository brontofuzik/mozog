using System;
using System.IO;
using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron.Iris
{
    public class Data
    {
        private const string path = @"D:\projects\_matfyz_\mozog\datasets\Iris.csv";

        public static readonly IEncoder<double[], int> Encoder = new IrisEncoder();

        public static IEncodedData<double[], int> Create()
        {
            var data = ClassificationData.New(Encoder, 4, 3);

            foreach (var line in File.ReadLines(path))
            {
                string[] items = line.Split(',');

                var input = new[] { Double.Parse(items[0]), Double.Parse(items[1]), Double.Parse(items[2]), Double.Parse(items[3]) };
                int output;
                switch (items[4])
                {
                    case "Iris-setosa":
                        output = 0;
                        break;
                    case "Iris-versicolor":
                        output = 1;
                        break;
                    case "Iris-virginica":
                        output = 2;
                        break;
                    default:
                        output = -1;
                        break;
                }

                data.Add(input, output, tag: output);
            }

            return data;
        }

        public class IrisEncoder : IEncoder<double[], int>
        {
            public double[] EncodeInput(double[] input) => input;

            public double[] DecodeInput(double[] input)
            {
                // Not needed
                throw new NotImplementedException();
            }

            public double[] EncodeOutput(int @class)
                => Vector.IndexToVector(@class, 3);

            public int DecodeOutput(double[] output)
                => Vector.VectorToIndex(output);
        }
    }
}
