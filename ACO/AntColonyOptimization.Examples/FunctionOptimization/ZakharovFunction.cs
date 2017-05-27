using System;

namespace AntColonyOptimization.Examples.FunctionOptimization
{
    internal class ZakharovFunction : ObjectiveFunction
    {
        public ZakharovFunction(int dimension)
            : base(dimension, Objective.Minimize)
        {
        }

        protected override double EvaluateInternal(double[] steps)
            => Mozog.Examples.Functions.Zakharov(steps);
    }
}
