using System;
using Mozog.Utils;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase
    {
        protected TrainerBase(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));
            TrainingSet = trainingSet;
            ValidationSet = validationSet;
            TestSet = testSet;
        }

        //public abstract string Name { get; }

        public TrainingSet TrainingSet { get; set; }

        public TrainingSet ValidationSet { get; set; }

        public TrainingSet TestSet { get; set; }

        public abstract TrainingLog Train(INetwork network, int maxIterationCount, double maxNetworkError);

        public TrainingLog Train(INetwork network, int maxIterationCount)
            => Train(network, maxIterationCount, 0);

        public TrainingLog Train(INetwork network, double maxNetworkError)
            => Train(network, Int32.MaxValue, maxNetworkError);

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
