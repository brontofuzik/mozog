using System;

namespace NeuralNetwork.Utils
{
    internal class Random
    {
        /// <summary>
        /// 
        /// </summary>
        static Random()
        {
            _random = new System.Random();
        }

        internal static int Next() => _random.Next();

        internal static int Next(int maxValue) => _random.Next(maxValue);

        internal static int Next(int minValue, int maxValue) => _random.Next(minValue, maxValue);

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
        internal static double NextDouble(double minValue, double maxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", nameof(maxValue));
            }

            return minValue + (maxValue - minValue) * _random.NextDouble();
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        internal static void Shuffle<T>(T[] array)
        {
            for (int i = array.Length; i > 1; --i)
            {
                int j = Next(i);
                T tmp = array[j];
                array[j] = array[i - 1];
                array[i - 1] = tmp;
            }
        }

        /// <summary>
        /// The pseudo-random number generator.
        /// </summary>
        private static readonly System.Random _random;
    }
}
