using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Supervised learning
    public class LabeledDataPoint : DataPoint
    {
        public LabeledDataPoint(double[] input, double[] output, object tag)
            : base(input, tag)
        {
            Require.IsNotNull(output, nameof(output));

            Output = output;
            NormalizedOutput = Vector.Normalize(output);
        }

        // Tuple
        public LabeledDataPoint((double[] input, double[] output, object tag) point)
            : this(point.input, point.output, point.tag)
        {
        }

        public LabeledDataPoint(double[] input, double[] output)
            : this(input, output, null)
        {
        }

        // Tuple
        public LabeledDataPoint((double[] input, double[] output) pattern)
            : this(pattern.input, pattern.output)
        {
        }

        public double[] Output { get; }

        public double[] NormalizedOutput { get; }

        public override string ToString() => $"({Vector.ToString(Input)}, {Vector.ToString(Output)})";
    }
}
