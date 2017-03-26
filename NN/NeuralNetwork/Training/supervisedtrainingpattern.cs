using Mozog.Utils;

namespace NeuralNetwork.Training
{
	/// <remarks>
    /// A supervised training pattern is a pair <em>(inputVector, outputVector)</em> where
    /// <em>inputVector</em> is the input vector and <em>outputVector</em> is the desired output vector of the pattern.
	/// </remarks>
    public class SupervisedTrainingPattern : UnsupervisedTrainingPattern
    {
        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector, object tag)
            : base(inputVector, tag)
        {
            Require.IsNotNull(outputVector, nameof(outputVector));

            OutputVector = outputVector;
            NormalizedOutputVector = NormalizeVector(outputVector);
        }

        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector)
            : this(inputVector, outputVector, null)
        {
        }

        public double[] OutputVector { get; }

        public double[] NormalizedOutputVector { get; }

        public override string ToString() => $"({VectorToString(InputVector)}, {VectorToString(OutputVector)})";
    }
}
