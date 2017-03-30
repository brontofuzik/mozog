using System;
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

        public int IterationCount { get; set; }

        public double NetworkError { get; set; }

        /// <summary>
        /// Gets the residual sum of squares (within-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (within-sample).
        /// </value>
        public double RSS_TrainingSet { get; private set; }

        /// <summary>
        /// Gets the residual standard deviation (within-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (within-sample).
        /// </value>
        public double RSD_TrainingSet { get; private set; }

        /// <summary>
        /// Gets the Akaike information criterion.
        /// </summary>
        /// <value>
        /// The Akaike information criterion.
        /// </value>
        public double AIC { get; private set; }

        /// <summary>
        /// Gets the bias-corrected Akaike information criterion.
        /// </summary>
        /// <value>
        /// The bias-corrected Akaike information criterion.
        /// </value>
        public double AICC { get; private set; }

        /// <summary>
        /// Gets the Bayesian information criterion.
        /// </summary>
        /// <value>
        /// The Bayesian information criterion.
        /// </value>
        public double BIC { get; private set; }

        /// <summary>
        /// Gets the Schwarz Bayesian criterion.
        /// </summary>
        /// <value>
        /// The Schwarz Bayesian criterion.
        /// </value>
        public double SBC { get; private set; }

        /// <summary>
        /// Gets the residual sum of squares (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual sum of squares (out-of-sample).
        /// </value>
        public double RSS_TestSet { get; private set; }

        /// <summary>
        /// Gets the residual standard deviation (out-of-sample).
        /// </summary>
        /// <value>
        /// The residual standard deviation (out-of-sample).
        /// </value>
        public double RSD_TestSet { get; private set; }

        public void CalculateMeasuresOfFit(INetwork network, DataSet trainingSet)
        {
            int n = trainingSet.Size;
            int p = network.SynapseCount;

            // Calculate (and log) the residual sum of squares (within-sample).
            RSS_TrainingSet = CalculateRSS(network, trainingSet);

            // Calculate (and log) the residual standard deviation (within-sample).
            RSD_TrainingSet = Math.Sqrt(RSS_TrainingSet / (double)n);

            // Calculate (and log) the Akaike information criterion.
            AIC = n * Math.Log(RSS_TrainingSet / (double)n) + 2 * p;

            // Calcuolate (and log) the bias-corrected Akaike information criterion.
            AICC = AIC + 2 * (p + 1) * (p + 2) / (n - p - 2);

            // Calculate (and log) the Bayesian information criterion.
            BIC = n * Math.Log(RSS_TrainingSet / (double)n) + p + p * Math.Log(n);

            // Calculate (and log) the Schwarz Bayesian criterion.
            SBC = n * Math.Log(RSS_TrainingSet / (double)n) + p * Math.Log(n);
        }

        public void CalculateForecastAccuracy(INetwork network, DataSet testSet)
        {
            int n = testSet.Size;
            int p = network.SynapseCount;

            // Calculate the residual sum of squares (out-of-sample).
            RSS_TestSet = CalculateRSS(network, testSet);

            // Calculate the residual standard deviation (out-of-sample).
            RSD_TestSet = Math.Sqrt(RSS_TestSet / (double)n);
        }

        // TODO Optimize.
        private static double CalculateRSS(INetwork network, DataSet dataSet)
        {
            double rss = 0.0;
            foreach (var point in dataSet)
            {
                double[] output = network.Evaluate(point.Input);
                double[] desiredOutput = point.Output;

                rss += Math.Pow(output[0] - desiredOutput[0], 2);
            }
            return rss;
        }
    }
}
