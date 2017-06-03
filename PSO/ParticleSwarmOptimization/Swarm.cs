using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;
using Mozog.Utils;
using Mozog.Utils.Math;
using ParticleSwarmOptimization.Functions.Objective;
using ParticleSwarmOptimization.Functions.Termination;
using static Mozog.Utils.Math.Math;

namespace ParticleSwarmOptimization
{
    public class Swarm
    {
        private readonly Particle[] particles;

        // Optimum
        private double[] bestPosition;
        private double bestError = Double.MaxValue;

        public Swarm(int dimension, int swarmSize, int neighbours)
        {
            Dimension = dimension;

            particles = swarmSize.Times(() => new Particle(this)).ToArray();
            CreateTopology(neighbours);
        }

        public int Dimension { get; }

        public double Min => ObjetiveFunction.Min;

        public double Max => ObjetiveFunction.Max;

        #region Functions

        public IObjectiveFunction ObjetiveFunction { get; set; }

        public Func<Swarm, double[]> InitializePosition { get; set; }

        public Func<Swarm, double[]> InitializeVelocity { get; set; }

        #endregion // Functions

        public Result Optimize(ITermination termination)
        {
            Initialize();

            int iteration = 0;
            while (!termination.Terminate(iteration, bestError))
            {
                foreach (var particle in particles)
                {
                    particle.Optimize();
                    UpdateOptimum(particle);
                }
                iteration++;
            }

            return new Result(bestPosition, bestError, iteration);
        }

        // Optimize using simple termination
        public Result Optimize(int maxIteration = Int32.MaxValue, double targetError = Double.MinValue)
            => Optimize(new SimpleTermination(maxIteration, targetError));

        // Optimize using stagnating termination
        public Result Optimize(int maxIterations, int maxStagnatingIterations)
            => Optimize(new StagnatingTermination(maxIterations, maxStagnatingIterations));

        public Result Optimize_OLD(int maxIterations, int maxStagnatingIterations)
        {
            int iteration = 0;
            int stagnatingIterations = 0;

            while (iteration < maxIterations && stagnatingIterations < maxStagnatingIterations)
            {
                bool isErrorImproved = false;
                foreach (var particle in particles)
                {
                    double error = particle.Optimize();

                    if (error < bestError)
                    {
                        bestPosition = (double[])particle.Position.Clone();
                        bestError = error;
                        isErrorImproved = true;
                        stagnatingIterations = 0;
                    }
                }

                if (!isErrorImproved)
                    stagnatingIterations++;

                iteration++;
            }

            return new Result(bestPosition, bestError, iteration);
        }

        private void Initialize()
        {
            particles.ForEach(p => p.Initialize());
            UpdateOptimum(particles.MinBy(p => p.Error));
        }

        private void UpdateOptimum(Particle particle)
        {
            if (particle.Error < bestError)
            {
                bestPosition = (double[])particle.bestGlobalPosition.Clone();
                bestError = particle.Error;
            }
        }

        private void CreateTopology(int neighbours)
        {
            IEnumerable<Particle> LeftNeighbours(int index, int count)
            {
                for (int l = 1; l <= count; l++)
                {
                    yield return particles[Mod(index - l, particles.Length)];
                }
            }

            IEnumerable<Particle> RightNeighbours(int index, int count)
            {
                for (int r = 1; r <= count; r++)
                {
                    yield return particles[Mod(index + r, particles.Length)];
                }
            }

            int leftNeighbourCount = neighbours / 2;
            int rightNeighbourCount = neighbours / 2 + neighbours % 2;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Neighbours.AddRange(LeftNeighbours(i, leftNeighbourCount));
                particles[i].Neighbours.AddRange(RightNeighbours(i, rightNeighbourCount));
            }
        }

        private void CreateTopology_Randomized(int neighbours)
        {
            var indices = Enumerable.Range(0, particles.Length).ToArray();
            indices = StaticRandom.Shuffle(indices);

            IEnumerable<Particle> LeftNeighbours(int index, int count)
            {
                for (int l = 1; l <= count; l++)
                {
                    yield return particles[indices[(index - l) % indices.Length]];
                }
            }

            IEnumerable<Particle> RightNeighbours(int index, int count)
            {
                for (int r = 1; r <= count; r++)
                {
                    yield return particles[indices[(index + r) % indices.Length]];
                }
            }

            int leftNeighbourCount = neighbours / 2;
            int rightNeighbourCount = neighbours / 2 + neighbours % 2;

            for (int i = 0; i < indices.Length; i++)
            {
                particles[indices[i]].Neighbours.AddRange(LeftNeighbours(i, leftNeighbourCount));
                particles[indices[i]].Neighbours.AddRange(RightNeighbours(i, rightNeighbourCount));
            }
        }
    }

    public struct Result
    {
        public double[] Position { get; }

        public double Error { get; }

        public int Iterations { get; }

        public Result(double[] position, double error, int iterations)
        {
            Position = position;
            Error = error;
            Iterations = iterations;
        }
    }
}