using NeuralNetwork.MultilayerPerceptron.Networks;
using NeuralNetwork.MultilayerPerceptron.Training;

namespace NeuralNetwork.Training.Ants
{
    public class AntColonyOptimizationTeacher : TeacherBase
    {
        private readonly NetworkAntColonyOptimization networkAntColonyOptimization = new NetworkAntColonyOptimization();

        public AntColonyOptimizationTeacher(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override string Name => "AntColonyOptimizationTeacher";

        public override TrainingLog Train(INetwork network, int maxIterationCount, double maxTolerableNetworkError)
        {
            // The network ant colony optimiaztion parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction(network, trainingSet);
            int antCount = 100;
            int normalPDFCount = 10;
            double requiredAccuracy = 0.001;

            // Train the network.
            int iterationCount;
            double networkError;
            double[] weights = networkAntColonyOptimization.Run(networkObjectiveFunction,
                maxIterationCount, out iterationCount, maxTolerableNetworkError, out networkError,
                antCount, normalPDFCount, requiredAccuracy
           );
            network.SetWeights(weights);

            // LOGGING
            // -------

            // Create the training log and log the training data.
            TrainingLog trainingLog = new TrainingLog(iterationCount, networkError);

            // Log the network statistics.
            LogNetworkStatistics(trainingLog, network);

            return trainingLog;
        }
    }
}
