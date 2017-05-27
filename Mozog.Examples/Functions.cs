using System;
using System.Linq;
using static System.Math;

namespace Mozog.Examples
{
    public static class Functions
    {
        // Rastrigin function (2D)
        // Minimum: f(0, 0) = 0
        public static Func<double[], double> Rastrigin => s =>
        {
            double x = s[0];
            double y = s[1];

            return 20 + (Pow(x, 2) - 10 * Cos(2 * PI * x)) + (Pow(y, 2) - 10 * Cos(2 * PI * y));
        };

        // Ackley function (2D)
        // Minimum: f(0, 0) = 0
        public static Func<double[], double> Ackley => s =>
        {
            double x = s[0];
            double y = s[1];

            return -20 * Exp(-0.2 * Sqrt(0.5 * (Pow(x, 2) + Pow(y, 2)))) - Exp(0.5 * (Cos(2 * PI * x) + Cos(2 * PI * y))) + E + 20;
        };

        // Sphere function (2D)
        // Minimum: f(0, 0) = 0
        public static Func<double[], double> Sphere => s =>
        {
            double x = s[0];
            double y = s[1];

            return Pow(x, 2) + Pow(y, 2);
        };

        // Rosenbrock function, a.k.a. Rosenbrock's valley or Rosenbrock's banana (XD)
        // Minimum: f(1, 1, ...) = 0
        // See: http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page2537.htm
        public static Func<double[], double> Rosenbrock => s =>
        {
            //double x = s[0];
            //double y = s[1];
            //return 100 * Pow(y - Pow(x, 2), 2) + Pow(x - 1, 2);

            double result = 0.0;

            // f(x) = SUM[i = 0 .. n-2]f(x)_i
            for (int i = 0; i < s.Length - 1; i++)
            {
                // f(x)_i = (1 - x_i)^2 + 100 * (x_i+1 - (x_i)^2)^2 
                result += Pow(1 - s[i], 2) + 100 * Pow(s[i + 1] - Pow(s[i], 2), 2);
            }

            return result;
        };

        // Sphere model function (XD)
        // Minimum: f(0, 0, ...) = 0.0
        // See http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page1113.htm
        public static Func<double[], double> SphereModel => s
            => s.Sum(t => Pow(t, 2));

        // Goldestein & Price function (2D)
        // Minimum: f(0, -1) = 3.0
        // See: http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page1760.htm
        public static Func<double[], double> GoldsteinPrice => s =>
        {
            double x = s[0];
            double y = s[1];

            return (1 + Pow(x + y + 1, 2) * (19 - 14 * x + 3 * Pow(x, 2) - 14 * y + 6 * x* y + 3 * Pow(y, 2)))
                   * (30 + Pow(2 * x - 3 * y, 2) * (18 - 32 * x + 12 * Pow(x, 2) + 48 * y - 36 * x* y + 27 * Pow(y, 2)));
        };

        // Zakharov function (2D)
        // Minimum: f(0, 0) = 0.0
        // See: http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page3088.htm
        public static Func<double[], double> Zakharov => s =>
        {
            const int dim = 2;

            double sum1 = Enumerable.Range(0, dim).Select(i => Pow(s[i], 2)).Sum();
            double sum2 = Enumerable.Range(0, dim).Select(i => 0.5 * i * s[i]).Sum();

            return sum1 + Pow(sum2, 2) + Pow(sum2, 4);
        };
    }
}
