using System;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public abstract class TrainerBase<TTrainingArgs> : ITrainer<TTrainingArgs>
        where TTrainingArgs : ITrainingArgs
    {
        public abstract TrainingLog Train(INetwork network, DataSet data, TTrainingArgs args);

        public virtual DataStatistics Test(INetwork network, DataSet data)
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

        public abstract event EventHandler<TrainingStatus> TrainingProgress;
    }
}
