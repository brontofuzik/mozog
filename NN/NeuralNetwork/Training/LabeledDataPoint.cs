using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Supervised learning
    public class LabeledDataPoint : DataPoint
    {
        // Tagged
        public LabeledDataPoint(double[] input, double[] output, object tag)
            : base(input, tag)
        {
            Require.IsNotNull(output, nameof(output));

            Output = output;
            NormalizedOutput = Vector.Normalize(output);
        }

        // Untagged
        public LabeledDataPoint(double[] input, double[] output)
            : this(input, output, null)
        {
        }

        public double[] Output { get; }

        public double[] NormalizedOutput { get; }

        public override string ToString() => $"({Vector.ToString(Input)}, {Vector.ToString(Output)})";
    }
}
