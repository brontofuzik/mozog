namespace Mozog.Utils.Math
{
    public static class MathExtensions
    {
        public static bool IsWithin(this double value, double min, double max)
            => min <= value && value <= max;

        public static double Clamp(this double value, double min, double max)
            => value < min ? min : value > max ? max : value;
    }
}
