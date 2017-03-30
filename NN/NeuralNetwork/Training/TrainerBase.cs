using System;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase<TTrainingArgs> : ITrainer<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        protected TrainerBase(DataSet trainingSet, DataSet validationSet, DataSet testSet)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));
            TrainingSet = trainingSet;
            ValidationSet = validationSet;
            TestSet = testSet;
        }

        public DataSet TrainingSet { get; }

        public DataSet ValidationSet { get; }

        public DataSet TestSet { get; }

        public abstract TrainingLog Train(INetwork network, TTrainingArgs args);

        public abstract TrainingLog Train(INetwork network, int maxIterations, double maxError);

        public TrainingLog Train(INetwork network, int maxIterations)
            => Train(network, maxIterations, 0.0);

        public TrainingLog Train(INetwork network, double maxError)
            => Train(network, Int32.MaxValue, maxError);

        public void LogNetworkStatistics(TrainingLog trainingLog, INetwork network)
        {
            // Calculate and log the measures of fit.
            trainingLog.CalculateMeasuresOfFit(network, TrainingSet);

            // Calculate and log the forecast accuracy.
            if (TestSet != null)
            {
                trainingLog.CalculateForecastAccuracy(network, TestSet);
            }
        }
    }
}
