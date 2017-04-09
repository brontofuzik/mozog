using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public static class EncodedDataSet
    {
        public static EncodedDataSet<TInput, TOutput> New<TInput, TOutput>(int inputSize, int outputSize, IEncoder<TInput, TOutput> encoder)
            => new EncodedDataSet<TInput, TOutput>(inputSize, outputSize, encoder);

        public static EncodedDataSet<TInput, TOutput> New<TInput, TOutput>(int inputSize, IEncoder<TInput, TOutput> encoder)
            => new EncodedDataSet<TInput, TOutput>(inputSize, encoder);
    }

    public class EncodedDataSet<TInput, TOutput> : DataSet, IEncodedDataSet<TInput, TOutput>
    {
        public IEncoder<TInput, TOutput> Encoder { get; }

        // Labeled data for supervised training
        public EncodedDataSet(int inputSize, int outputSize, IEncoder<TInput, TOutput> encoder)
            : base(inputSize, outputSize)
        {
            Encoder = encoder;
        }

        // Unlabeled data for unsupervised training
        public EncodedDataSet(int inputSize, IEncoder<TInput, TOutput> encoder)
            : this(inputSize, 0, encoder)
        {
        }

        public IEncodedDataPoint<TInput, TOutput> this[int index]
            => (IEncodedDataPoint<TInput, TOutput>)base[index];

        public void Add(TInput input, TOutput output, object tag = null)
        {
            Add(new EncodedDataPoint<TInput, TOutput>(Encoder, input, output, tag));
        }
    }

    public interface IEncodedDataSet<TInput, TOutput> : IDataSet
    {
        IEncoder<TInput, TOutput> Encoder { get; }

        new IEncodedDataPoint<TInput, TOutput> this[int index] { get; }

        void Add(TInput inputTag, TOutput outputTag, object tag = null);
    }
}
