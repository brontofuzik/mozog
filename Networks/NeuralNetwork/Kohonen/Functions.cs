using System;
using static Mozog.Utils.Math.Math;

namespace NeuralNetwork.Kohonen
{
    public delegate double DecayFunction(double progress);

    public static class Decay
    {
        public static DecayFunction Linear(double initialRate, double finalRate)
        {
            var totalDecay = finalRate - initialRate;
            return progress => initialRate + progress * totalDecay;
        }

        public static DecayFunction Exponential(double initialRate, double finalRate)
        {
            const double SampleRate = 1e6;
            var decayRate = Math.Pow(finalRate / initialRate, 1 / SampleRate);
            return progress => initialRate * Math.Pow(decayRate, progress * SampleRate);
        }
    }

    public delegate double NeighbourhoodFunction(double distance, double radius);

    public static class Neighbourhood
    {
        public static NeighbourhoodFunction Bubble()
        {
            return (distance, radius) => distance <= Math.Abs(radius) ? 1.0 : 0.0;
        }

        public static NeighbourhoodFunction Gaussian()
        {
            return (distance, radius) => Math.Exp(-Square(distance / (2 * Square(radius))));
        }
    }
}