using System;
using System.Collections.Generic;
using System.Linq;

namespace Mozog.Utils
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<int> Range(int from, int to)
            => Enumerable.Range(from, to - from);

        public static void Times(this int count, Action action)
        {
            for (int i = 0; i < count; i++)
                action();
        }

        public static IEnumerable<T> Times<T>(this int count, Func<T> func)
        {
            for (int i = 0; i < count; i++)
                yield return func();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in source)
                action(item, i++);
        }

        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            list.AsEnumerable().ForEach(action);
        }

        public static void ForEach<T>(this IList<T> list, Action<T, int> action)
        {
            list.AsEnumerable().ForEach(action);
        }

        public static void ForEachWithinRange<T>(this T[] array, int from, int to, Action<int> action)
        {
            for (int i = from; i < to; i++)
                action(i);
        }

        public static void ForEachOutsideRange<T>(this T[] array, int from, int to, Action<int> action)
        {
            for (int i = 0; i < array.Length; i++)
                if (i < from || to <= i)
                    action(i);
        }

        public static bool None<T>(this IEnumerable<T> source) => !source.Any();

        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static T MaxBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> keySelector)
            where TProp : IComparable
        {
            return source.Aggregate((best, item) => keySelector(item).CompareTo(keySelector(best)) > 0 ? item : best);
        }

        public static T MinBy<T, TProp>(this IEnumerable<T> source, Func<T, TProp> keySelector)
            where TProp : IComparable
        {
            return source.Aggregate((best, item) => keySelector(item).CompareTo(keySelector(best)) < 0 ? item : best);
        }
    }
}
