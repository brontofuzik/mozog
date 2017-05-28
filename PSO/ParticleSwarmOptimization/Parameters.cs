using System;

namespace ParticleSwarmOptimization
{
    public class Parameters
    {
        public double W { get; set; }

        public double C1 { get; set; }

        public double C2 { get; set; }

        public double RangeMin { get; set; }

        public double RangeMax { get; set; }

        public int MaxEpochs { get; set; }

        public int MaxStaticEpochs { get; set; }

        public int SwarmSize { get; set; }

        public int MaxNeighbours { get; set; }

        public Parameters Validate()
        {
            if (MaxNeighbours > SwarmSize)
                throw new Exception("Swarm size must be greater than the number of neighbours");

            if (MaxNeighbours < 0)
                throw new Exception("MaxNeighbours cannot be less than zero");

            if (MaxStaticEpochs <= 1)
                throw new Exception("MaxStaticEpochs must be greater than one");

            if (MaxEpochs <= 10)
                throw new Exception("MaxEpochs must be greater than ten");

            if (MaxEpochs < MaxStaticEpochs)
                throw new Exception("MaxStaticEpochs must not be larger than MaxEpochs");

            if (RangeMax <= RangeMin)
                throw new Exception("RangeMax must be greater than RangeMin");

            return this;
        }
    }
}
