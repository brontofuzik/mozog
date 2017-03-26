using GeneticAlgorithm;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training.Evolutionary
{
    public class GeneticAlgorithmTrainer : TrainerBase
    {
        // TODO
        //private readonly GeneticAlgorithm<double> geneticAlgo;

        public GeneticAlgorithmTrainer(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
            : base(trainingSet, validationSet, testSet)
        {
        }

        public override string Name => "GeneticAlgorithmTrainer";

        public override TrainingLog Train(INetwork network, int maxIterationCount, double maxTolerableNetworkError)
        {
            var geneticAlgo = NetworkGeneticAlgorithm.Algorithm(network, trainingSet, maxTolerableNetworkError, maxIterationCount);

            Result<double> result = geneticAlgo.Run(populationSize: 500, crossoverRate: 0.8, mutationRate: 0.05);  
            network.SetWeights(result.Solution);

            TrainingLog trainingLog = new TrainingLog(result.Generations, result.Evaluation);
            LogNetworkStatistics(trainingLog, network);

            return trainingLog;
        }
    }
}
