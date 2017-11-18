using System.Collections.Generic;
using System.Linq;

namespace Mozog.Utils.Math
{
    public static class MathExtensions
    {
        public static bool IsWithin(this double value, double min, double max)
            => min <= value && value <= max;

        public static double Clamp(this double value, double min, double max)
            => value < min ? min : value > max ? max : value;

        public static int Product(this IEnumerable<int> values)
            => values.Aggregate(1, (acc, val) => acc * val);

        public static double Product(this IEnumerable<double> values)
            => values.Aggregate(1.0, (acc, val) => acc * val);
    }

    public static class Math
    {
        // Modulo
        public static int Mod(int a, int n) => (a % n + n) % n;
    }
}
