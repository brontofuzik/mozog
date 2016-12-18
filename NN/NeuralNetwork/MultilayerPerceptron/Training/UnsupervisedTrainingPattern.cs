using System;
using System.Text;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
    /// <remarks>
    /// <para>
    /// An unsupervised training pattern.
    /// </para>
    /// <para>
    /// Definition: An unsupervised training pattern is an inputVector.
    /// </para>
    /// </remarks>
    public class UnsupervisedTrainingPattern
    {
        /// <summary>
        /// Initializes a new instance of the UnsupervisedTrainingPattern class.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <param name="tag">The tag.</param>
        public UnsupervisedTrainingPattern(double[] inputVector, object tag)
        {
            #region Preconditions

            // The input vector must be provided.
            Utilities.RequireObjectNotNull(inputVector, "inputVector");

            #endregion // Preconditions

            _inputVector = inputVector;
            _normalizedInputVector = NormalizeVector(inputVector);

            _tag = tag;
        }

        /// <summary>
        /// Initializes a new instance of the UnsupervisedTrainingPattern class.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        public UnsupervisedTrainingPattern(double[] inputVector)
            : this(inputVector, null)
        {
        }

        /// <summary>
        /// Normalizes a vector of numbers to the given magnitude.
        /// </summary>
        /// <param name="vector">The vector to be normalized.</param>
        /// <param name="magnitude">The magnitude of the normalized vector.</param>
        /// <returns>The normalized vector of the given magnitude.</returns>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Condition: <c>magnitude</c> is negative. 
        /// </exception>
        public static double[] NormalizeVector(double[] vector, double magnitude)
        {
            #region Preconditions

            // The vector must be provided.
            Utilities.RequireObjectNotNull(vector, "vector");

            // The magnitude must be non-negative.
            Utilities.RequireNumberNonNegative(magnitude, "magnitude");

            #endregion // Preconditions

            // Calculate the sum of squares.
            double sumOfSquares = 0d;
            for (int i = 0; i < vector.Length; ++i)
            {
                sumOfSquares += vector[i] * vector[i];
            }

            // Calculate the normalization neighbourhoodRadiusParameter.
            double factor = (sumOfSquares != 0) ? Math.Sqrt(magnitude / sumOfSquares) : 0;

            // Calculate the normalized vector 
            double[] normalizedVector = new double[vector.Length];
            for (int i = 0; i < normalizedVector.Length; i++)
            {
                normalizedVector[i] = vector[i] * factor;
            }

            return normalizedVector;
        }

        /// <summary>
        /// Normalizes a vector of numbers to the magnitude of 1.
        /// </summary>
        /// <param name="vector">The vector to be normalized.</param>
        /// <returns>The normalized vector of the magnitude 1.</returns>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        public static double[] NormalizeVector(double[] vector)
        {
            return NormalizeVector(vector, 1.0);
        }

        /// <summary>
        /// Returns a string that represents the given vector.
        /// </summary>
        /// <param name="vector">The vector to be represented by string.</param>
        /// <returns>
        /// A string that represents the given vector.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>vector</c> is <c>null</c>.
        /// </exception>
        public static string VectorToString(double[] vector)
        {
            #region Preconditions

            // The vector must be provided.
            Utilities.RequireObjectNotNull(vector, "vector");

            #endregion // Preconditions

            StringBuilder vectorStringBuilder = new StringBuilder();

            vectorStringBuilder.Append("[");
            foreach (double element in vector)
            {
                vectorStringBuilder.Append(element.ToString("F2") + ", ");
            }
            if (vector.Length > 0)
            {
                vectorStringBuilder.Remove(vectorStringBuilder.Length - 2, 2);
            }
            vectorStringBuilder.Append("]");

            return vectorStringBuilder.ToString();
        }

        /// <summary>
        /// Converts the training pattern to its string representation.
        /// </summary>
        /// <returns>
        /// The training pattern's string representation.
        /// </returns>
        public override string ToString()
        {
            return String.Format("({0})", VectorToString(_inputVector));

            // alternatively:
            // return (base.ToString() + "(" + VectorToString( inputVector ) + "," + VectorToString( outputVector ) + ")");
        }

        /// <summary>
        /// Gets the input vector.
        /// </summary>
        /// <value>
        /// The input vector.
        /// </value>
        public double[] InputVector
        {
            get
            {
                return _inputVector;
            }
        }

        /// <summary>
        /// Gets the normalized input vector.
        /// </summary>
        /// <value>
        /// The normalized input vector.
        /// </value>
        public double[] NormalizedInputVector
        {
            get
            {
                return _normalizedInputVector;
            }
        }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }
        
        /// <summary>
        /// The input vector.
        /// </summary>
        private double[] _inputVector;

        /// <summary>
        /// The normalized input vector.
        /// </summary>
        private double[] _normalizedInputVector;

        /// <summary>
        /// The tag.
        /// </summary>
        private object _tag;
    }
}
