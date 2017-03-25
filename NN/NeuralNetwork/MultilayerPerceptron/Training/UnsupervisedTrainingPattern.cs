using System;
using System.Linq;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
    /// <remarks>
    /// An unsupervised training pattern is an inputVector.
    /// </remarks>
    public class UnsupervisedTrainingPattern
    {
        public UnsupervisedTrainingPattern(double[] inputVector, object tag)
        {
            Require.IsNotNull(inputVector, nameof(inputVector));

            InputVector = inputVector;
            NormalizedInputVector = NormalizeVector(inputVector);
            Tag = tag;
        }

        public UnsupervisedTrainingPattern(double[] inputVector)
            : this(inputVector, null)
        {
        }

        public double[] InputVector { get; }

        public double[] NormalizedInputVector { get; }

        public object Tag { get; set; }

        public static double[] NormalizeVector(double[] vector, double magnitude)
        {
            Require.IsNotNull(vector, nameof(vector));
            Require.IsNonNegative(magnitude, nameof(magnitude));

            // Calculate the sum of squares.
            double sumOfSquares = 0d;
            for (int i = 0; i < vector.Length; ++i)
            {
                sumOfSquares += vector[i] * vector[i];
            }

            // Calculate the normalization neighbourhoodRadiusParameter.
            double factor = sumOfSquares != 0 ? Math.Sqrt(magnitude / sumOfSquares) : 0;

            // Calculate the normalized vector 
            double[] normalizedVector = new double[vector.Length];
            for (int i = 0; i < normalizedVector.Length; i++)
            {
                normalizedVector[i] = vector[i] * factor;
            }

            return normalizedVector;
        }

        public static double[] NormalizeVector(double[] vector) => NormalizeVector(vector, 1.0);

        public static string VectorToString(double[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));

            return $"[{String.Join(", ", vector.Select(e => e.ToString("F2")))}]";
        }

        public override string ToString() => $"({VectorToString(InputVector)})";
    }
}
