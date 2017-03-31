using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public class EncodedDataSet<TInput, TOutput> : DataSet
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
            Add(new LabeledDataPoint(encoder.EncodeInput(input), encoder.EncodeOutput(output), tag));
        }

        // Untagged
        public void Add(TInput input, TOutput output)
        {
            Add(input, output, null);
        }
    }
}
