using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Mozog.Utils.Math;

namespace ParticleSwarmOptimization
{
    public class Particle : IParticle
    {
        private readonly Parameters parameters;

        private readonly Func<double[], double> errorFunc;

        public Particle(int dimension, Func<double[], double> errorFunc, Parameters parameters)
        {
            this.parameters = parameters;
            this.errorFunc = errorFunc;

            Dimension = dimension;

            Position = GetRandomPosition();
            Velocity = GetRandomVelocity();
            Error = errorFunc(Position);

            BestPosition = (double[])Position.Clone();
            BestError = Error;
        }

        public int Dimension { get; }

        public double[] Position { get; }

        public double[] Velocity { get; }

        public double Error { get; private set; }

        public double[] BestPosition { get; private set; }

        public double BestError { get; private set; }

        public List<IParticle> Neighbours { get; set; } = new List<IParticle>();

        private IEnumerable<IParticle> ParticleWithNeighbours
            => Enumerable.Repeat(this, 1).Concat(Neighbours);

        public double Optimize()
        {
            double[] bestLocalPosition = GetBestLocalPosition();
            UpdateVelocity(bestLocalPosition);

            UpdatePosition();

            // Update errors
            if (IsInRange(Position))
            {
                Error = errorFunc(Position);
                if (Error < BestError)
                {
                    BestPosition = (double[])Position.Clone();
                    BestError = Error;
                }          
            }

            return BestError;
        }

        //
        // Manager
        //

        private double[] GetRandomPosition()
            => Enumerable.Range(0, Dimension)
                .Select(_ => (parameters.RangeMax - parameters.RangeMin) * StaticRandom.Double() + parameters.RangeMin)
                .ToArray();

        private double[] GetRandomVelocity()
            => Enumerable.Range(0, Dimension)
                .Select(_ => (parameters.RangeMax - parameters.RangeMin) * StaticRandom.Double() + parameters.RangeMin)
                .ToArray();

        private double[] GetBestLocalPosition()
            => ParticleWithNeighbours.MinBy(p => p.BestError).BestPosition;

        private void UpdateVelocity(double[] bestLocalPosition)
        {
            for (int i = 0; i < Velocity.Length; i++)
            {
                Velocity[i] = parameters.W * Velocity[i]
                    + parameters.C1 * StaticRandom.Double() * (BestPosition[i] - Position[i])
                    + parameters.C2 * StaticRandom.Double() * (bestLocalPosition[i] - Position[i]);
            }
        }

        private void UpdatePosition()
        {
            for (int i = 0; i < Position.Length; i++)
            {
                Position[i] += Velocity[i];
            }
        }

        private bool IsInRange(double[] position)
            => position.All(pd => parameters.RangeMin <= pd && pd <= parameters.RangeMax);
    }

    public interface IParticle
    {
        double BestError { get; }

        double[] BestPosition { get; }

        int Dimension { get; }

        double Error { get; }

        List<IParticle> Neighbours { get; set; }

        double[] Position { get; }

        double[] Velocity { get; }

        double Optimize();
    }
}