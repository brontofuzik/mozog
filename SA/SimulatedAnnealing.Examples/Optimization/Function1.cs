using System;

namespace SimulatedAnnealing.Examples.Optimization
{
    internal class Function1 : ObjectiveFunction<double>
    {
        public Function1()
            : base(Objective.Minimize)
        {
        }

        public override double EvaluateInternal(double[] state)
            => 1 - Math.Sin(state[0]) / state[0];
    }
}
