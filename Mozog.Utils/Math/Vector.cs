using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using Mozog.Utils.Math;

namespace Mozog.Utils.Math
{
    public static class Vector
    {
        // Immutable
        public static double[] Add(this double[] vector1, double[] vector2)
        {
            double[] result = new double[vector1.Length];
            for (int i = 0; i < vector1.Length; i++)
            {
                result[i] = vector1[i] + vector2[i];
            }
            return result;
        }

        // Mutable
        public static double[] AddM(this double[] vector1, double[] vector2)
        {
            for (int i = 0; i < vector1.Length; i++)
            {
                vector1[i] += vector2[i];
            }
            return vector1;
        }

        // Immutable
        public static double[] Subtract(double[] vector1, double[] vector2)
        {
            Vector<double> v1 = Vector<double>.Build.DenseOfArray(vector1);
            Vector<double> v2 = Vector<double>.Build.DenseOfArray(vector2);
            return (v1 - v2).ToArray();
        }

        // Immutable
        public static int[] Subtract(int[] vector1, int[] vector2)
        {
            int[] result = new int[vector1.Length];
            for (int i = 0; i < vector1.Length; ++i)
            {
                result[i] = vector1[i] - vector2[i];
            }
            return result;
        }

        // Using Math.NET
        public static double Multiply(double[] vector1, double[] vector2)
            => Vector<double>.Build.DenseOfArray(vector1) * Vector<double>.Build.DenseOfArray(vector2);

        public static int Multiply(int[] vector1, int[] vector2)
            => vector1.Select((t, i) => t * vector2[i]).Sum();

        public static double Magnitude(double[] vector)
            => Vector<double>.Build.DenseOfArray(vector).L2Norm();

        public static double Magnitude(int[] vector)
            => System.Math.Sqrt(Multiply(vector, vector));

        public static double Distance(double[] vector1, double[] vector2)
            => Magnitude(Subtract(vector1, vector2));

        public static double Distance(int[] vector1, int[] vector2)
            => Magnitude(Subtract(vector1, vector2));

        public static double[] Normalize(double[] vector, double magnitude = 1.0)
        {
            Require.IsNotNull(vector, nameof(vector));
            Require.IsNonNegative(magnitude, nameof(magnitude));

            double sumOfSquares = vector.Sum(e => e * e);
            double factor = sumOfSquares != 0 ? System.Math.Sqrt(magnitude / sumOfSquares) : 0;

            return vector.Select(e => e * factor).ToArray();
        }

        // No active element if index -1.
        public static double[] IndexToVector(int index, int length)
        {
            var vector = new double[length];
            if (index >= 0)
            {
                vector[index] = 1.0;
            }
            return vector;
        }

        // Index is -1 if none/more than 1 element is active. 
        public static int VectorToIndex(double[] vector, double threshold = 0.0)
        {
            if (threshold == 0.0)
            {
                return vector.Select((e, i) => (element: e, index: i))
                    .Aggregate((t1, t2) => t2.element > t1.element ? t2 : t1).index;
            }
            else
            {
                int index = -1;
                int activeCount = 0;
                for (int i = 0; i < vector.Length; i++)
                {
                    if (vector[i] >= threshold)
                    {
                        index = i;
                        activeCount++;
                    }
                }
                return activeCount == 1 ? index : -1;
            }
        }

        public static int HammingDistance(double[] vector1, double[] vector2)
            => vector1.Zip(vector2, (d1, d2) => d1 != d2 ? 1 : 0).Sum();

        // Immutable
        public static double[] Map(this double[] vector, Func<double, double> func)
            => vector.Select(func).ToArray();

        // Mutable
        public static double[] MapM(this double[] vector, Func<double, double> func)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = func(vector[i]);
            }
            return vector;
        }

        public static double[] MapM(this double[] vector, Func<double, int, double> func)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                vector[i] = func(vector[i], i);
            }
            return vector;
        }

        public static string ToString(int[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));
            return $"[{String.Join(", ", vector.Select(e => e.ToString()))}]";
        }

        public static string ToString(double[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));
            return $"[{String.Join(", ", vector.Select(e => e.ToString("F2")))}]";
        }
    }
}
