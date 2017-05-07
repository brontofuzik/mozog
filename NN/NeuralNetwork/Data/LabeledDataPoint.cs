using Mozog.Utils;
using Mozog.Utils.Math;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Data
{
    // Supervised learning
    public class LabeledDataPoint : DataPoint, ILabeledDataPoint
    {
        // Tagged
        public LabeledDataPoint(double[] input, double[] output, object tag = null)
            : base(input, tag)
        {
            Require.IsNotNull(output, nameof(output));

            Output = output;

            // TODO Data normalization
            //NormalizedOutput = Vector.Normalize(output);
        }

        public double[] Output { get; }

        // TODO Data normalization
        //public double[] NormalizedOutput { get; }

        public override string ToString() => $"({Vector.ToString(Input)}, {Vector.ToString(Output)})";
    }

    public interface ILabeledDataPoint : IDataPoint
    {
        double[] Output { get; }

        // TODO Data normalization
        //public double[] NormalizedOutput { get; }
    }

    public class EncodedDataPoint<TInput, TOutput> : LabeledDataPoint, IEncodedDataPoint<TInput, TOutput>
    {
        private readonly IEncoder<TInput, TOutput> encoder;

        public EncodedDataPoint(IEncoder<TInput, TOutput> encoder, TInput input, TOutput output, object tag = null)
            : base(encoder.EncodeInput(input), encoder.EncodeOutput(output), tag)
        {
            this.encoder = encoder;
        }

        // TODO Data encoding
        public TInput InputTag => encoder.DecodeInput(Input);

        public TOutput OutputTag => encoder.DecodeOutput(Output);
    }

    public interface IEncodedDataPoint<TInput, TOutput> : ILabeledDataPoint
    {
        TInput InputTag { get; }

        TOutput OutputTag { get; }
    }
}
