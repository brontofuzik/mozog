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

        public static string ToString<T>(T[] vector)
        {
            Require.IsNotNull(vector, nameof(vector));
            return $"[{String.Join(", ", vector.Select(e => e.ToString()))}]";
        }
    }
}
