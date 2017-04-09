using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class DataSet<TInput, TOutput> : IDataSet<TInput, TOutput>
    {
        private readonly List<ILabeledDataPoint<TInput, TOutput>> points = new List<ILabeledDataPoint<TInput, TOutput>>();

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

        public ILabeledDataPoint<TInput, TOutput> this[int index] => points[index];

        public IEnumerator<ILabeledDataPoint<TInput, TOutput>> GetEnumerator() => points.GetEnumerator();

        IEnumerator<ILabeledDataPoint<TInput, TOutput>> IEnumerable<ILabeledDataPoint<TInput, TOutput>>.GetEnumerator()
            => GetEnumerator();

        IEnumerator<ILabeledDataPoint> IEnumerable<ILabeledDataPoint>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public IEnumerable<ILabeledDataPoint<TInput, TOutput>> Random()
        {
            var shuffledPoints = points.ToArray();
            StaticRandom.Shuffle(shuffledPoints);
            return shuffledPoints.AsEnumerable();
        }

        IEnumerable<ILabeledDataPoint> IDataSet.Random() => Random();

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

            points.Add((ILabeledDataPoint<TInput, TOutput>)point);
        }

        // Tagged
        public void Add(double[] input, TInput inputTag, double[] output, TOutput outputTag, object tag = null)
        {
            Add(new LabeledDataPoint<TInput, TOutput>(input, inputTag, output, outputTag, tag));
        }

        // Untagged
        public void Add(double[] input, double[] output, object tag = null)
        {
            Add(new LabeledDataPoint<TInput, TOutput>(input, output, tag));
        }

        public void AddRange(IEnumerable<ILabeledDataPoint> points)
        {
            this.points.AddRange((IEnumerable<ILabeledDataPoint<TInput, TOutput>>)points);
        }

        public void Add(IDataSet dataSet)
        {
            Require.IsNotNull(dataSet, nameof(dataSet));
            if (InputSize != dataSet.InputSize || OutputSize != dataSet.OutputSize)
            {
                // TODO Incompatible sets.
                throw new ArgumentException();
            }

            points.AddRange((IDataSet<TInput, TOutput>)dataSet);
        }

        public bool Contains(ILabeledDataPoint point)
            => points.Contains(point);

        public bool Remove(ILabeledDataPoint point)
            => points.Remove((ILabeledDataPoint<TInput, TOutput>)point);

        public void Clear()
        {
            points.Clear();
        }

        public override string ToString() => $"{{\n{String.Join("\n", points.Select((p, i) => $"\t{i}: {p}"))}\n}}";
    }

    public class DataSet : DataSet<object, object>
    {
        public DataSet(int inputSize, int outputSize = 0)
            : base(inputSize, outputSize)
        {
        }

        public new IEnumerator<ILabeledDataPoint> GetEnumerator() => base.GetEnumerator();
    }

    public interface IDataSet : IEnumerable<ILabeledDataPoint>
    {
        int InputSize { get; }

        int OutputSize { get; }

        int Size { get; }

        IEnumerable<ILabeledDataPoint> Random();

        void Add(ILabeledDataPoint point);

        void Add(double[] input, double[] output, object tag = null);

        void AddRange(IEnumerable<ILabeledDataPoint> points);

        void Add(IDataSet dataSet);

        bool Contains(ILabeledDataPoint point);

        bool Remove(ILabeledDataPoint point);

        void Clear();
    }

    public interface IDataSet<TInput, TOutput> : IDataSet, IEnumerable<ILabeledDataPoint<TInput, TOutput>>
    {
        ILabeledDataPoint<TInput, TOutput> this[int index] { get; }

        new IEnumerable<ILabeledDataPoint<TInput, TOutput>> Random();

        void Add(double[] input, TInput inputTag, double[] output, TOutput outputTag, object tag = null);
    }
}
