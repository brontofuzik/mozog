using System;
using static System.Math;

namespace SimulatedAnnealing.Functions.Cooling
{
    public class LambdaCooling : CoolingFunction
    {
        public static CoolingFunction Linear => new LambdaCooling((t0, tn, k, n) => t0 + (tn - t0) * (k / (double)n));

        public static CoolingFunction Exponential => new LambdaCooling((t0, tn, k, n) => t0 * Pow(tn / t0, k / (double)n));

        private readonly Func<double, double, int, int, double> cool;

        public LambdaCooling(Func<double, double, int, int, double> cool)
        {
            this.cool = cool;
        }

        public override double CoolTemperature(int currentIteration)
            => cool(initialTemperature, finalTemperature, currentIteration, totalIterations);
    }
}
