using System;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public class ValidationTrainer<TTrainingArgs> : TrainerBase<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        private readonly ITrainer<TTrainingArgs> innerTrainer;

        // Partitions
        private readonly double trainingRatio;
        private DataSet trainingSet;

        private readonly double validationRatio;
        private DataSet validationSet;

        private readonly double testRatio;
        private DataSet testSet;

        public ValidationTrainer(ITrainer<TTrainingArgs> innerTrainer, double trainingRatio, double validationRatio, double testRatio)
        {
            this.innerTrainer = innerTrainer;

            if (trainingRatio + validationRatio + testRatio != 1.0)
            {
                throw new ArgumentException("The sum of ratios must be equal to one.");
            }
            this.trainingRatio = trainingRatio;
            this.validationRatio = validationRatio;
            this.testRatio = testRatio;
        }

        // Holdout validation:
        // https://en.wikipedia.org/wiki/Cross-validation_(statistics)#Holdout_method
        public override TrainingLog Train(INetwork network, DataSet data, TTrainingArgs args)
        {
            PartitionDataSet(data);

            var log = innerTrainer.Train(network, trainingSet, args);
            log.TestSetStats = innerTrainer.Test(network, testSet);

            return log;
        }

        private void PartitionDataSet(DataSet data)
        {
            var points = data.Random().ToArray();

            // Training set
            var split = SplitArray(points, trainingRatio);
            trainingSet = new DataSet(data.InputSize, data.OutputSize).AddRange(split.first);

            // Validation set
            split = SplitArray(split.second, validationRatio / (validationRatio + testRatio));
            validationSet = new DataSet(data.InputSize, data.OutputSize).AddRange(split.first);

            // Testing set
            testSet = new DataSet(data.InputSize, data.OutputSize).AddRange(split.second);
        }

        private static (T[] first, T[] second) SplitArray<T>(T[] array, double ratio)
        {
            int count = (int)Math.Round(ratio * array.Length);
            return (array.Take(count).ToArray(), array.Skip(count).ToArray());
        }

        public override DataStatistics Test(INetwork network, DataSet data)
        {
            throw new NotImplementedException();
        }

        public override event EventHandler<TrainingStatus> TrainingProgress
        {
            add { innerTrainer.TrainingProgress += value; }
            remove { innerTrainer.TrainingProgress -= value; }
        }
    }
}
