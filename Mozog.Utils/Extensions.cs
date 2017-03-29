using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozog.Utils
{
    public static class Extensions
    {
        public static void Times(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
            {
                action();
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
            {
                action(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in source)
            {
                action(item, i++);
            }
        }

        public static void ForEachWithinRange<T>(this T[] array, int from, int to, Action<int> action)
        {
            for (int i = from; i < to; i++)
            {
                action(i);
            }
        }

        public static void ForEachOutsideRange<T>(this T[] array, int from, int to, Action<int> action)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (i < from || to <= i)
                {
                    action(i);
                }
            }
        }

        public static bool None<T>(this IEnumerable<T> source) => !source.Any();
    }
}
