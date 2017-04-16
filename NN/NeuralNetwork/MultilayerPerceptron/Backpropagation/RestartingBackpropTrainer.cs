using NeuralNetwork.Training;

namespace NeuralNetwork.MultilayerPerceptron.Backpropagation
{
    public class RestartingBackpropTrainer : BackpropagationTrainer
    {
        private readonly int restartInterval;

        public RestartingBackpropTrainer(int restartInterval = 0)
        {
            this.restartInterval = restartInterval;
        }

        protected override TrainingLog TrainBackprop()
        {
            args.MaxIterations = restartInterval;

            TrainingLog log;
            do
            {
                log = base.TrainBackprop();
            }
            while (log.TrainingStatistics.AverageError >= args.MaxError);

            return log;
        }
    }
}
