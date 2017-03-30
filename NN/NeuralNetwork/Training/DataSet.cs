using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class DataSet : IEnumerable<LabeledDataPoint>
    {
        private readonly List<LabeledDataPoint> points = new List<LabeledDataPoint>();

        public DataSet(int inputSize, int outputSize, object tag)
        {
            Require.IsPositive(inputSize, nameof(inputSize));
            InputSize = inputSize;

            Require.IsNonNegative(outputSize, nameof(outputSize));
            OutputSize = outputSize;

            Tag = tag;
        }

        public DataSet(int inputSize, int outputSize)
            : this(inputSize, outputSize, null)
        {    
        }

        public DataSet(int vectorLength)
            : this(vectorLength, 0, null)
        {
        }

        public int InputSize { get; }

        public int OutputSize { get; }

        public object Tag { get; }

        public IEnumerable<LabeledDataPoint> RandomPoints
        {
            get
            {
                LabeledDataPoint[] shuffledPoints = points.ToArray();
                StaticRandom.Shuffle(shuffledPoints);
                return shuffledPoints.AsEnumerable();
            }
        }

        public int Size => points.Count;

        public LabeledDataPoint this[int index] => points[index];

        public IEnumerator<LabeledDataPoint> GetEnumerator() => points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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

            this.points.Add(point);
        }

        public void Add((double[] input, double[] output, object tag) point)
        {
            Add(new LabeledDataPoint(point));
        }

        public void Add((double[] input, double[] output) point)
        {
            Add(new LabeledDataPoint(point));
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

        // TODO ???
        //public TrainingSet SeparateTestSet(int index, int size)
        //{
        //    TrainingSet testSet = new TrainingSet(InputVectorLength, OutputVectorLength);
        //    testSet.pattern.AddRange(pattern.GetRange(index, size));
        //    pattern.RemoveRange(index, size);
        //    return testSet;
        //}

        public override string ToString() => $"{{\n{String.Join("\n", points.Select((p, i) => $"\t{i}: {p}"))}\n}}";
    }
}
