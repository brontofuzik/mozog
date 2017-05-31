using System;

namespace ParticleSwarmOptimization.Functions
{
    public class StagnatingTermination : ITermination
    {
        private readonly int maxIterations;
        private readonly int maxStagnatingIterations;
        private readonly double targetError;

        private double lowestError = Double.MaxValue;
        private int stagnatingIterations = 0;

        public StagnatingTermination(int maxIterations = Int32.MaxValue, int maxStagnatingIterations = Int32.MaxValue, double targetError = Double.MinValue)
        {
            this.maxIterations = maxIterations;
            this.maxStagnatingIterations = maxStagnatingIterations;
            this.targetError = targetError;
        }

        public bool Terminate(int iteration, double error)
        {
            if (error < lowestError)
            {
                lowestError = error;
                stagnatingIterations = 0;
            }
            else
            {
                stagnatingIterations++;
            }

            return iteration >= maxIterations || stagnatingIterations >= maxStagnatingIterations || error <= targetError;
        }
    }
}