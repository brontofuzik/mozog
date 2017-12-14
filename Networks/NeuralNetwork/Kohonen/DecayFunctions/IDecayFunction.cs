namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    public interface ILearningRateFunction
    {
        //double Evaluate(int iteration);

        double Evaluate(double progress);
    }
}
