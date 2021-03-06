﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MathNet.Numerics.Random;

namespace Mozog.Utils.Math
{
    public class StaticRandom
    {
        static int trueSeed = Environment.TickCount;
        private static int? testSeed = null;

        private static readonly ThreadLocal<Random> random = new ThreadLocal<Random>(() => new Random(testSeed ?? Interlocked.Increment(ref trueSeed)));

        public static int Seed { set => testSeed = value; }

        public static int Int() => random.Value.Next();

        public static long Int64() => random.Value.NextInt64();

        public static int Int(int max) => random.Value.Next(max);

        public static int Int(int min, int max)
        {
            if (max < min)
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", nameof(max));

            return random.Value.Next(min, max);
        }

        public static double Double() => random.Value.NextDouble();

        public static double Double(double min, double max)
        {
            if (max < min)
                throw new ArgumentException("The maximum value must be greater than or equal to the minimum value.", nameof(max));

            return min + (max - min) * random.Value.NextDouble();
        }

        public static double[] DoubleArray(int dimension, double min = 0.0, double max = 1.0)
            => dimension.Times(() => Double(min, max)).ToArray();

        public static double Normal(double mean, double stdDev)
        {
            // https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform
            double u1 = 1.0 - Double();
            double u2 = 1.0 - Double();
            double stdNormal = System.Math.Sqrt(-2 * System.Math.Log(u1)) * System.Math.Sin(2 * System.Math.PI * u2); // N(0, 1)
            return mean + stdDev * stdNormal; // N(mean, stdDev^2)
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

        public static T Pick<T>(IList<T> items) => items[Int(items.Count)];
    }
}
