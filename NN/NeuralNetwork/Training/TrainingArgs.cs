namespace NeuralNetwork.Training
{
    public class TrainingArgs : ITrainingArgs
    {
        public TrainingArgs(int maxIterations, double maxError)
        {
            MaxIterations = maxIterations;
            MaxError = maxError;
        }

        public int MaxIterations { get; }

        public double MaxError { get; }

        public virtual bool IsDone(int iterationCount, double networkError)
            => iterationCount >= MaxIterations || networkError <= MaxError;
    }

    public interface ITrainingArgs
    {
    }
}
