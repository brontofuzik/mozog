using System;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace SimulatedAnnealing.Examples.Optimization
{
    internal class OptimizationSimulatedAnnealing : SimulatedAnnealing<double>
    {
        protected override double[] InitializeState()
            => Dimension.Times(() => StaticRandom.Double(-20, +20)).ToArray();

        protected override double[] PerturbState(double[] currentState)
        {
            double[] newState = new double[Dimension];
            Array.Copy(currentState, newState, Dimension);

            newState[StaticRandom.Int(0, Dimension)] = StaticRandom.Double(-20, +20);

            return newState;
        }
    }
}
