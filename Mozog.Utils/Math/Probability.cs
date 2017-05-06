using System;

namespace Mozog.Utils.Math
{
    public struct Probability
    {
        private readonly double p;

        public Probability(double p)
        {
            this.p = p;
        }

        public static implicit operator Probability(double p)
            => new Probability(p);

        public static implicit operator Boolean(Probability probability)
            => StaticRandom.Double() < probability.p;
    }
}