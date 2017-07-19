using System;
using System.Linq;
using Mozog.Utils;
using NeuralNetwork.Training;

namespace NeuralNetwork.Examples.MultilayerPerceptron
{
    public static class Test
    {
        public static int Average(Func<TrainingLog> trainFunc, int repeats)
        {
            return repeats.Times(() => trainFunc().Iterations).Sum() / repeats;
        }
    }
}
