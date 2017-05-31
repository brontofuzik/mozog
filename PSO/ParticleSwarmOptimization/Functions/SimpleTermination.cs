using System;

namespace ParticleSwarmOptimization.Functions
{
    public class SimpleTermination : ITermination
    {
        private readonly int maxIterations;
        private readonly double targetError;

        public SimpleTermination(int maxIterations = Int32.MaxValue, double targetError = Double.MinValue)
        {
            this.maxIterations = maxIterations;
            this.targetError = targetError;
        }

        public bool Terminate(int iteration, double error)
            => iteration >= maxIterations || error <= targetError;
    }
}