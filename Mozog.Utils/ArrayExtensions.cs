using System;
using System.Collections.Generic;
using System.Linq;

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

        public static T[] GetSubarray<T>(this T[] self, int index, int length)
        {
            var subarray = new T[length];
            Array.Copy(sourceArray: self, sourceIndex: index, destinationArray: subarray, destinationIndex: 0, length: length);
            return subarray;
        }

        public static void ReplaceSubarray<T>(this T[] self, int index, T[] subarray)
        {
            Array.Copy(sourceArray: subarray, sourceIndex: 0, destinationArray: self, destinationIndex: index, length: subarray.Length);
        }

        public static string ToString<T>(T[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));
            return $"[{String.Join(",", vector.Select(e => e.ToString()))}]";
        }
    }
}
