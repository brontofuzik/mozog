using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <remarks>
    /// A genetic algorithm teacher.
    /// </remarks>
    public class GeneticAlgorithmTeacher
        : TeacherBase
    {
        /// <summary>
        /// The network genetic algorithm.
        /// </summary>
        private readonly NetworkGeneticAlgorithm networkGeneticAlgorithm = new NetworkGeneticAlgorithm();

        /// <summary>
        /// Gets the name of the teacher.
        /// </summary>
        /// <value>
        /// The name of the teacher.
        /// </value>
        public override string Name => "GeneticAlgorithmTeacher";

        /// <summary>
        /// Creates a new genetic teacher.
        /// </summary>
        /// <param name="trainingSet">The training set.</param>
        /// <param name="validationSet">The validation set.</param>
        /// <param name="testSet">The test set.</param>
        public GeneticAlgorithmTeacher(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        /// <summary>
        /// Trains a network.
        /// </summary>
        /// <param name="network">The network to train.</param>
        /// <param name="maxIterationCount">The maximum number of iterations.</param>
        /// <param name="maxTolerableNetworkError">The maximum tolerable network error.</param>
        public override TrainingLog Train(INetwork network, int maxIterationCount, double maxTolerableNetworkError)
        {
            // The network genetic algorithm parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction(network, trainingSet);

            // Train the network.
            int iterationCount;
            double networkError;
            Result<double> result = networkGeneticAlgorithm.Run(networkObjectiveFunction, maxIterationCount, maxTolerableNetworkError,
                populationSize: 500, crossoverRate: 0.8, mutationRate: 0.05, scaling: false);
            network.SetWeights(result.Solution);

            // LOGGING
            // -------

            // Create the training log and log the training data.
            TrainingLog trainingLog = new TrainingLog(result.Generations, result.Evaluation);

            // Log the network statistics.
            LogNetworkStatistics(trainingLog, network);

            return trainingLog;
        }
    }
}
