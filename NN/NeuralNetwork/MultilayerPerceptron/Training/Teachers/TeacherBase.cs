using System;
using Mozog.Utils;
using NeuralNetwork.MultilayerPerceptron.Networks;

namespace NeuralNetwork.MultilayerPerceptron.Training.Teachers
{
    public abstract class TeacherBase
    {
        protected TrainingSet trainingSet;
        protected TrainingSet validationSet;
        protected TrainingSet testSet;

        protected TeacherBase(TrainingSet trainingSet, TrainingSet validationSet, TrainingSet testSet)
        {
            Require.IsNotNull(trainingSet, nameof(trainingSet));
            this.trainingSet = trainingSet;

            this.validationSet = validationSet;
            this.testSet = testSet;
        }

        public abstract string Name { get; }

        public TrainingSet TrainingSet => trainingSet;

        public TrainingSet ValidationSet => validationSet;

        public TrainingSet TestSet => testSet;

        public abstract TrainingLog Train(INetwork network, int maxIterationCount, double maxNetworkError);

        public TrainingLog Train(INetwork network, int maxIterationCount) => Train(network, maxIterationCount, 0);

        public TrainingLog Train(INetwork network, double maxNetworkError) => Train(network, Int32.MaxValue, maxNetworkError);

        public void LogNetworkStatistics(TrainingLog trainingLog, INetwork network)
        {
            // Calculate and log the measures of fit.
            trainingLog.CalculateMeasuresOfFit(network, trainingSet);

            // Calculate and log the forecast accuracy.
            if (testSet != null)
            {
                trainingLog.CalculateForecastAccuracy(network, testSet);
            }
        }
    }
}
