using System.Collections.Generic;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public static class EncodedData
    {
        public static EncodedData<TInput, TOutput> New<TInput, TOutput>(IEncoder<TInput, TOutput> encoder, int inputSize, int outputSize = 0)
            => new EncodedData<TInput, TOutput>(encoder, inputSize, outputSize);
    }

    public class EncodedData<TInput, TOutput> : DataSet, IEncodedData<TInput, TOutput>
    {
        protected IEncoder<TInput, TOutput> encoder;

        // Labeled data for supervised training
        public EncodedData(IEncoder<TInput, TOutput> encoder, int inputSize, int outputSize = 0)
            : base(inputSize, outputSize)
        {
            this.encoder = encoder;
        }

        public new IEncodedDataPoint<TInput, TOutput> this[int index]
            => (IEncodedDataPoint<TInput, TOutput>)base[index];

        public new IEnumerator<IEncodedDataPoint<TInput, TOutput>> GetEnumerator()
        {
            using (var enumerator = base.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return (IEncodedDataPoint<TInput, TOutput>)enumerator.Current;
                }
            }
        }

        //IEnumerator<IEncodedDataPoint<TInput, TOutput>> IEnumerable<IEncodedDataPoint<TInput, TOutput>>.GetEnumerator()
        //    => GetEnumerator();

        public void Add(TInput input, TOutput output, object tag = null)
        {
            Add(new EncodedDataPoint<TInput, TOutput>(encoder, input, output, tag));
        }

        public override IDataSet CreateNewSet() => new EncodedData<TInput, TOutput>(encoder, InputSize, OutputSize);
    }

    public interface IEncodedData<TInput, TOutput> : IDataSet //, IEnumerable<IEncodedDataPoint<TInput, TOutput>>
    {
        new IEncodedDataPoint<TInput, TOutput> this[int index] { get; }

        new IEnumerator<IEncodedDataPoint<TInput, TOutput>> GetEnumerator();

        void Add(TInput inputTag, TOutput outputTag, object tag = null);
    }
}
