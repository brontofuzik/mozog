using System;

namespace SimulatedAnnealing.Examples.Optimization
{
    internal class Function2 : ObjectiveFunction<double>
    {
        public Function2()
            : base(Objective.Minimize)
        {
        }

        public override double EvaluateInternal(double[] state)
        {
            double x = state[0];
            double y = state[1];
            double exponent1 = -Math.Pow(x, 2) - Math.Pow(y + 1, 2);
            double exponent2 = -Math.Pow(x, 2) - Math.Pow(y, 2);
            double exponent3 = -Math.Pow(x + 1, 2) - Math.Pow(y, 2);
            return 8 - 3 * Math.Pow(1 - x, 2) * Math.Exp(exponent1) - 10 * (1 / 5.0 * x - Math.Pow(x, 3) - Math.Pow(y, 5)) * Math.Exp(exponent2) - 1 / 3.0 * Math.Exp(exponent3);
        }
    }
}
