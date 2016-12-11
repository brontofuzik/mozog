using System;

namespace NeuralNetwork
{
    /// <summary>
    /// This (internal static) class represents a collection of static validation routines.
    /// </summary>
    static class Utilities
    {
        #region Static constructor

        /// <summary>
        /// 
        /// </summary>
        static Utilities()
        {
            _random = new Random();
        }

        #endregion // Static constructor


        #region Internal members

        #region Static methods

        #region Require methods

        /// <summary>
        /// Requires that an object is not <c>null</c>.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="objName">The name of the object.</param>
        /// <exception cref="ArgumentNullException">
        /// Condition: <c>obj</c> is <c>null</c>.
        /// </exceptio>
        internal static void RequireObjectNotNull(object obj, string objName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(objName);
            }
        }

        /// <summary>
        /// Requires that a number is non-negative.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c>number</c> is negative.
        /// </exception>
        internal static void RequireNumberNonNegative(double number, string numberName)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(numberName, number, "The number must be non-negative.");
            }
        }

        /// <summary>
        /// Requires that a number is positive.
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <exception cref="ArgumentException">
        /// Condition: <c> number </c> is non-positive.
        /// </exception>
        internal static void RequireNumberPositive(double number, string numberName)
        {
            if (number <= 0)
            {
                throw new ArgumentOutOfRangeException(numberName, number, "The number must be positive.");
            }
        }

        /// <summary>
        /// Requires that a number is within range.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="numberName">The name of the number.</param>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        internal static void RequireNumberWithinRange(double number, string numberName, double minValue, double maxValue)
        {
            if (number < minValue || maxValue < number)
            {
                throw new ArgumentOutOfRangeException(numberName, number, "The number must be within range.");
            }
        }

        #endregion // Require methods

        #region Vector albegra methods

        /// <summary>
        /// Calculates the difference between two vectors of ints.
        /// </summary>
        /// <param name="vector1">The minuend vector.</param>
        /// <param name="vector2">The subtrahend vector.</param>
        /// <returns>The difference between the two vectors.</returns>
        internal static int[] IntVectorDifference(int[] vector1, int[] vector2)
        {
            int[] vectorDifference = new int[vector1.Length];
            for (int i = 0; i < vector1.Length; ++i)
            {
                vectorDifference[i] = vector1[i] - vector2[i];
            }
            return vectorDifference;
        }

        /// <summary>
        /// Calculates the difference between two vectors of doubles.
        /// </summary>
        /// <param name="vector1">The minuend vector.</param>
        /// <param name="vector2">The subtrahend vector.</param>
        /// <returns>The difference between the two vectors.</returns>
        internal static double[] DoubleVectorDifference(double[] vector1, double[] vector2)
        {
            double[] vectorDifference = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; ++i)
            {
                vectorDifference[i] = vector1[i] - vector2[i];
            }
            return vectorDifference;
        }

        /// <summary>
        /// Calculates the dot product of two vectors of ints.
        /// </summary>
        /// <param name="vector1">The first multiplicand vector.</param>
        /// <param name="vector2">The second multiplicand vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        internal static int IntVectorDotProduct(int[] vector1, int[] vector2)
        {
            int vectorDotProduct = 0;
            for (int i = 0; i < vector1.Length; ++i)
            {
                vectorDotProduct += vector1[i] * vector2[i];
            }
            return vectorDotProduct;
        }

        /// <summary>
        /// Calculates the dot product of two vectors of doubles.
        /// </summary>
        /// <param name="vector1">The first multiplicand vector.</param>
        /// <param name="vector2">The second multiplicand vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        internal static double DoubleVectorDotProduct(double[] vector1, double[] vector2)
        {
            double vectorDotProduct = 0;
            for (int i = 0; i < vector1.Length; ++i)
            {
                vectorDotProduct += vector1[i] * vector2[i];
            }
            return vectorDotProduct;
        }

        /// <summary>
        /// Calculates the magnitude of a vector of ints.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The magnitude of the vector.</returns>
        internal static double IntVectorMagnitude(int[] vector)
        {
            return Math.Sqrt(Utilities.IntVectorDotProduct(vector, vector));
        }

        /// <summary>
        /// Calculates the magnitude of a vector of doubles.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The magnitude of the vector.</returns>
        internal static double DoubleVectorMagnitude(double[] vector)
        {
            return Math.Sqrt(Utilities.DoubleVectorDotProduct(vector, vector));
        }

        internal static double DistanceBetweenIntVectors(int[] vector1, int[] vector2)
        {
            return Utilities.IntVectorMagnitude(Utilities.IntVectorDifference(vector1, vector2));
        }

        internal static double DistanceBetweenDoubleVectors(double[] vector1, double[] vector2)
        {
            return Utilities.DoubleVectorMagnitude(Utilities.DoubleVectorDifference(vector1, vector2));
        }

        #endregion // Vector algebra methods

        internal static int Next()
        {
            return _random.Next();
        }

        internal static int Next(int maxValue)
        {
            return _random.Next(maxValue);
        }

        internal static int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The inclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
        /// <returns>
        /// A double-precision floating point number greater than or equal to minValue and less than or equal to maxValue; that is, the range of return values includes minValue and maxValue.
        /// </returns>
        /// <exception name="ArgumentOutOfRangeException">
        /// Condition: <c>minValue</c> is greater than <c>maxValue</c>. 
        /// </exception>
        internal static double NextDouble(double minValue, double maxValue)
        {
            #region Preconditions

            // The maximum value must be greater than or equal to the minimum value.
            if (maxValue < minValue)
            {
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", "maxValue");
            }

            #endregion // Preconditions

            return (minValue + (maxValue - minValue) * _random.NextDouble());
        }

        // http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        internal static void Shuffle<T>(T[] array)
        {
            for (int i = array.Length; i > 1; --i)
            {
                int j = Next(i);
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }
    
        #endregion Static methods

        #endregion // Internal members


        #region Private members

        #region Static fields

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        private static Random _random;

        #endregion // Static fields

        #endregion // Private members
    }
}
