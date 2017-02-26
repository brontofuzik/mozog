using GeneticAlgorithm;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <summary>
    /// A genetic algorithm designed to train a neural network.
    /// </summary>
    internal static class NetworkGeneticAlgorithm
    {
        public static GeneticAlgorithm<double> Algorithm(INetwork network, TrainingSet trainingSet, double acceptableEvaluation, int maxIterationCount)
            => new GeneticAlgorithm<double>(network.SynapseCount).Configure(cfg => cfg
            .Fitness.Minimize(chromosome =>
            {
                network.SetWeights(chromosome);
                return network.CalculateError(trainingSet);
            })
            .Initialization.Piecewise(() => Random.Double(-10, +10))
            .Crossover.SinglePoint()
            .Mutation.RandomPoint(gene => (gene + Random.Double(-10, +10)) / 2.0)
            .Termination.MaxGenerationsOrAcceptableEvaluation(maxGenerations: maxIterationCount, acceptableEvaluation: acceptableEvaluation));
    }
}
