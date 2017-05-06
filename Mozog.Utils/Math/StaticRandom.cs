using System;
using System.Threading;

namespace Mozog.Utils.Math
{
    public class StaticRandom
    {
        static int seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        public static int Int() => random.Value.Next();

        public static int Int(int maxValue) => random.Value.Next(maxValue);

        public static int Int(int minValue, int maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", nameof(maxValue));

            return random.Value.Next(minValue, maxValue);
        }

        public static double Double() => random.Value.NextDouble();

        public static double Double(double minValue, double maxValue)
        {
            if (maxValue < minValue)
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", nameof(maxValue));

            return minValue + (maxValue - minValue) * random.Value.NextDouble();
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static T[] Shuffle<T>(T[] array)
        {
            for (int i = array.Length; i > 1; --i)
            {
                int j = Int(i);
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
            return array;
        }
    }
}
