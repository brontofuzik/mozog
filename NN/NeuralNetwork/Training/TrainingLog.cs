namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int trainingIterations, double trainingError)
        {
            TrainingIterations = trainingIterations;
            TrainingError = trainingError;
        }

        public int TrainingIterations { get; }

        public double TrainingError { get; }

        public DataStatistics TrainingSetStats { get; set; }

        public DataStatistics ValidationSetStats { get; set; }

        public DataStatistics TestSetStats { get; set; }
    }
}
