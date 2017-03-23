using System;
using System.Threading;

namespace Mozog.Utils
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

        public static bool WithProbability(double probability) => Double() < probability;
    }
}
