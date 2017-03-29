using System;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.Training
{
    public class UnsupervisedTrainingPattern
    {
        public UnsupervisedTrainingPattern(double[] inputVector, object tag)
        {
            Require.IsNotNull(inputVector, nameof(inputVector));

            InputVector = inputVector;
            NormalizedInputVector = Vector.Normalize(inputVector);
            Tag = tag;
        }

        public UnsupervisedTrainingPattern(double[] inputVector)
            : this(inputVector, null)
        {
        }

        public double[] InputVector { get; }

        public double[] NormalizedInputVector { get; }

        public object Tag { get; set; }

        public override string ToString() => $"({Vector.ToString(InputVector)})";
    }
}
