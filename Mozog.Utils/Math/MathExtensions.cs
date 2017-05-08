namespace Mozog.Utils.Math
{
    public static class MathExtensions
    {
        public static double Clamp(this double value, double min, double max)
            => value < min ? min : value > max ? max : value;
    }
}
