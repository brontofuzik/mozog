using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public interface ITrainer
    {
        TrainingLog Train(INetwork network, int maxIterationCount, double maxNetworkError);
    }
}
