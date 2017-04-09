using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Unsupervised learning
    public class DataPoint: IDataPoint
    {
        // Tagged
        public DataPoint(double[] input, object tag = null)
        {
            Require.IsNotNull(input, nameof(input));

            Input = input;
            Tag = tag;

            // TODO Data normalization
            //NormalizedInput = Vector.Normalize(input)
        }

        public double[] Input { get; }

        public object Tag { get; set; }

        // TODO Data normalization
        //public double[] NormalizedInput { get; }

        public override string ToString() => $"({Vector.ToString(Input)})";
    }

    public interface IDataPoint
    {
        double[] Input { get; }

        object Tag { get; }

        // TODO Data normalization
        //public double[] NormalizedInput { get; }
    }
}
