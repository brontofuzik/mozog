namespace NeuralNetwork.Training
{
    public class TrainingArgs : ITrainingArgs
    {
        public TrainingArgs(int maxIterations, double maxError)
        {
            MaxIterations = maxIterations;
            MaxError = maxError;
        }

        public int MaxIterations { get; set; }

        public double MaxError { get; }
    }

    public interface ITrainingArgs
    {
    }
}
