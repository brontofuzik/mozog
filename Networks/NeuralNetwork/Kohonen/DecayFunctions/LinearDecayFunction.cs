namespace NeuralNetwork.Kohonen.LearningRateFunctions
{
    public class LinearDecayFunction : ILearningRateFunction
    {
        private readonly double initialRate;
        private readonly double totalDecay;

        public LinearDecayFunction(double initialRate, double finalRate)
        {
            this.initialRate = initialRate;
            totalDecay = finalRate - initialRate;
        }

        public double Evaluate(double progress)
            => initialRate + progress * totalDecay;
    }
}
