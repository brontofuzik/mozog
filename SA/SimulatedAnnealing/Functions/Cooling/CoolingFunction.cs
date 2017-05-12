using System;

namespace SimulatedAnnealing.Functions.Cooling
{
    public abstract class CoolingFunction : FunctionBase<object>, ICoolingFunction
    {
        protected double initialTemperature;
        protected double finalTemperature;
        protected int totalIterations;

        public abstract double CoolTemperature(int currentIteration);

        public void SetParams(double initialTemperature, double finalTemperature = 0, int totalIterations = Int32.MaxValue)
        {
            this.initialTemperature = initialTemperature;
            this.finalTemperature = finalTemperature;
            this.totalIterations = totalIterations;
        }
    }
}
