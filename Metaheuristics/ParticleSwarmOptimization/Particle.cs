using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;

namespace ParticleSwarmOptimization
{
    public class Particle
    {
        private const double W = 0.7;
        private const double C1 = 1.4;
        private const double C2 = 1.4;

        private readonly Swarm algo;

        // Optimum
        public double[] bestGlobalPosition;
        public double bestGlobalError = Double.MaxValue;

        public Particle(Swarm algo)
        {
            this.algo = algo;
        }

        public List<Particle> Neighbours { get; set; } = new List<Particle>();

        public double[] Position { get; private set; }

        private double[] Velocity { get; set; }

        public double Error { get; private set; }

        private IEnumerable<Particle> ParticleWithNeighbours
            => this.Yield().Concat(Neighbours);

        public void Initialize()
        {
            Position = algo.InitializePosition(algo);
            Velocity = algo.InitializeVelocity(algo);
            UpdateError();
        }

        public double Optimize()
        {
            double[] bestLocalPosition = ParticleWithNeighbours.MinBy(p => p.bestGlobalError).bestGlobalPosition;

            UpdateVelocity(bestLocalPosition);
            UpdatePosition();
            UpdateError();

            return bestGlobalError;
        }

        private void UpdateVelocity(double[] bestLocalPosition)
        {
            for (int i = 0; i < Velocity.Length; i++)
            {
                Velocity[i] = W * Velocity[i]
                    + C1 * StaticRandom.Double() * (bestGlobalPosition[i] - Position[i])
                    + C2 * StaticRandom.Double() * (bestLocalPosition[i] - Position[i]);
            }
        }

        private void UpdatePosition()
        {
            Position.AddM(Velocity).MapM(e => e.Clamp(algo.Min, algo.Max));
        }

        private void UpdateError()
        {
            Error = algo.ObjetiveFunction.Evaluate(Position);
            UpdateOptimum();
        }

        private void UpdateOptimum()
        {
            if (Error < bestGlobalError)
            {
                bestGlobalPosition = (double[])Position.Clone();
                bestGlobalError = Error;
            }
        }
    }
}