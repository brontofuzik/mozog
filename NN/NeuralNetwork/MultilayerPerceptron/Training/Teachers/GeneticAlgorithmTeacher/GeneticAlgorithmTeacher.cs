using GeneticAlgorithm;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers.GeneticAlgorithmTeacher
{
    /// <remarks>
    /// A genetic algorithm teacher.
    /// </remarks>
    public class GeneticAlgorithmTeacher : TeacherBase
    {
        /// <summary>
        /// The network genetic algorithm.
        /// </summary>
        //private readonly GeneticAlgorithm<double> geneticAlgo;

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
            var geneticAlgo = NetworkGeneticAlgorithm.Algorithm(network, trainingSet);

            Result<double> result = geneticAlgo.Run(
                populationSize: 500,
                crossoverRate: 0.8,
                mutationRate: 0.05,
                scaling: false,
                acceptableEvaluation: maxTolerableNetworkError,
                maxGenerations: maxIterationCount);  
            network.SetWeights(result.Solution);

            TrainingLog trainingLog = new TrainingLog(result.Generations, result.Evaluation);
            LogNetworkStatistics(trainingLog, network);

            return trainingLog;
        }
    }
}
