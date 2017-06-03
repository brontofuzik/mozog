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

            Position = algo.InitializePosition(algo);
            Velocity = algo.InitializeVelocity(algo);
            Error = algo.ObjetiveFunction.Evaluate(Position);

            bestPosition = (double[])Position.Clone();
            bestError = Error;
        }

        public List<Particle> Neighbours { get; set; } = new List<Particle>();

        public double[] Position { get; }

        public double[] Velocity { get; }

        public double Error { get; private set; }

        private IEnumerable<Particle> ParticleWithNeighbours
            => this.AsEnumerable().Concat(Neighbours);

        public double Optimize()
        {
            double[] bestLocalPosition = ParticleWithNeighbours.MinBy(p => p.bestError).bestPosition;

            UpdateVelocity(bestLocalPosition);
            UpdatePosition();
            UpdateError();

            return bestError;
        }

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

        private void UpdateError()
        {
            if (IsInRange)
            {
                Error = algo.ObjetiveFunction.Evaluate(Position);
            }

            if (Error < bestError)
            {
                bestPosition = (double[])Position.Clone();
                bestError = Error;
            }
        }

        private bool IsInRange => Position.All(x => x.IsWithin(algo.Min, algo.Max));
    }
}