using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class SupervisedTrainingPattern : UnsupervisedTrainingPattern
    {
        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector, object tag)
            : base(inputVector, tag)
        {
            Require.IsNotNull(outputVector, nameof(outputVector));

            OutputVector = outputVector;
            NormalizedOutputVector = Vector.Normalize(outputVector);
        }

        // Tuple
        public SupervisedTrainingPattern((double[] input, double[] output, object tag) pattern)
            : this(pattern.input, pattern.output, pattern.tag)
        {
        }

        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector)
            : this(inputVector, outputVector, null)
        {
        }

        // Tuple
        public SupervisedTrainingPattern((double[] input, double[] output) pattern)
            : this(pattern.input, pattern.output)
        {
        }

        public double[] OutputVector { get; }

        public double[] NormalizedOutputVector { get; }

        public override string ToString() => $"({Vector.ToString(InputVector)}, {Vector.ToString(OutputVector)})";
    }
}
