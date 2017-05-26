using System;

namespace AntColonyOptimization.Examples.FunctionOptimization
{
    /// <summary>
    /// A sphere model (SM) function.
    /// http://www-optima.amp.i.kyoto-u.ac.jp/member/student/hedar/Hedar_files/TestGO_files/Page1113.htm
    /// </summary>
    internal class SphereModelFunction : ObjectiveFunction
    {
        public SphereModelFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps)
        {
            double output = 0.0;
            for (int i = 0; i < steps.Length; i++)
            {
                output += Math.Pow(steps[i], 2);
            }
            return output;
        }
    }
}
