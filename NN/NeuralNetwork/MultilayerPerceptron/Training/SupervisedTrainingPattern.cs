using System;
using Mozog.Utils;

namespace NeuralNetwork.MultilayerPerceptron.Training
{
	/// <remarks>
    /// <para>
	/// A supervised training pattern.
	/// </para>
    /// <para>
    /// Definition: A supervised training pattern is a pair <em>(inputVector, outputVector)</em> where
    /// <em>inputVector</em> is the input vector and <em>outputVector</em> is the desired output vector of the pattern.
    /// </para>
	/// </remarks>
    public class SupervisedTrainingPattern
        : UnsupervisedTrainingPattern
    {
        /// <summary>
        /// Initializes a new instance of the SupervisedTrainingPattern class.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <param name="outputVector">The output vector.</param>
        /// <param name="tag">The tag.</param>
        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector, object tag)
            : base(inputVector, tag)
        {
            Require.IsNotNull(outputVector, nameof(outputVector));

            _outputVector = outputVector;
            _normalizedOutputVector = NormalizeVector(outputVector);
        }

        /// <summary>
        /// Initializes a new instance of the SupervisedTrainingPattern class.
        /// </summary>
        /// <param name="inputVector">The input vector.</param>
        /// <param name="outputVector">The output vector.</param>
        public SupervisedTrainingPattern(double[] inputVector, double[] outputVector)
            : this(inputVector, outputVector, null)
        {
        }

        /// <summary>
        /// Converts the training pattern to its string representation.
        /// </summary>
        /// 
        /// <returns>
        /// The training pattern's string representation.
        /// </returns>
        public override string ToString()
        {
            return String.Format("({0}, {1})", VectorToString(InputVector), VectorToString(_outputVector));
            
            // alternatively:
            // return (base.ToString() + "(" + VectorToString(inputVector) + "," + VectorToString(outputVector) + ")");
        }

		/// <summary>
		/// Gets the output vector.
		/// </summary>
		/// <value>
        /// The output vector.
        /// </value>
        public double[] OutputVector
        {
            get
            {
                return _outputVector;
            }
        }

        /// <summary>
        /// Gets the normalized output vector.
        /// </summary>
        /// <value>
        /// The normalized output vector.
        /// </value>
        public double[] NormalizedOutputVector
        {
            get
            {
                return _normalizedOutputVector;
            }
        }

        /// <summary>
        /// The output vector.
        /// </summary>
        private double[] _outputVector;

        /// <summary>
        /// The normalized output vector.
        /// </summary>
        private double[] _normalizedOutputVector;
    }
}
