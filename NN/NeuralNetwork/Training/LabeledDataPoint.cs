using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Supervised learning
    public class LabeledDataPoint<TInput, TOutput> : DataPoint<TInput>, ILabeledDataPoint<TInput, TOutput>
    {
        // Tagged
        public LabeledDataPoint(double[] input, TInput inputTag, double[] output, TOutput outputTag, object tag = null)
            : base(input, inputTag, tag)
        {
            Require.IsNotNull(output, nameof(output));

            Output = output;
            OutputTag = outputTag;

            // TODO Data normalization
            //NormalizedOutput = Vector.Normalize(output);
        }

        // Untagged
        public LabeledDataPoint(double[] input, double[] output, object tag = null)
            : this(input, default(TInput), output, default(TOutput), tag)
        {
        }

        public double[] Output { get; }

        public TOutput OutputTag { get; }

        // TODO Data normalization
        //public double[] NormalizedOutput { get; }

        public override string ToString() => $"({Vector.ToString(Input)}, {Vector.ToString(Output)})";
    }

    public class LabeledDataPoint : LabeledDataPoint<object, object>
    {
        // Tagged
        public LabeledDataPoint(double[] input, object inputTag, double[] output, object outputTag, object tag = null)
            : base(input, inputTag, output, outputTag, tag)
        {
        }

        // Untagged
        public LabeledDataPoint(double[] input, double[] output, object tag = null)
            : this(input, null, output, null, tag)
        {
        }
    }

    public interface ILabeledDataPoint : IDataPoint
    {
        double[] Output { get; }

        // TODO Data normalization
        //public double[] NormalizedOutput { get; }
    }

    public interface ILabeledDataPoint<TInput, TOutput> : IDataPoint<TInput>, ILabeledDataPoint
    {
        TOutput OutputTag { get; }
    }
}
