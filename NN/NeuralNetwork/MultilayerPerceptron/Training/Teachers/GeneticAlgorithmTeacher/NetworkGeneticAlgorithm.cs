using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <summary>
    /// A genetic algorithm designed to train a neural network.
    /// </summary>
    internal static class NetworkGeneticAlgorithm
    {
        public static GeneticAlgorithm<double> Algorithm(INetwork network, TrainingSet trainingSet) =>
            new GeneticAlgorithm<double>(network.SynapseCount,
            objective: ObjectiveFunction<double>.Minimize(chromosome =>
            {
                network.SetWeights(chromosome);
                return network.CalculateError(trainingSet);
            }),
            initialization: PiecewiseInitialization<double>(_ => Random.Double(-10, +10)),
            crossover: SinglePointCrossover<double>(),
            mutation: RandomPointMutation<double>((gene, _) => (gene + Random.Double(-10, +10)) / 2.0)
        );
    }
}
