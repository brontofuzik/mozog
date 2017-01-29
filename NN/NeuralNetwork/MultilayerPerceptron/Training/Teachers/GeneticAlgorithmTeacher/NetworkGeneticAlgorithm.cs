using System;
using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;
using Random = Mozog.Utils.Random;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <summary>
    /// A genetic algorithm designed to train a neural network.
    /// </summary>
    internal static class NetworkGeneticAlgorithm
    {
        public static GeneticAlgorithm<double> Algorithm(INetwork network, TrainingSet trainingSet) => new GeneticAlgorithm<double>(network.SynapseCount)
        {
            InitializationFunction = args =>
            {
                double[] genes = new double[args.ChromosomeSize];
                for (int i = 0; i < args.ChromosomeSize; i++)
                {
                    genes[i] = Random.Double(-10, +10);
                }
                return genes;
            },

            ObjectiveFunction = new NetworkObjectiveFunction(network, trainingSet),

            // The (1-point) crossover function.
            CrossoverOperator = (offspring1, offspring2, args1) =>
            {
                // Choose a point randomly.
                // The point must be located after the first and before the last gene; the point is from the interval [1, chromosomeSize). 
                int point = Random.Int(1, args1.ChromosomeSize);

                // Crossover all genes from point (including) to the end.
                // parent1:    [x1_0, x1_1, ..., x1_point-1, x1_point, ..., x1_size-1]
                // parent2:    [x2_0, x2_1, ..., x2_point-1, x2_point, ..., x2_size-1]
                // offspring1: [x1_0, x1_1, ..., x1_point-1, x2_point, ..., x2_size-1]
                // offspring2: [x2_0, x2_1, ..., x2_point-1, x1_point, ..., x1_size-1]

                int tmpGenesSize = args1.ChromosomeSize - point;
                double[] tmpGenes = new double[tmpGenesSize];

                // "tmpGenes = offspring1Genes"
                Array.Copy(offspring1, point, tmpGenes, 0, tmpGenesSize);
                // "offspring1Genes = offspring2Genes"
                Array.Copy(offspring2, point, offspring1, point, tmpGenesSize);
                // "offspring2Genes = tmpGenes"
                Array.Copy(tmpGenes, 0, offspring2, point, tmpGenesSize);
            },

            MutationOperator = (offspring, args) =>
            {
                int index = Random.Int(0, args.ChromosomeSize);
                offspring[index] = (offspring[index] + Random.Double(-10, +10)) / 2.0;
            }
        };

        class NetworkObjectiveFunction : ObjectiveFunction<double>
        {
            private readonly INetwork network;
            private readonly TrainingSet trainingSet;

            public NetworkObjectiveFunction(INetwork network, TrainingSet trainingSet)
                : base(Objective.Minimize)
            {
                this.network = network;
                this.trainingSet = trainingSet;
            }

            public override double Evaluate(double[] weights)
            {
                network.SetWeights(weights);
                return network.CalculateError(trainingSet);
            }
        }
    }
}
