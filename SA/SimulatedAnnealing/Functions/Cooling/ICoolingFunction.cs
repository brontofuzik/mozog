using System;

namespace SimulatedAnnealing.Functions.Cooling
{
    public interface ICoolingFunction : IFunction<object>
    {
        double CoolTemperature(int iteration);

        void SetParams(double initialTemperature, double finalTemperature = 0.0, int totalIteratios = Int32.MaxValue);
    }
}
