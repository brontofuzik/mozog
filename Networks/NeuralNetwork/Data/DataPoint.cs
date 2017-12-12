using Mozog.Utils;
using Mozog.Utils.Math;

namespace NeuralNetwork.Data
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

        public double this[int index] => Input[index];

        // TODO Data normalization
        //public double[] NormalizedInput { get; }

        public override string ToString() => $"({Vector.ToString(Input)})";
    }

    public interface IDataPoint
    {
        double[] Input { get; }

        object Tag { get; }

        double this[int index] { get; }

        // TODO Data normalization
        //public double[] NormalizedInput { get; }
    }
}
