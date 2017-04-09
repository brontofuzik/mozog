﻿using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase<TTrainingArgs> : ITrainer<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        public abstract TrainingLog Train(INetwork network, IDataSet data, TTrainingArgs args);

        public TestingLog Test<_>(INetwork network, IDataSet data)
        {
            var stats = TestBasic(network, data);
            var log = new TestingLog { DataStats = stats };

            // Classifer?
            var classificationData = data as IEncodedDataSet<_, int>;
            if (classificationData != null)
            {
                var classifierStats = TestClassifier(network, classificationData);
                log.Accuracy = classifierStats.accuracy;
                log.Precision = classifierStats.precision;
                log.Recall = classifierStats.recall;
            }

            return log;
        }

        public virtual DataStatistics TestBasic(INetwork network, IDataSet data)
        {
            var error = 0.0;
            var rss = 0.0;

            foreach (var point in data)
            {
                var result = network.Evaluate(point.Input, point.Output);
                error += result.error;
                rss += Math.Pow(result.output[0] - point.Output[0], 2);
            }

            return new DataStatistics(data.Size, network.SynapseCount, error, rss);
        }

        private (double accuracy, double precision, double recall) TestClassifier<_>(INetwork network, IEncodedDataSet<_, int> data)
        {
            int classCount = data.OutputSize;
            var classes = Enumerable.Range(0, classCount);
            int[,] confusionMatrix = new int[classCount, classCount];

            // Populate the confusion matrix
            foreach (IEncodedDataPoint<_, int> point in data)
            {
                var trueClass = point.OutputTag;
                var result = network.Evaluate(point.Input);
                var classifiedClass = data.Encoder.DecodeOutput(result);
                confusionMatrix[trueClass, classifiedClass]++;
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

        public abstract event EventHandler<TrainingStatus> TrainingProgress;
    }
}
