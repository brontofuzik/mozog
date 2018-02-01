using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Mozog.Utils
{
    public static class Misc
    {
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            => dict.ContainsKey(key) ? dict[key] : default(TValue);

        public static TValue? GetOrDefaultNullable<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key) where TValue : struct
            => dict.ContainsKey(key) ? dict[key] : (TValue?)null;

        public static void Swap<T>(ref T var1, ref T var2)
        {
            T temp = var1;
            var1 = var2;
            var2 = temp;
        }

        public static void SwapArrays<T>(T[] array1, int origin1, T[] array2, int origin2, int length)
        {
            T[] temp = new T[length];

            // array1 -> temp
            Array.Copy(array1, origin1, temp, 0, length);

            // array2 -> array1
            Array.Copy(array2, origin2, array1, origin1, length);

            // temp -> array2
            Array.Copy(temp, 0, array2, origin2, length);
        }

        public static TimeSpan MeasureTime(Action action)
        {
            var sw = Stopwatch.StartNew();
            action();
            return sw.Elapsed;
        }

        public static (TResult result, TimeSpan elapsedTime) MeasureTime<TResult>(Func<TResult> func)
        {
            var sw = Stopwatch.StartNew();
            var result = func();
            return (result, sw.Elapsed);
        }
    }
}
