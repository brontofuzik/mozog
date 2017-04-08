using System;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public interface ITrainer<in TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        TrainingLog Train(INetwork network, DataSet data, TTrainingArgs args);

        DataStatistics Test(INetwork network, DataSet data);

        event EventHandler<TrainingStatus> TrainingProgress;
    }

    public struct TrainingStatus
    {
        public double Error { get; private set; }

        public int Iterations { get; private set; }

        public TrainingStatus(int iterations, double error)
        {
            Error = error;
            Iterations = iterations;
        }
    }
}
