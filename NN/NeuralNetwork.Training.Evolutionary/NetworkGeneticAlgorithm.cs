using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.Training.Evolutionary
{
    /// <summary>
    /// A genetic algorithm designed to train a neural network.
    /// </summary>
    internal static class NetworkGeneticAlgorithm
    {
        public static GeneticAlgorithm.GeneticAlgorithm<double> Algorithm(INetwork network, TrainingSet trainingSet, double acceptableEvaluation, int maxIterationCount)
            => new GeneticAlgorithm.GeneticAlgorithm<double>(network.SynapseCount).Configure(cfg => cfg
            .Fitness.Minimize(chromosome =>
            {
                network.SetWeights(chromosome);
                return network.CalculateError(trainingSet);
            })
            .Initialization.Piecewise(() => StaticRandom.Double(-10, +10))
            .Crossover.SinglePoint()
            .Mutation.RandomPoint(gene => (gene + StaticRandom.Double(-10, +10)) / 2.0)
            .Termination.MaxGenerationsOrAcceptableEvaluation(maxGenerations: maxIterationCount, acceptableEvaluation: acceptableEvaluation));
    }
}
