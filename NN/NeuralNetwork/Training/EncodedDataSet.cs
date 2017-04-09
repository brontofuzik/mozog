using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public class EncodedDataSet<TInput, TOutput> : DataSet<TInput, TOutput>
    {
        private readonly IEncoder<TInput, TOutput> encoder;

        // Labeled data for supervised training
        public EncodedDataSet(int inputSize, int outputSize, IEncoder<TInput, TOutput> encoder)
            : base(inputSize, outputSize)
        {
            this.encoder = encoder;
        }

        // Unlabeled data for unsupervised training
        public EncodedDataSet(int inputSize, IEncoder<TInput, TOutput> encoder)
            : this(inputSize, 0, encoder)
        {
        }

        // Tagged
        public void Add(TInput input, TOutput output, object tag)
        {
            Add(encoder.EncodeInput(input), input, encoder.EncodeOutput(output), output, tag);
        }

        // Untagged
        public void Add(TInput input, TOutput output)
        {
            Add(input, output, null);
        }
    }

    public static class EncodedDataSet
    {
        public static EncodedDataSet<TInput, TOutput> New<TInput, TOutput>(int inputSize, int outputSize, IEncoder<TInput, TOutput> encoder)
            => new EncodedDataSet<TInput, TOutput>(inputSize, outputSize, encoder);

        public static EncodedDataSet<TInput, TOutput> New<TInput, TOutput>(int inputSize, IEncoder<TInput, TOutput> encoder)
            => new EncodedDataSet<TInput, TOutput>(inputSize, encoder);
    }
}
