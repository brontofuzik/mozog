using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class DataSet : IDataSet
    {
        private readonly List<ILabeledDataPoint> points = new List<ILabeledDataPoint>();

        public DataSet(int inputSize, int outputSize = 0)
        {
            Require.IsPositive(inputSize, nameof(inputSize));
            InputSize = inputSize;

            Require.IsNonNegative(outputSize, nameof(outputSize));
            OutputSize = outputSize;
        }

        public int InputSize { get; }

        public int OutputSize { get; }

        public int Size => points.Count;

        public ILabeledDataPoint this[int index] => points[index];

        public IEnumerator<ILabeledDataPoint> GetEnumerator() => points.GetEnumerator();

        IEnumerator<ILabeledDataPoint> IEnumerable<ILabeledDataPoint>.GetEnumerator() => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<ILabeledDataPoint> Random()
        {
            var shuffledPoints = points.ToArray();
            StaticRandom.Shuffle(shuffledPoints);
            return shuffledPoints.AsEnumerable();
        }

        public void Add(ILabeledDataPoint point)
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

        // Untagged
        public void Add(double[] input, double[] output, object tag = null)
        {
            Add(new LabeledDataPoint(input, output, tag));
        }

        public IDataSet AddRange(IEnumerable<ILabeledDataPoint> points)
        {
            this.points.AddRange(points);
            return this;
        }

        public void Add(IDataSet dataSet)
        {
            Require.IsNotNull(dataSet, nameof(dataSet));
            if (InputSize != dataSet.InputSize || OutputSize != dataSet.OutputSize)
            {
                // TODO Incompatible sets.
                throw new ArgumentException();
            }

            points.AddRange(dataSet);
        }

        public bool Contains(ILabeledDataPoint point)
            => points.Contains(point);

        public bool Remove(ILabeledDataPoint point)
            => points.Remove(point);

        public void Clear()
        {
            points.Clear();
        }

        public virtual IDataSet CreateNewSet() => new DataSet(InputSize, OutputSize);

        public override string ToString() => $"{{\n{String.Join("\n", points.Select((p, i) => $"\t{i}: {p}"))}\n}}";

    }

    public interface IDataSet : IEnumerable<ILabeledDataPoint>
    {
        int InputSize { get; }

        int OutputSize { get; }

        int Size { get; }

        ILabeledDataPoint this[int index] { get; }

        IEnumerable<ILabeledDataPoint> Random();

        void Add(ILabeledDataPoint point);

        void Add(double[] input, double[] output, object tag = null);

        IDataSet AddRange(IEnumerable<ILabeledDataPoint> points);

        void Add(IDataSet dataSet);

        bool Contains(ILabeledDataPoint point);

        bool Remove(ILabeledDataPoint point);

        void Clear();

        IDataSet CreateNewSet();
    }
}
