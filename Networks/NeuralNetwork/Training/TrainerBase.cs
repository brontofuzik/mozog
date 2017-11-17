using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Data;
using NeuralNetwork.MLP;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase<TTrainingArgs> : ITrainer<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        public abstract TrainingLog Train(INetwork network, IDataSet data, TTrainingArgs args);

        public TestingLog Test(INetwork network, IDataSet data)
        {
            var stats = CalculateStats(network, data);
            var log = new TestingLog { Statistics = stats };

            // Classifer?
            var classificationData = data as IClassificationData;
            if (classificationData != null)
            {
                var classifierStats = TestClassifier(network, classificationData);
                log.Accuracy = classifierStats.accuracy;
                log.Precision = classifierStats.precision;
                log.Recall = classifierStats.recall;
            }

            return log;
        }

        public virtual DataStatistics CalculateStats(INetwork network, IDataSet data)
        {
            var stats = new DataStatistics(data.Size, network.SynapseCount);
            foreach (var point in data)
            {
                var result = network.EvaluateLabeled(point.Input, point.Output);
                stats.AddObservation(result.output, point.Output, result.error);
            }
            return stats;
        }

        private (double accuracy, double precision, double recall) TestClassifier(INetwork network, IClassificationData data)
        {
            int classCount = data.OutputSize;
            var classes = Enumerable.Range(0, classCount);
            int[,] confusionMatrix = new int[classCount, classCount];

            // Populate the confusion matrix
            foreach (var point in data)
            {
                var trueClass = point.@class;
                var output = network.EvaluateUnlabeled(point.input);
                var outputClass = data.OutputToClass(output);
                confusionMatrix[trueClass, outputClass]++;
            }

            double Mean(IEnumerable<double> values) => values.Sum() / values.Count();

            double ClassPrecision(int @class)
                => confusionMatrix[@class, @class] / (double)classes.Sum(c => confusionMatrix[c, @class]);

            double ClassRecall(int @class)
                => confusionMatrix[@class, @class] / (double)classes.Sum(c => confusionMatrix[@class, c]);

            double accuracy = classes.Sum(c => confusionMatrix[c,c]) / (double)data.Size;
            double precision = Mean(classes.Select(c => ClassPrecision(c)));
            double recall = Mean(classes.Select(c => ClassRecall(c)));
            return (accuracy, precision, recall);
        }

        public abstract event EventHandler<TrainingStatus> WeightsUpdated;

        public abstract event EventHandler WeightsReset;
    }
}
