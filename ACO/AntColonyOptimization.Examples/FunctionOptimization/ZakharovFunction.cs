using System;

namespace AntColonyOptimization.Examples.FunctionOptimization
{
    /// <summary>
    /// A Zakharov (Z) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page3088.htm
    /// </summary>
    internal class ZakharovFunction : ObjectiveFunction
    {
        public ZakharovFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps)
        {
            int n = 2;
            double sum1 = 0.0;
            double sum2 = 0.0;
            for (int i = 0; i < n; i++)
            {
                sum1 += Math.Pow(steps[i], 2);
                sum2 += 0.5 * i * steps[i];
            }
            return sum1 + Math.Pow(sum2, 2) + Math.Pow(sum2, 4);
        }
    }
}
