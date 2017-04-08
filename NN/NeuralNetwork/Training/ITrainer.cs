using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public interface ITrainer<in TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        TrainingLog Train(INetwork network, DataSet data, TTrainingArgs args);

        DataStatistics Test(INetwork network, DataSet data);
    }
}
