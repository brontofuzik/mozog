using System;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public class ValidatingTrainer<TTrainingArgs> : TrainerDecorator<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        private const int ValidationInterval = 10;
        private const int RunLimit = 5;

        private readonly double trainingRatio;
        private readonly double validationRatio;
        private readonly double testRatio;

        private IDataSet trainingSet;
        private IDataSet validationSet;
        private IDataSet testSet;

        private double validationError = Double.MaxValue;
        private int run;

        public ValidatingTrainer(ITrainer<TTrainingArgs> innerTrainer, double trainingRatio, double validationRatio, double testRatio)
            : base(innerTrainer)
        {
            if (trainingRatio + validationRatio + testRatio != 1.0)
            {
                throw new ArgumentException("The sum of ratios must be equal to one.");
            }
            this.trainingRatio = trainingRatio;
            this.validationRatio = validationRatio;
            this.testRatio = testRatio;

            WeightsUpdated += ValidationTrainer_WeightsUpdated;
            WeightsReset += ValidationTrainer_WeightsReset;
        }

        // Holdout validation:
        // https://en.wikipedia.org/wiki/Cross-validation_(statistics)#Holdout_method
        public override TrainingLog Train(INetwork network, IDataSet data, TTrainingArgs args)
        {
            PartitionDataSet(data);

            var log = innerTrainer.Train(network, trainingSet, args);

            log.TrainingSetStats = TestBasic(network, trainingSet);

            if (validationSet != null && validationSet.Size > 0)
                log.ValidationSetStats = TestBasic(network, validationSet);

            if (testSet != null && testSet.Size > 0)
                log.TestSetStats = TestBasic(network, testSet);

            return log;
        }

        private void PartitionDataSet(IDataSet data)
        {
            var points = data.Random().ToArray();

            // Training set
            var split = SplitArray(points, trainingRatio);
            trainingSet = data.CreateNewSet().AddRange(split.first);

            // Validation set
            split = SplitArray(split.second, validationRatio / (validationRatio + testRatio));
            validationSet = data.CreateNewSet().AddRange(split.first);

            // Testing set
            testSet = data.CreateNewSet().AddRange(split.second);
        }

        private static (T[] first, T[] second) SplitArray<T>(T[] array, double ratio)
        {
            int count = (int)Math.Round(ratio * array.Length);
            return (array.Take(count).ToArray(), array.Skip(count).ToArray());
        }

        private void ValidationTrainer_WeightsUpdated(object sender, TrainingStatus e)
        {
            if (e.Iterations % ValidationInterval != 0) return;

            var result = TestBasic(e.Network, validationSet);

            if (result.Error > validationError)
            {
                run++;
                if (run == RunLimit)
                {
                    e.StopTraining = true;
                }
            }
            else
            {
                run = 0;
                validationError = result.Error;
                e.StopTraining = false;
            }
        }

        private void ValidationTrainer_WeightsReset(object sender, EventArgs e)
        {
            run = 0;
            validationError = Double.MaxValue;
        }
    }
}
