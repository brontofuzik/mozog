using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Unsupervised learning
    public class DataPoint
    {
        public DataPoint(double[] input, object tag)
        {
            Require.IsNotNull(input, nameof(input));

            Input = input;
            NormalizedInput = Vector.Normalize(input);
            Tag = tag;
        }

        public DataPoint(double[] input)
            : this(input, null)
        {
        }

        public double[] Input { get; }

        public double[] NormalizedInput { get; }

        public object Tag { get; set; }

        public override string ToString() => $"({Vector.ToString(Input)})";
    }
}
