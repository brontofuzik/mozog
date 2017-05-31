using System.Collections.Generic;
using System.Linq;
using MoreLinq;
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

        public double[] bestPosition;
        public double bestError;

        public Particle(Swarm algo)
        {
            this.algo = algo;

            Position = GetRandomPosition();
            Velocity = GetRandomVelocity();
            Error = algo.ErrorFunc(Position);

            bestPosition = (double[])Position.Clone();
            bestError = Error;
        }

        public List<Particle> Neighbours { get; set; } = new List<Particle>();

        public double[] Position { get; }

        public double[] Velocity { get; }

        public double Error { get; private set; }

        private IEnumerable<Particle> ParticleWithNeighbours
            => Enumerable.Repeat(this, 1).Concat(Neighbours);

        public double Optimize()
        {
            double[] bestLocalPosition = GetBestLocalPosition();
            UpdateVelocity(bestLocalPosition);

            UpdatePosition();

            // Update errors
            if (IsInRange(Position))
            {
                Error = algo.ErrorFunc(Position);
                if (Error < bestError)
                {
                    bestPosition = (double[])Position.Clone();
                    bestError = Error;
                }          
            }

            return bestError;
        }

        //
        // Manager
        //

        private double[] GetRandomPosition()
            => algo.Dimension.Times(() => StaticRandom.Double(algo.Min, algo.Max)).ToArray();

        private double[] GetRandomVelocity()
            => algo.Dimension.Times(() => StaticRandom.Double(algo.Min, algo.Max)).ToArray();

        private double[] GetBestLocalPosition() => ParticleWithNeighbours.MinBy(p => p.bestError).bestPosition;

        private void UpdateVelocity(double[] bestLocalPosition)
        {
            for (int i = 0; i < Velocity.Length; i++)
            {
                Velocity[i] = W * Velocity[i]
                    + C1 * StaticRandom.Double() * (bestPosition[i] - Position[i])
                    + C2 * StaticRandom.Double() * (bestLocalPosition[i] - Position[i]);
            }
        }

        private void UpdatePosition()
        {
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] += Velocity[i];
            }
        }

        private bool IsInRange(double[] position) => position.All(pd => algo.Min <= pd && pd <= algo.Max);
    }
}