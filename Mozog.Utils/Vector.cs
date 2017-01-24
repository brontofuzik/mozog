using System;
using MathNet.Numerics.LinearAlgebra;

namespace Mozog.Utils
{
    public class Vector
    {
        /// <summary>
        /// Calculates the difference between two vectors of reals.
        /// </summary>
        /// <param name="vector1">The minuend vector.</param>
        /// <param name="vector2">The subtrahend vector.</param>
        /// <returns>The difference between the two vectors.</returns>
        public static double[] Subtract(double[] vector1, double[] vector2)
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(vector1);
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(vector2);
            return (v1 - v2).ToArray();
        }

        /// <summary>
        /// Calculates the difference between two vectors of integers.
        /// </summary>
        /// <param name="vector1">The minuend vector.</param>
        /// <param name="vector2">The subtrahend vector.</param>
        /// <returns>The difference between the two vectors.</returns>
        public static int[] Subtract(int[] vector1, int[] vector2)
        {
            int[] result = new int[vector1.Length];
            for (int i = 0; i < vector1.Length; ++i)
            {
                result[i] = vector1[i] - vector2[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the dot product of two vectors of reals.
        /// </summary>
        /// <param name="vector1">The first multiplicand vector.</param>
        /// <param name="vector2">The second multiplicand vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static double Multiply(double[] vector1, double[] vector2)
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(vector1);
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(vector2);
            return v1 * v2;
        }

        /// <summary>
        /// Calculates the dot product of two vectors of integers.
        /// </summary>
        /// <param name="vector1">The first multiplicand vector.</param>
        /// <param name="vector2">The second multiplicand vector.</param>
        /// <returns>The dot product of the two vectors.</returns>
        public static int Multiply(int[] vector1, int[] vector2)
        {
            int result = 0;
            for (int i = 0; i < vector1.Length; ++i)
            {
                result += vector1[i] * vector2[i];
            }
            return result;
        }

        /// <summary>
        /// Calculates the magnitude of a real vector of reals.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The magnitude of the vector.</returns>
        public static double Magnitude(double[] vector)
        {
            Vector<double> v = Vector<double>.Build.DenseOfArray(vector);
            return v.L2Norm();
        }

        /// <summary>
        /// Calculates the magnitude of a vector of integers.
        /// </summary>
        /// <param name="vector">The vector.</param>
        /// <returns>The magnitude of the vector.</returns>
        public static double Magnitude(int[] vector) => Math.Sqrt(Multiply(vector, vector));

        public static double Distance(double[] vector1, double[] vector2) => Magnitude(Subtract(vector1, vector2));

        public static double Distance(int[] vector1, int[] vector2) => Magnitude(Subtract(vector1, vector2));
    }
}
