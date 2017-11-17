namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    // LR = ((LR_f - LR_i) / TIC) * TII + LR_i
    public class LinearLearningRateFunction : LearningRateFunctionBase
    {
        private readonly double learningRate;

        public LinearLearningRateFunction(int iterations, double initialRate, double finalRate)
            : base(iterations, initialRate, finalRate)
        {
            learningRate = (finalRate - initialRate) / iterations;
        }

        public LinearLearningRateFunction(int iterations)
            : base(iterations, MinLearningRate, MaxLearningRate)
        {
        }

        public override double Evaluate(int iteration)
            => InitialRate + learningRate * iteration;
    }
}
