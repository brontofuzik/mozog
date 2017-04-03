using System;
using System.Linq;
using NeuralNetwork.Interfaces;

namespace NeuralNetwork.Training
{
    public class TrainingLog
    {
        public TrainingLog(int iterationCount, double networkError)
        {
            IterationCount = iterationCount;
            NetworkError = networkError;
        }

        public int IterationCount { get; private set; }

        public double NetworkError { get; private set; }

        // Residual sum of squares (within-sample)
        public double RSS_TrainingSet { get; private set; }

        // Residual standard deviation (within-sample)
        public double RSD_TrainingSet { get; private set; }

        // Mean squared error
        public double MSE_TrainingSet { get; private set; }

        // Akaike information criterion
        public double AIC { get; private set; }

        // Bias-corrected Akaike information criterion
        public double AICC { get; private set; }

        // Bayesian information criterion
        public double BIC { get; private set; }

        /// Residual sum of squares (out-of-sample)
        public double RSS_TestSet { get; private set; }

        // Residual standard deviation (out-of-sample).
        public double RSD_TestSet { get; private set; }

        // Training set performance
        public void CalculateMeasuresOfFit(INetwork network, DataSet trainingSet)
        {
            int n = trainingSet.Size;
            int p = network.SynapseCount;

            RSS_TrainingSet = CalculateRSS(network, trainingSet);
            RSD_TrainingSet = Math.Sqrt(RSS_TrainingSet / (n - 2));
            MSE_TrainingSet = RSS_TrainingSet / n;
            AIC = n * Math.Log(MSE_TrainingSet) + 2 * p;
            AICC = AIC + 2 * (p + 1) * (p + 2) / (n - p - 2);
            BIC = n * Math.Log(MSE_TrainingSet) + p * Math.Log(n);
        }

        // Test set performance
        public void CalculateForecastAccuracy(INetwork network, DataSet testSet)
        {
            int n = testSet.Size;
            int p = network.SynapseCount;

            RSS_TestSet = CalculateRSS(network, testSet);
            RSD_TestSet = Math.Sqrt(RSS_TestSet / (double)n);
        }

        private static double CalculateRSS(INetwork network, DataSet data)
        {
            double SinglePoint(LabeledDataPoint point)
            {
                var output = network.Evaluate(point.Input);
                var expectedOutput = point.Output;
                return Math.Pow(output[0] - expectedOutput[0], 2);
            }

            return data.Sum(point => SinglePoint(point));
        }
    }
}
