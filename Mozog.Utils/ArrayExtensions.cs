using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Mozog.Utils
{
    public static class ArrayExtensions
    {
        public static IEnumerable<T[]> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size).ToArray();
            }
        }

        public static T[] GetSubarray<T>(this T[] array, int index, int length)
        {
            var subarray = new T[length];
            Array.Copy(sourceArray: array, sourceIndex: index, destinationArray: subarray, destinationIndex: 0, length: length);
            return subarray;
        }

        public static void ReplaceSubarray<T>(this T[] array, int index, T[] subarray)
        {
            Array.Copy(sourceArray: subarray, sourceIndex: 0, destinationArray: array, destinationIndex: index, length: subarray.Length);
        }

        // 2D
        public static IEnumerable<(int i1, int i2, T value)> Select2D<T>(this T[,] array)
        {
            for (int i1 = 0; i1 < array.GetLength(0); i1++)
            for (int i2 = 0; i2 < array.GetLength(1); i2++)
                yield return (i1, i2, array[i1, i2]);
        }

        // 2D
        public static void ForEach2D<T>(this T[,] array, Action<int, int, T> action)
        {
            for (int i1 = 0; i1 < array.GetLength(0); i1++)
            for (int i2 = 0; i2 < array.GetLength(1); i2++)
                action(i1, i2, array[i1, i2]);
        }

        // 2D
        public static T[,] Initialize2D<T>(this T[,] array, Func<int, int, T> initializer)
        {
            array.ForEach2D((i1, i2, _) => array[i1, i2] = initializer(i1, i2));
            return array;
        }

        // 2D
        public static T[] Flatten2D<T>(this T[,] array)
        {
            var flattened = new T[array.GetLength(0) * array.GetLength(1)];
            int i = 0;
            for (int row = 0; row < array.GetLength(0); row++)
            {
                for (int column = 0; column < array.GetLength(1); column++)
                {
                    flattened[i++] = array[row, column];
                }
            }
            return flattened;
        }

        public static string ToString<T>(T[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));
            return $"[{String.Join(",", vector.Select(e => e.ToString()))}]";
        }
    }
}
