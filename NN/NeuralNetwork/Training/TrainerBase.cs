using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase<TTrainingArgs> : ITrainer<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        public abstract TrainingLog Train(INetwork network, DataSet data, TTrainingArgs args);

        public abstract DataStatistics Test(INetwork network, DataSet data);
    }
}
