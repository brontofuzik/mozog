using System.Collections.Generic;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Data
{
    public static class ClassificationData
    {
        public static ClassificationData<TInput> New<TInput>(IEncoder<TInput, int> encoder, int inputSize, int outputSize)
            => new ClassificationData<TInput>(encoder, inputSize, outputSize);
    }

    public class ClassificationData<TInput> : EncodedData<TInput, int>, IClassificationData
    {
        public ClassificationData(IEncoder<TInput, int> encoder, int inputSize, int outputSize)
            : base(encoder, inputSize, outputSize)
        {
        }

        public new (double[] input, int @class) this[int index]
        {
            get
            {
                var point = base[index];
                return (point.Input, point.OutputTag);
            }
        }

        public new IEnumerator<(double[] input, int @class)> GetEnumerator()
        {
            using (var enumerator = base.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    var point = enumerator.Current;
                    yield return (point.Input, point.OutputTag);
                }
            }
        }

        //IEnumerator<(double[] input, int @class)> IEnumerable<(double[] input, int @class)>.GetEnumerator()
        //    => GetEnumerator();

        public int OutputToClass(double[] output) => encoder.DecodeOutput(output);

        public override IDataSet CreateNewSet() => new ClassificationData<TInput>(encoder, InputSize, OutputSize);
    }

    public interface IClassificationData : IDataSet //, IEnumerable<(double[] input, int @class)>
    {
        new (double[] input, int @class) this[int index] { get; }

        new IEnumerator<(double[] input, int @class)> GetEnumerator();

        int OutputToClass(double[] output);
    }
}
