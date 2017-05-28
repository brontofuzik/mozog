using System;
using System.Collections.Generic;
using System.Linq;
using Mozog.Utils.Math;

namespace ParticleSwarmOptimization
{
    public class Swarm
    {
        private readonly Parameters parameters;

        private readonly IParticle[] particles;

        // Factory method
        public static Swarm Build(int dimensions, Func<double[], double> errorFunc, Parameters parameters)
        {
            var particles = Enumerable.Range(0, parameters.SwarmSize)
                .Select(i => new Particle(dimensions, errorFunc, parameters))
                .ToArray();
            return new Swarm(particles, parameters);
        }

        public Swarm(IParticle[] particles, Parameters parameters)
        {          
            this.particles = particles;
            this.parameters = parameters;

            CreateTopology();
            //CreateTopology_Randomized();

            BestGlobalPosition = particles[0].Position;
            BestGlobalError = particles[0].Error;
        }

        public double[] BestGlobalPosition { get; private set; }

        public double BestGlobalError { get; private set; }

        public (double[] position, double error) Optimize()
        {
            int epoch = 0;
            int staticEpoch = 0;

            while (epoch < parameters.MaxEpochs && staticEpoch < parameters.MaxStaticEpochs)
            {
                bool isErrorImproved = false;
                foreach (var particle in particles)
                {
                    double error = particle.Optimize();

                    if (error < BestGlobalError)
                    {
                        BestGlobalPosition = (double[])particle.Position.Clone();
                        BestGlobalError = error;
                        isErrorImproved = true;
                        staticEpoch = 0;
                    }
                }

                if (!isErrorImproved)
                    staticEpoch++;

                epoch++;
            }

            return (BestGlobalPosition, BestGlobalError);
        }

        //
        // Manager
        //

        private void CreateTopology()
        {
            IEnumerable<IParticle> LeftNeighbours(int index, int count)
            {
                for (int l = 1; l <= count; l++)
                {
                    yield return particles[(index - l) % particles.Length];
                }
            }

            IEnumerable<IParticle> RightNeighbours(int index, int count)
            {
                for (int r = 1; r <= count; r++)
                {
                    yield return particles[(index + r) % particles.Length];
                }
            }

            int leftNeighbourCount = parameters.MaxNeighbours / 2;
            int rightNeighbourCount = parameters.MaxNeighbours / 2 + parameters.MaxNeighbours % 2;

            for (int i = 0; i < particles.Length; i++)
            {
                particles[i].Neighbours.AddRange(LeftNeighbours(i, leftNeighbourCount));
                particles[i].Neighbours.AddRange(RightNeighbours(i, rightNeighbourCount));
            }
        }

        private void CreateTopology_Randomized()
        {
            var indices = Enumerable.Range(0, particles.Length).ToArray();
            indices = StaticRandom.Shuffle(indices);

            IEnumerable<IParticle> LeftNeighbours(int index, int count)
            {
                for (int l = 1; l <= count; l++)
                {
                    yield return particles[indices[(index - l) % indices.Length]];
                }
            }

            IEnumerable<IParticle> RightNeighbours(int index, int count)
            {
                for (int r = 1; r <= count; r++)
                {
                    yield return particles[indices[(index + r) % indices.Length]];
                }
            }

            int leftNeighbourCount = parameters.MaxNeighbours / 2;
            int rightNeighbourCount = parameters.MaxNeighbours / 2 + parameters.MaxNeighbours % 2;

            for (int i = 0; i < indices.Length; i++)
            {
                particles[indices[i]].Neighbours.AddRange(LeftNeighbours(i, leftNeighbourCount));
                particles[indices[i]].Neighbours.AddRange(RightNeighbours(i, rightNeighbourCount));
            }
        }
    }
}