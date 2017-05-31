using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils;
using Mozog.Utils.Math;
using ParticleSwarmOptimization.Functions;

namespace ParticleSwarmOptimization
{
    public class Swarm
    {
        private readonly Particle[] particles;

        private double[] bestGlobalPosition;
        private double bestGlobalError;

        public Swarm(int dimension, int swarmSize, int neighbours)
        {
            Dimension = dimension;

            particles = swarmSize.Times(() => new Particle(this)).ToArray();
            CreateTopology(neighbours);
            //CreateTopology_Randomized(neighbours);

            bestGlobalPosition = particles[0].Position;
            bestGlobalError = particles[0].Error;
        }

        internal int Dimension { get; }

        public double Min { get; set; }

        public double Max { get; set; }

        #region Functions

        public Func<double[], double> ErrorFunc { get; set; }

        #endregion // Functions

        public Result Optimize(ITermination termination)
        {
            int iteration = 0;
            while (!termination.Terminate(iteration, bestGlobalError))
            {
                foreach (var particle in particles)
                {
                    double error = particle.Optimize();

                    if (error < bestGlobalError)
                    {
                        bestGlobalPosition = (double[])particle.Position.Clone();
                        bestGlobalError = error;
                    }
                }
                iteration++;
            }

            return new Result(bestGlobalPosition, bestGlobalError, iteration);
        }

        // Simple termination
        public Result Optimize(int maxIteration = Int32.MaxValue, double targetError = Double.MinValue)
            => Optimize(new SimpleTermination(maxIteration, targetError));

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

                    if (error < bestGlobalError)
                    {
                        bestGlobalPosition = (double[])particle.Position.Clone();
                        bestGlobalError = error;
                        isErrorImproved = true;
                        stagnatingIterations = 0;
                    }
                }

                if (!isErrorImproved)
                    stagnatingIterations++;

                iteration++;
            }

            return new Result(bestGlobalPosition, bestGlobalError, iteration);
        }

        // Stagnating termination
        public Result Optimize_NEW(int maxIterations, int maxStagnatingIterations)
            => Optimize(new StagnatingTermination(maxIterations, maxStagnatingIterations));

        //
        // Manager
        //

        private void CreateTopology(int neighbours)
        {
            IEnumerable<Particle> LeftNeighbours(int index, int count)
            {
                for (int l = 1; l <= count; l++)
                {
                    yield return particles[(index - l) % particles.Length];
                }
            }

            IEnumerable<Particle> RightNeighbours(int index, int count)
            {
                for (int r = 1; r <= count; r++)
                {
                    yield return particles[(index + r) % particles.Length];
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