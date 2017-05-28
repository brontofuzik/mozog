using System;
using System.Linq;

namespace ParticleSwarmOptimization.Examples
{
    [Serializable]
    public static class Functions
    {
        // Beale function
        // Minimum: f(3, 0.5)
        // http://www.sfu.ca/~ssurjano/beale.html
        public static double Beale(double[] xs)
        {
            double x = xs[0];
            double y = xs[1];

            return (1.5 - x + x*y) * (1.5 - x + x*y)
                + (2.25 - x + x*y*y) * (2.25 - x + x*y*y)
                + (2.625 - x + x*y*y*y) * (2.625 - x + x*y*y*y);
        }

        public static int BealeDimension => 2;

        // Griewank function
        // Minimum: f(0, 0, ..., 0)
        // http://mathworld.wolfram.com/GriewankFunction.html
        public static double Griewank(double[] xs)
        {
            double f = 0;
            double p = 1;

            for (int d = 0; d < xs.Length; d++)
            {
                double xd = xs[d];
                f = f + xd * xd;
                p = p * Math.Cos(xd / Math.Sqrt((d + 1)));
            }

            f = 1 + f / 4000 - p;

            return f;
        }

        public static int GriewankDimension => 4;

        // Rosenbrock function
        // Minimum: f(1, 1, ..., 1)
        // https://en.wikipedia.org/wiki/Rosenbrock_function
        public static double Rosenbrock(double[] xs)
        {
            double x = xs[0];
            double y = xs[1];

            return (1 - x) * (1 - x) + 100 * ((y - x*x) * (y - x*x));
        }

        public static int RosenbrockDimension => 2;

        // Sphere function
        // Minimum: f(0, 0, ..., 0)
        // http://www.sfu.ca/~ssurjano/spheref.html
        public static double Sphere(double[] xs)
            => xs.Sum(t => t * t);

        public static int SphereDimension => 4;
    }
}