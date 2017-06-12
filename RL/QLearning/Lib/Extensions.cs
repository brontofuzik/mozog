using System;
using System.Globalization;

namespace QLearning.Lib
{
    static class Extensions
    {
        public static readonly CultureInfo CultureEnUs = new CultureInfo("en-US");

        public static string Pretty(this double d)
            => Math.Round(d, 2).ToString("G", CultureEnUs);

        public static string EnumToString<T>(this T type)
            => Enum.GetName(typeof(T), type);
    }
}