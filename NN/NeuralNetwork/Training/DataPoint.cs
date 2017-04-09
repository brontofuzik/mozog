using Mozog.Utils;

namespace NeuralNetwork.Training
{
    // Unsupervised learning
    public class DataPoint<TInput> : IDataPoint<TInput>
    {
        // Tagged
        public DataPoint(double[] input, TInput inputTag = default(TInput), object tag = null)
        {
            Require.IsNotNull(input, nameof(input));

            Input = input;
            InputTag = inputTag;
            Tag = tag;

            // TODO Data normalization
            //NormalizedInput = Vector.Normalize(input)
        }

        // Untagged
        public DataPoint(double[] input, object tag = null)
            : this(input, default(TInput), tag)
        {
        }

        public double[] Input { get; }

        public TInput InputTag { get; set; }

        public object Tag { get; set; }

        // TODO Data normalization
        //public double[] NormalizedInput { get; }

        public override string ToString() => $"({Vector.ToString(Input)})";
    }

    public class DataPoint : DataPoint<object>
    {
        // Tagged
        public DataPoint(double[] input, object inputTag, object tag = null)
            : base(input, inputTag, tag)
        {
        }

        // Untagged
        public DataPoint(double[] input, object tag = null)
            : this(input, null, tag)
        {
        }
    }

    public interface IDataPoint
    {
        double[] Input { get; }

        object Tag { get; }

        // TODO Data normalization
        //public double[] NormalizedInput { get; }
    }

    public interface IDataPoint<TInput> : IDataPoint
    {
        TInput InputTag { get; }
    }
}
