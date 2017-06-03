namespace Mozog.Utils.Math
{
    public static class MathExtensions
    {
        public static bool IsWithin(this double value, double min, double max)
            => min <= value && value <= max;

        public static double Clamp(this double value, double min, double max)
            => value < min ? min : value > max ? max : value;
    }

    public static class Math
    {
        public static int Mod(int a, int n) => (a % n + n) % n;
    }
}
