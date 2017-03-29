using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public interface ITrainer<in TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        TrainingLog Train(INetwork network, TTrainingArgs args);
    }
}
