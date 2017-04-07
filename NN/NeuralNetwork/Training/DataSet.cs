using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class DataSet : IEnumerable<LabeledDataPoint>
    {
        private readonly List<LabeledDataPoint> points = new List<LabeledDataPoint>();

        // Labeled data for supervised training
        public DataSet(int inputSize, int outputSize)
        {
            Require.IsPositive(inputSize, nameof(inputSize));
            InputSize = inputSize;

            Require.IsNonNegative(outputSize, nameof(outputSize));
            OutputSize = outputSize;
        }

        // Unlabeled data for unsupervised training
        public DataSet(int inputSize)
            : this(inputSize, 0)
        {
        }

        public int InputSize { get; }

        public int OutputSize { get; }

        public int Size => points.Count;

        public LabeledDataPoint this[int index] => points[index];

        public IEnumerator<LabeledDataPoint> GetEnumerator() => points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<LabeledDataPoint> GetPointsInRandomOrder()
        {
            var shuffledPoints = points.ToArray();
            StaticRandom.Shuffle(shuffledPoints);
            return shuffledPoints.AsEnumerable();
        }

        public void Add(LabeledDataPoint point)
        {
            Require.IsNotNull(point, nameof(point));
            if (InputSize != point.Input.Length)
            {
                throw new ArgumentException("The input vector must be of size " + InputSize, nameof(point));
            }
              
            if (OutputSize != point.Output.Length)
            {
                throw new ArgumentException("The output vector must be of size " + OutputSize, nameof(point));
            }

            points.Add(point);
        }

        // Tagged
        public void Add(double[] input, double[] output, object tag)
        {
            Add(new LabeledDataPoint(input, output, tag));
        }

        // Untagged
        public void Add(double[] input, double[] output)
        {
            Add(new LabeledDataPoint(input, output));
        }

        public DataSet AddRange(IEnumerable<LabeledDataPoint> points)
        {
            this.points.AddRange(points);
            return this;
        }

        public void Add(DataSet dataSet)
        {
            Require.IsNotNull(dataSet, nameof(dataSet));
            if (InputSize != dataSet.InputSize || OutputSize != dataSet.OutputSize)
            {
                // TODO Incompatible sets.
                throw new ArgumentException();
            }

            points.AddRange(dataSet.points);
        }

        public bool Contains(LabeledDataPoint point) => points.Contains(point);

        public bool Remove(LabeledDataPoint point)
            => points.Remove(point);

        public void Clear()
        {
            points.Clear();
        }

        public override string ToString() => $"{{\n{String.Join("\n", points.Select((p, i) => $"\t{i}: {p}"))}\n}}";
    }
}
