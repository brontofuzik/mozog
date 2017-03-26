using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training.Annealing
{
    public class SimulatedAnnealingTrainer : TrainerBase
    {
        private readonly NetworkSimlatedAnnealing networkSimulatedAnnealing = new NetworkSimlatedAnnealing();

        public SimulatedAnnealingTrainer(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override string Name => "SimulatedAnnealingTrainer";

        public override TrainingLog Train(INetwork network, int maxIterationCount, double maxTolerableNetworkError)
        {
            // The network simulated annealing parameters.
            NetworkObjectiveFunction networkObjectiveFunction = new NetworkObjectiveFunction(network, trainingSet);
            double initialTemperature = 1000.0;
            double finalTemperature = 0.001;

            // Train the network.
            int iterationCount;
            double networkError;
            double[] weights = networkSimulatedAnnealing.Run(networkObjectiveFunction,
                maxIterationCount, out iterationCount, maxTolerableNetworkError, out networkError,
                initialTemperature, finalTemperature);
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
