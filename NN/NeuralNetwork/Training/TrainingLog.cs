namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int trainingIterations)
        {
            Iterations = trainingIterations;
        }

        public int Iterations { get; }

        public DataStatistics TrainingSetStats { get; set; }

        public DataStatistics ValidationSetStats { get; set; }

        public DataStatistics TestSetStats { get; set; }
    }
}
