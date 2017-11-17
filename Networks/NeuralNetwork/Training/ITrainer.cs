using System;
using NeuralNetwork.Data;
using NeuralNetwork.MLP;

namespace NeuralNetwork.Training
{
    public interface ITrainer<in TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        TrainingLog Train(INetwork network, IDataSet data, TTrainingArgs args);

        TestingLog Test(INetwork network, IDataSet data);

        DataStatistics CalculateStats(INetwork network, IDataSet data);

        event EventHandler<TrainingStatus> WeightsUpdated;

        event EventHandler WeightsReset;
    }

    public class TrainingStatus
    {
        public INetwork Network { get; set; }

        public int Iterations { get; private set; }

        public double Error { get; private set; }

        public bool StopTraining { get; set; }

        public TrainingStatus(INetwork network, int iterations, double error)
        {
            Network = network;
            Iterations = iterations;
            Error = error;
        }
    }
}
